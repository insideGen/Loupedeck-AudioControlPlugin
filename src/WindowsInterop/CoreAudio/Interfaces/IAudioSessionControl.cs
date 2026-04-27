namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioSessionControl interface enables a client to configure the control parameters for an audio session
/// and to monitor events in the session.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nn-audiopolicy-iaudiosessioncontrol"></a></remarks>
[Guid("F4B1A599-7266-4319-A8CA-E70ACB11E8CD")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IAudioSessionControl
{
    /// <summary>
    /// The GetState method retrieves the current state of the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-getstate"></a></remarks>
    [PreserveSig]
    int GetState([Out] out AudioSessionState state);

    /// <summary>
    /// The GetDisplayName method retrieves the display name for the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-getdisplayname"></a></remarks>
    [PreserveSig]
    int GetDisplayName([Out][MarshalAs(UnmanagedType.LPWStr)] out string displayName);

    /// <summary>
    /// The SetDisplayName method assigns a display name to the current session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-setdisplayname"></a></remarks>
    [PreserveSig]
    int SetDisplayName([In][MarshalAs(UnmanagedType.LPWStr)] string displayName, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    /// <summary>
    /// The GetIconPath method retrieves the path for the display icon for the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-geticonpath"></a></remarks>
    [PreserveSig]
    int GetIconPath([Out][MarshalAs(UnmanagedType.LPWStr)] out string iconPath);

    /// <summary>
    /// The SetIconPath method assigns a display icon to the current session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-seticonpath"></a></remarks>
    [PreserveSig]
    int SetIconPath([In][MarshalAs(UnmanagedType.LPWStr)] string iconPath, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    /// <summary>
    /// The GetGroupingParam method retrieves the grouping parameter of the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-getgroupingparam"></a></remarks>
    [PreserveSig]
    int GetGroupingParam([Out] out Guid groupingId);

    /// <summary>
    /// The SetGroupingParam method assigns a session to a grouping of sessions.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-setgroupingparam"></a></remarks>
    [PreserveSig]
    int SetGroupingParam([In][MarshalAs(UnmanagedType.LPStruct)] Guid groupingId, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    /// <summary>
    /// The RegisterAudioSessionNotification method registers the client to receive notifications of session events,
    /// including changes in the stream state.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-registeraudiosessionnotification"></a></remarks>
    [PreserveSig]
    int RegisterAudioSessionNotification([In] IAudioSessionEvents client);

    /// <summary>
    /// The UnregisterAudioSessionNotification method deletes a previous registration by the client to receive notifications.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-unregisteraudiosessionnotification"></a></remarks>
    [PreserveSig]
    int UnregisterAudioSessionNotification([In] IAudioSessionEvents client);
}
