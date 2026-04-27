namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioMute interface provides access to a hardware mute control.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nn-devicetopology-iaudiomute"></a></remarks>
[Guid("DF45AEEA-B74A-4B6B-AFAD-2366B6AA012E")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IAudioMute
{
    /// <summary>
    /// The GetMute method gets the current state (enabled or disabled) of the mute control.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iaudiomute-getmute"></a></remarks>
    [PreserveSig]
    int GetMute([Out, MarshalAs(UnmanagedType.Bool)] out bool mute);

    /// <summary>
    /// The SetMute method enables or disables the mute control.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iaudiomute-setmute"></a></remarks>
    [PreserveSig]
    int SetMute([In, MarshalAs(UnmanagedType.Bool)] bool mute, [In] ref Guid eventContext);
}
