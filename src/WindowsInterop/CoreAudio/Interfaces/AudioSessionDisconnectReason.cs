namespace WindowsInterop.CoreAudio;

/// <summary>
/// Defines constants that indicate a reason for an audio session being disconnected.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionevents-onsessiondisconnected"></a></remarks>
public enum AudioSessionDisconnectReason
{
    /// <summary>
    /// The user removed the audio endpoint device.
    /// </summary>
    DeviceRemoval = 0,
    /// <summary>
    /// The Windows audio service has stopped.
    /// </summary>
    ServerShutdown = 1,
    /// <summary>
    /// The stream format changed for the device that the audio session is connected to.
    /// </summary>
    FormatChanged = 2,
    /// <summary>
    /// The user logged off the Windows Terminal Services (WTS) session that the audio session was running in.
    /// </summary>
    SessionLogoff = 3,
    /// <summary>
    /// The WTS session that the audio session was running in was disconnected.
    /// </summary>
    SessionDisconnected = 4,
    /// <summary>
    /// The (shared-mode) audio session was disconnected to make the audio endpoint device available for an exclusive-mode connection.
    /// </summary>
    ExclusiveModeOverride = 5
}
