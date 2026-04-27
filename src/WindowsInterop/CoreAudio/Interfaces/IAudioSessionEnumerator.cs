namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioSessionEnumerator interface enumerates audio sessions on an audio device.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nn-audiopolicy-iaudiosessionenumerator"></a></remarks>
[Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IAudioSessionEnumerator
{
    /// <summary>
    /// The GetCount method gets the total number of audio sessions that are open on the audio device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionenumerator-getcount"></a></remarks>
    int GetCount(out int sessionCount);

    /// <summary>
    /// The GetSession method gets the audio session specified by an audio session number.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionenumerator-getsession"></a></remarks>
    int GetSession(int sessionCount, out IAudioSessionControl session);
}
