namespace WindowsInterop.CoreAudio;

using System.Threading;

public class WaveIn : IDisposable
{
    private readonly AutoResetEvent callbackEvent;
    private volatile WaveInCaptureState captureState;
    private IntPtr waveInHandle;
    private WaveInBuffer[] buffers;

    public event EventHandler<WaveInEventArgs> DataAvailable;

    /// <summary>
    /// Milliseconds for the buffer. Recommended value is 100ms.
    /// </summary>
    public int BufferMilliseconds { get; set; }

    /// <summary>
    /// Number of Buffers to use (usually 2 or 3).
    /// </summary>
    public int NumberOfBuffers { get; set; }

    /// <summary>
    /// The device number to use.
    /// </summary>
    public int DeviceNumber { get; set; }

    /// <summary>
    /// WaveFormat we are recording in.
    /// </summary>
    public WaveFormat WaveFormat { get; set; }

    public WaveIn()
    {
        this.callbackEvent = new AutoResetEvent(initialState: false);
        this.BufferMilliseconds = 100;
        this.NumberOfBuffers = 3;
        this.DeviceNumber = 0;
        this.WaveFormat = new WaveFormat(8000, 16, 1);
        this.captureState = WaveInCaptureState.Stopped;
    }

    private void CreateBuffers()
    {
        // Default to three buffers of 100ms each.
        int bufferSize = this.BufferMilliseconds * this.WaveFormat.AvgBytesPerSecond / 1000;
        if (bufferSize % this.WaveFormat.BlockAlign != 0)
        {
            bufferSize -= bufferSize % this.WaveFormat.BlockAlign;
        }
        this.buffers = new WaveInBuffer[this.NumberOfBuffers];
        for (int i = 0; i < this.buffers.Length; i++)
        {
            this.buffers[i] = new WaveInBuffer(this.waveInHandle, bufferSize);
        }
    }

    private void CloseWaveInDevice()
    {
        // Some drivers need the reset to properly release buffers.
        if (WaveInterop.waveInReset(this.waveInHandle) != 0)
        {
            //throw new Exception("waveInReset");
        }
        if (this.buffers != null)
        {
            for (int i = 0; i < this.buffers.Length; i++)
            {
                this.buffers[i].Dispose();
            }
            this.buffers = null;
        }
        if (WaveInterop.waveInClose(this.waveInHandle) != 0)
        {
            //throw new Exception("waveInClose");
        }
        this.waveInHandle = IntPtr.Zero;
    }

    private void OpenWaveInDevice()
    {
        this.CloseWaveInDevice();
        if (WaveInterop.waveInOpen(out this.waveInHandle, (IntPtr)this.DeviceNumber, this.WaveFormat, this.callbackEvent.SafeWaitHandle.DangerousGetHandle(), IntPtr.Zero, WaveInterop.WaveInOutOpenFlags.CallbackEvent) != 0)
        {
            throw new Exception("waveInOpen");
        }
        this.CreateBuffers();
    }

    private void RecordThread()
    {
        Exception exception = null;
        try
        {
            this.DoRecording();
        }
        catch (Exception e)
        {
            exception = e;
        }
        finally
        {
            this.captureState = WaveInCaptureState.Stopped;
        }
    }

    private void DoRecording()
    {
        this.captureState = WaveInCaptureState.Capturing;
        foreach (WaveInBuffer buffer in this.buffers)
        {
            if (!buffer.InQueue)
            {
                buffer.Reuse();
            }
        }
        while (this.captureState == WaveInCaptureState.Capturing)
        {
            if (this.callbackEvent.WaitOne())
            {
                // Requeue any buffers returned to us.
                foreach (WaveInBuffer buffer in this.buffers)
                {
                    if (buffer.Done)
                    {
                        if (buffer.BytesRecorded > 0)
                        {
                            DataAvailable?.Invoke(this, new WaveInEventArgs(buffer.Data, buffer.BytesRecorded));
                        }

                        if (this.captureState == WaveInCaptureState.Capturing)
                        {
                            buffer.Reuse();
                        }
                    }
                }
            }
        }
    }

    public void StartRecording()
    {
        if (this.captureState != WaveInCaptureState.Stopped)
        {
            throw new InvalidOperationException("Already recording");
        }
        this.OpenWaveInDevice();
        if (WaveInterop.waveInStart(this.waveInHandle) != 0)
        {
            throw new Exception("waveInStart");
        }
        this.captureState = WaveInCaptureState.Starting;
        ThreadPool.QueueUserWorkItem((state) => this.RecordThread(), null);
    }

    public void StopRecording()
    {
        if (this.captureState != WaveInCaptureState.Stopped)
        {
            this.captureState = WaveInCaptureState.Stopping;
            if (WaveInterop.waveInStop(this.waveInHandle) != 0)
            {
                throw new Exception("waveInStop");
            }
            if (WaveInterop.waveInReset(this.waveInHandle) != 0)
            {
                throw new Exception("waveInReset");
            }
            this.callbackEvent.Set(); // Signal the thread to exit.
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        this.StopRecording();
        this.CloseWaveInDevice();
    }

    ~WaveIn()
    {
        this.Dispose();
    }
}
