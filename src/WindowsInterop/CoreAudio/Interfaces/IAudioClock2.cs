namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioClock2 interface is used to get the current device position.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nn-audioclient-iaudioclock2"></a></remarks>
[Guid("6F49FF73-6727-49AC-A008-D98CF5E70048")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IAudioClock2 : IAudioClock
{
    /// <summary>
    /// The GetDevicePosition method gets the current device position, in frames, directly from the hardware.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclock2-getdeviceposition"></a></remarks>
    [PreserveSig]
    int GetDevicePosition(out ulong devicePosition, out ulong qpcPosition);
}
