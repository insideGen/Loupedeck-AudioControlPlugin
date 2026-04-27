namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

public class AudioCaptureClient : IDisposable
{
    private readonly IAudioCaptureClient audioCaptureClientComObj;

    public AudioCaptureClient(IAudioCaptureClient audioCaptureClientComObj)
    {
        this.audioCaptureClientComObj = audioCaptureClientComObj;
    }

    public IntPtr GetBuffer(out int numFramesToRead, out AudioClientBufferFlags bufferFlags, out long devicePosition, out long qpcPosition)
    {
        Marshal.ThrowExceptionForHR(this.audioCaptureClientComObj.GetBuffer(out IntPtr bufferPointer, out numFramesToRead, out bufferFlags, out devicePosition, out qpcPosition));
        return bufferPointer;
    }

    public IntPtr GetBuffer(out int numFramesToRead, out AudioClientBufferFlags bufferFlags)
    {
        Marshal.ThrowExceptionForHR(this.audioCaptureClientComObj.GetBuffer(out IntPtr bufferPointer, out numFramesToRead, out bufferFlags, out long _, out long _));
        return bufferPointer;
    }

    public void ReleaseBuffer(int numFramesWritten)
    {
        Marshal.ThrowExceptionForHR(this.audioCaptureClientComObj.ReleaseBuffer(numFramesWritten));
    }

    public int GetNextPacketSize()
    {
        Marshal.ThrowExceptionForHR(this.audioCaptureClientComObj.GetNextPacketSize(out int numFramesInNextPacket));
        return numFramesInNextPacket;
    }

    public void Dispose()
    {
        try
        {
            Marshal.ReleaseComObject(this.audioCaptureClientComObj);
        }
        catch
        {
        }
        GC.SuppressFinalize(this);
    }

    ~AudioCaptureClient()
    {
        this.Dispose();
    }
}
