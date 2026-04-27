namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioEndpointVolumeCallback interface provides notifications of changes in the volume level
/// and muting state of an audio endpoint device.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nn-endpointvolume-iaudioendpointvolumecallback"></a></remarks>
[Guid("657804FA-D6AD-4496-8A60-352752AF4F89")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IAudioEndpointVolumeCallback
{
    /// <summary>
    /// The OnNotify method notifies the client that the volume level or muting state of the audio endpoint device has changed.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolumecallback-onnotify"></a></remarks>
    void OnNotify(IntPtr notifyData);
}
