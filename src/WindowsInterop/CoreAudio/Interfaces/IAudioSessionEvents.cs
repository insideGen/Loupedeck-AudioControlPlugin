namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioSessionEvents interface provides notifications of session-related events
/// such as changes in the volume level, display name, and session state.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nn-audiopolicy-iaudiosessionevents"></a></remarks>
[Guid("24918ACC-64B3-37C1-8CA9-74A66E9957A8")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IAudioSessionEvents
{
    /// <summary>
    /// Notifies the client that the display name for the session has changed.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionevents-ondisplaynamechanged"></a></remarks>
    /// <param name="displayName">The new display name for the session.</param>
    /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
    void OnDisplayNameChanged([In][MarshalAs(UnmanagedType.LPWStr)] string displayName, [In] ref Guid eventContext);

    /// <summary>
    /// Notifies the client that the display icon for the session has changed.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionevents-oniconpathchanged"></a></remarks>
    /// <param name="iconPath">The path for the new display icon for the session.</param>
    /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
    void OnIconPathChanged([In][MarshalAs(UnmanagedType.LPWStr)] string iconPath, [In] ref Guid eventContext);

    /// <summary>
    /// Notifies the client that the volume level or muting state of the session has changed.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionevents-onsimplevolumechanged"></a></remarks>
    /// <param name="volume">The new volume level for the audio session.</param>
    /// <param name="isMuted">The new muting state.</param>
    /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
    void OnSimpleVolumeChanged([In][MarshalAs(UnmanagedType.R4)] float volume, [In][MarshalAs(UnmanagedType.Bool)] bool isMuted, [In] ref Guid eventContext);

    /// <summary>
    /// Notifies the client that the volume level of an audio channel in the session submix has changed.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionevents-onchannelvolumechanged"></a></remarks>
    /// <param name="channelCount">The channel count.</param>
    /// <param name="newVolumes">An array of volumnes cooresponding with each channel index.</param>
    /// <param name="channelIndex">The number of the channel whose volume level changed.</param>
    /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
    void OnChannelVolumeChanged([In][MarshalAs(UnmanagedType.U4)] int channelCount, [In][MarshalAs(UnmanagedType.SysInt)] IntPtr newVolumes, [In][MarshalAs(UnmanagedType.U4)] int channelIndex, [In] ref Guid eventContext);

    /// <summary>
    /// Notifies the client that the grouping parameter for the session has changed.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionevents-ongroupingparamchanged"></a></remarks>
    /// <param name="groupingId">The new grouping parameter for the session.</param>
    /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
    void OnGroupingParamChanged([In] ref Guid groupingId, [In] ref Guid eventContext);

    /// <summary>
    /// Notifies the client that the stream-activity state of the session has changed.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionevents-onstatechanged"></a></remarks>
    /// <param name="state">The new session state.</param>
    void OnStateChanged([In] AudioSessionState state);

    /// <summary>
    /// Notifies the client that the session has been disconnected.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionevents-onsessiondisconnected"></a></remarks>
    /// <param name="disconnectReason">The reason that the audio session was disconnected.</param>
    void OnSessionDisconnected([In] AudioSessionDisconnectReason disconnectReason);
}
