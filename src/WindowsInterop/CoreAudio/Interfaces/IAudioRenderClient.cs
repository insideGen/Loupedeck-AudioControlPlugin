namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioRenderClient interface enables a client to write output data to a rendering endpoint buffer.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nn-audioclient-iaudiorenderclient"></a></remarks>
[Guid("F294ACFC-3146-4483-A7BF-ADDCA7C260E2")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IAudioRenderClient
{
    /// <summary>
    /// Retrieves a pointer to the next available space in the rendering endpoint buffer into which the caller can write a data packet.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudiorenderclient-getbuffer"></a></remarks>
    int GetBuffer(int numFramesRequested, out IntPtr dataBufferPointer);

    /// <summary>
    /// The ReleaseBuffer method releases the buffer space acquired in the previous call to the IAudioRenderClient::GetBuffer method.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudiorenderclient-releasebuffer"></a></remarks>
    int ReleaseBuffer(int numFramesWritten, AudioClientBufferFlags bufferFlags);
}
