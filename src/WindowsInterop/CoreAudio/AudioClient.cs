namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

public class AudioClient : IDisposable
{
    private readonly IAudioClient audioClientComObj;

    private AudioCaptureClient _audioCaptureClient = null;

    public AudioCaptureClient AudioCaptureClient
    {
        get
        {
            if (this._audioCaptureClient == null)
            {
                try
                {
                    Marshal.ThrowExceptionForHR(this.audioClientComObj.GetService(typeof(AudioCaptureClient).GUID, out object audioCaptureClientComObj));
                    this._audioCaptureClient = new AudioCaptureClient(audioCaptureClientComObj as IAudioCaptureClient);
                }
                catch
                {
                    this._audioCaptureClient = null;
                }
            }
            return this._audioCaptureClient;
        }
    }

    public AudioClient(IAudioClient audioClientComObj)
    {
        this.audioClientComObj = audioClientComObj;
    }

    public void GetDevicePeriod(out long defaultDevicePeriod, out long minimumDevicePeriod)
    {
        Marshal.ThrowExceptionForHR(this.audioClientComObj.GetDevicePeriod(out defaultDevicePeriod, out minimumDevicePeriod));
    }

    public WaveFormat GetMixFormat()
    {
        Marshal.ThrowExceptionForHR(this.audioClientComObj.GetMixFormat(out IntPtr waveFormatPointer));
        WaveFormat waveFormat = WaveFormat.MarshalFromPtr(waveFormatPointer);
        Marshal.FreeCoTaskMem(waveFormatPointer);
        return waveFormat;
    }

    public bool IsFormatSupported(AudioClientShareMode shareMode, WaveFormat format, out WaveFormat closestMatchFormat)
    {
        int result = this.audioClientComObj.IsFormatSupported(shareMode, format, out IntPtr closestMatchPointer);
        if (closestMatchPointer != IntPtr.Zero)
        {
            closestMatchFormat = WaveFormat.MarshalFromPtr(closestMatchPointer);
            Marshal.FreeCoTaskMem(closestMatchPointer);
        }
        else
        {
            closestMatchFormat = null;
        }
        return result == 0;
    }

    public void Initialize(AudioClientShareMode shareMode, AudioClientStreamFlags streamFlags, long bufferDuration, long periodicity, WaveFormat format, Guid audioSessionGuid)
    {
        int result = this.audioClientComObj.Initialize(shareMode, streamFlags, bufferDuration, periodicity, format, audioSessionGuid);
    }

    public uint GetBufferSize()
    {
        this.audioClientComObj.GetBufferSize(out uint numBufferFrames);
        return numBufferFrames;
    }

    public void SetEventHandle(IntPtr eventWaitHandle)
    {
        Marshal.ThrowExceptionForHR(this.audioClientComObj.SetEventHandle(eventWaitHandle));
    }

    public void Start()
    {
        Marshal.ThrowExceptionForHR(this.audioClientComObj.Start());
    }

    public void Stop()
    {
        Marshal.ThrowExceptionForHR(this.audioClientComObj.Stop());
    }

    public void Dispose()
    {
        this.Stop();
        this.AudioCaptureClient.Dispose();
        try
        {
            Marshal.ReleaseComObject(this.audioClientComObj);
        }
        catch
        {
        }
        GC.SuppressFinalize(this);
    }

    ~AudioClient()
    {
        this.Dispose();
    }
}
