namespace WindowsInterop.CoreAudio;

/// <summary>
/// The AudioClientBufferFlags enumeration defines flags that indicate the status of an audio endpoint buffer.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/ne-audioclient-_audclnt_bufferflags"></a></remarks>
[Flags]
public enum AudioClientBufferFlags
{
    /// <summary>
    /// None
    /// </summary>
    None,
    /// <summary>
    /// The data in the packet is not correlated with the previous packet's device position;
    /// this is possibly due to a stream state transition or timing glitch.
    /// </summary>
    DataDiscontinuity = 0x1,
    /// <summary>
    /// Treat all of the data in the packet as silence and ignore the actual data values.
    /// </summary>
    Silent = 0x2,
    /// <summary>
    /// The time at which the device's stream position was recorded is uncertain.
    /// Thus, the client might be unable to accurately set the time stamp for the current data packet.
    /// </summary>
    TimestampError = 0x4
}
