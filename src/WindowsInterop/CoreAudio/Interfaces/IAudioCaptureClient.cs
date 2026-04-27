namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioCaptureClient interface enables a client to read input data from a capture endpoint buffer.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nn-audioclient-iaudiocaptureclient"></a></remarks>
[Guid("C8ADBD64-E71E-48a0-A4DE-185C395CD317")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComImport]
public interface IAudioCaptureClient
{
    /// <summary>
    /// Retrieves a pointer to the next available packet of data in the capture endpoint buffer.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudiocaptureclient-getbuffer"></a></remarks>
    int GetBuffer(out IntPtr dataBuffer, out int numFramesToRead, out AudioClientBufferFlags bufferFlags, out long devicePosition, out long qpcPosition);

    /// <summary>
    /// The ReleaseBuffer method releases the buffer.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudiocaptureclient-releasebuffer"></a></remarks>
    int ReleaseBuffer(int numFramesRead);

    /// <summary>
    /// The GetNextPacketSize method retrieves the number of frames in the next data packet in the capture endpoint buffer.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudiocaptureclient-getnextpacketsize"></a></remarks>
    int GetNextPacketSize(out int numFramesInNextPacket);
}
