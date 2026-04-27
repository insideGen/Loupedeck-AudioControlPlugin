namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioSessionControl2 interface can be used by a client to get information about the audio session.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nn-audiopolicy-iaudiosessioncontrol2"></a></remarks>
[Guid("BfB7FF88-7239-4FC9-8FA2-07C950BE9C6D")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IAudioSessionControl2 : IAudioSessionControl
{
    /// <summary>
    /// The GetState method retrieves the current state of the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-getstate"></a></remarks>
    [PreserveSig]
    new int GetState([Out] out AudioSessionState state);

    /// <summary>
    /// The GetDisplayName method retrieves the display name for the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-getdisplayname"></a></remarks>
    [PreserveSig]
    new int GetDisplayName([Out][MarshalAs(UnmanagedType.LPWStr)] out string displayName);

    /// <summary>
    /// The SetDisplayName method assigns a display name to the current session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-setdisplayname"></a></remarks>
    [PreserveSig]
    new int SetDisplayName([In][MarshalAs(UnmanagedType.LPWStr)] string displayName, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    /// <summary>
    /// The GetIconPath method retrieves the path for the display icon for the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-geticonpath"></a></remarks>
    [PreserveSig]
    new int GetIconPath([Out][MarshalAs(UnmanagedType.LPWStr)] out string iconPath);

    /// <summary>
    /// The SetIconPath method assigns a display icon to the current session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-seticonpath"></a></remarks>
    [PreserveSig]
    new int SetIconPath([In][MarshalAs(UnmanagedType.LPWStr)] string iconPath, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    /// <summary>
    /// The GetGroupingParam method retrieves the grouping parameter of the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-getgroupingparam"></a></remarks>
    [PreserveSig]
    new int GetGroupingParam([Out] out Guid groupingId);

    /// <summary>
    /// The SetGroupingParam method assigns a session to a grouping of sessions.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-setgroupingparam"></a></remarks>
    [PreserveSig]
    new int SetGroupingParam([In][MarshalAs(UnmanagedType.LPStruct)] Guid groupingId, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    /// <summary>
    /// The RegisterAudioSessionNotification method registers the client to receive notifications of session events,
    /// including changes in the stream state.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-registeraudiosessionnotification"></a></remarks>
    [PreserveSig]
    new int RegisterAudioSessionNotification([In] IAudioSessionEvents client);

    /// <summary>
    /// The UnregisterAudioSessionNotification method deletes a previous registration by the client to receive notifications.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol-unregisteraudiosessionnotification"></a></remarks>
    [PreserveSig]
    new int UnregisterAudioSessionNotification([In] IAudioSessionEvents client);

    /// <summary>
    /// The GetSessionIdentifier method retrieves the audio session identifier.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol2-getsessionidentifier"></a></remarks>
    [PreserveSig]
    int GetSessionIdentifier([Out][MarshalAs(UnmanagedType.LPWStr)] out string sessionId);

    /// <summary>
    /// The GetSessionInstanceIdentifier method retrieves the identifier of the audio session instance.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol2-getsessioninstanceidentifier"></a></remarks>
    [PreserveSig]
    int GetSessionInstanceIdentifier([Out][MarshalAs(UnmanagedType.LPWStr)] out string sessionInstanceId);

    /// <summary>
    /// The GetProcessId method retrieves the process identifier of the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol2-getprocessid"></a></remarks>
    [PreserveSig]
    int GetProcessId([Out] out int processId);

    /// <summary>
    /// The IsSystemSoundsSession method indicates whether the session is a system sounds session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol2-issystemsoundssession"></a></remarks>
    [PreserveSig]
    HRESULT IsSystemSoundsSession();

    /// <summary>
    /// The SetDuckingPreference method enables or disables the default stream attenuation experience (auto-ducking) provided by the system.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessioncontrol2-setduckingpreference"></a></remarks>
    [PreserveSig]
    int SetDuckingPreference(bool autoDucking);
}
