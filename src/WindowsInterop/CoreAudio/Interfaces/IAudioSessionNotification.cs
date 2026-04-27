namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioSessionNotification interface provides notification when an audio session is created.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nn-audiopolicy-iaudiosessionnotification"></a></remarks>
[Guid("641DD20B-4D41-49CC-ABA3-174B9477BB08")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IAudioSessionNotification
{
    /// <summary>
    /// The OnSessionCreated method notifies the registered processes that the audio session has been created.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionnotification-onsessioncreated"></a></remarks>
    void OnSessionCreated(IAudioSessionControl newSession);
}
