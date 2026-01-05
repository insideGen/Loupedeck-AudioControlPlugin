namespace WindowsInterop.CoreAudio
{
    using System;

    /// <summary>
    /// The Role enumeration defines constants that indicate the role that the system has assigned to an audio endpoint device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/ne-mmdeviceapi-erole"></a></remarks>
    [Flags]
    public enum Role
    {
        /// <summary>
        /// Games, system notification sounds, and voice commands.
        /// </summary>
        Console = 0,
        /// <summary>
        /// Music, movies, narration, and live music recording.
        /// </summary>
        Multimedia = 1,
        /// <summary>
        /// Voice communications (talking to another person).
        /// </summary>
        Communications = 2
    }
}
