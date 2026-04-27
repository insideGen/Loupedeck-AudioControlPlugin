namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

internal class WaveInBuffer : IDisposable
{
    private readonly WaveHeader header;
    private GCHandle hBuffer;
    private IntPtr waveInHandle;
    private GCHandle hHeader; // We need to pin the header structure
    private GCHandle hThis; // for the user callback

    /// <summary>
    /// Provides access to the actual record buffer (for reading only).
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// Indicates whether the Done flag is set on this buffer.
    /// </summary>
    public bool Done
    {
        get
        {
            return (this.header.flags & WaveHeaderFlags.Done) == WaveHeaderFlags.Done;
        }
    }

    /// <summary>
    /// Indicates whether the InQueue flag is set on this buffer.
    /// </summary>
    public bool InQueue
    {
        get
        {
            return (this.header.flags & WaveHeaderFlags.InQueue) == WaveHeaderFlags.InQueue;
        }
    }

    /// <summary>
    /// Number of bytes recorded.
    /// </summary>
    public int BytesRecorded
    {
        get
        {
            return this.header.bytesRecorded;
        }
    }

    /// <summary>
    /// The buffer size in bytes.
    /// </summary>
    public int BufferSize { get; }

    /// <summary>
    /// creates a new Wave buffer.
    /// </summary>
    /// <param name="waveInHandle">WaveIn device to write to.</param>
    /// <param name="bufferSize">Buffer size in bytes.</param>
    public WaveInBuffer(IntPtr waveInHandle, int bufferSize)
    {
        this.BufferSize = bufferSize;
        this.Data = new byte[bufferSize];
        this.hBuffer = GCHandle.Alloc(this.Data, GCHandleType.Pinned);
        this.waveInHandle = waveInHandle;
        this.header = new WaveHeader();
        this.hHeader = GCHandle.Alloc(this.header, GCHandleType.Pinned);
        this.header.data = this.hBuffer.AddrOfPinnedObject();
        this.header.bufferLength = bufferSize;
        this.header.loops = 1;
        this.hThis = GCHandle.Alloc(this);
        this.header.user = (IntPtr)this.hThis;
        if (WaveInterop.waveInPrepareHeader(waveInHandle, this.header, Marshal.SizeOf(this.header)) != 0)
        {
            throw new Exception("waveInPrepareHeader");
        }
    }

    /// <summary>
    /// Place this buffer back to record more audio.
    /// </summary>
    public void Reuse()
    {
        // TEST: we might not actually need to bother unpreparing and repreparing.
        if (WaveInterop.waveInUnprepareHeader(this.waveInHandle, this.header, Marshal.SizeOf(this.header)) != 0)
        {
            throw new Exception("waveInUnprepareHeader");
        }
        if (WaveInterop.waveInPrepareHeader(this.waveInHandle, this.header, Marshal.SizeOf(this.header)) != 0)
        {
            throw new Exception("waveInPrepareHeader");
        }
        //System.Diagnostics.Debug.Assert(header.bytesRecorded == 0, "bytes recorded was not reset properly");
        if (WaveInterop.waveInAddBuffer(this.waveInHandle, this.header, Marshal.SizeOf(this.header)) != 0)
        {
            throw new Exception("waveInAddBuffer");
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if (this.waveInHandle != IntPtr.Zero)
        {
            if (WaveInterop.waveInUnprepareHeader(this.waveInHandle, this.header, Marshal.SizeOf(this.header)) != 0)
            {
                throw new Exception("waveInUnprepareHeader");
            }
            this.waveInHandle = IntPtr.Zero;
        }
        if (this.hHeader.IsAllocated)
        {
            this.hHeader.Free();
        }
        if (this.hBuffer.IsAllocated)
        {
            this.hBuffer.Free();
        }
        if (this.hThis.IsAllocated)
        {
            this.hThis.Free();
        }
    }

    ~WaveInBuffer()
    {
        this.Dispose();
    }
}
