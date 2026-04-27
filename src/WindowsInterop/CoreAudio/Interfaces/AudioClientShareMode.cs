namespace WindowsInterop.CoreAudio;

/// <summary>
/// The AudioClientShareMode enumeration defines constants that indicate whether an audio stream will run
/// in shared mode or in exclusive mode.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiosessiontypes/ne-audiosessiontypes-audclnt_sharemode"></a></remarks>
public enum AudioClientShareMode
{
    /// <summary>
    /// The audio stream will run in shared mode.
    /// </summary>
    Shared,
    /// <summary>
    /// The audio stream will run in exclusive mode.
    /// </summary>
    Exclusive
}
