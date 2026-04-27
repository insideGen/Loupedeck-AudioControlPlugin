namespace WindowsInterop.CoreAudio;

/// <summary>
/// Specifies characteristics that a client can assign to an audio stream during the initialization of the stream.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/coreaudio/audclnt-streamflags-xxx-constants"></a></remarks>
[Flags]
public enum AudioClientStreamFlags : uint
{
    /// <summary>
    /// None
    /// </summary>
    None = 0,
    /// <summary>
    /// The audio stream will be a member of a cross-process audio session.
    /// </summary>
    CrossProcess = 0x00010000,
    /// <summary>
    /// The audio stream will operate in loopback mode.
    /// </summary>
    Loopback = 0x00020000,
    /// <summary>
    /// Processing of the audio buffer by the client will be event driven.
    /// </summary>
    EventCallback = 0x00040000,
    /// <summary>
    /// The volume and mute settings for an audio session will not persist across application restarts.
    /// </summary>
    NoPersist = 0x00080000,
    /// <summary>
    /// The sample rate of the stream is adjusted to a rate specified by an application.
    /// </summary>
    RateAdjust = 0x00100000,
    /// <summary>
    /// A channel matrixer and a sample rate converter are inserted as necessary to convert between
    /// the uncompressed format supplied to IAudioClient::Initialize and the audio engine mix format.
    /// </summary>
    AutoConvertPcm = 0x80000000,
    /// <summary>
    /// When used with AutoConvertPcm, a sample rate converter with better quality than the default conversion
    /// but with a higher performance cost is used. This should be used if the audio is ultimately intended to be heard
    /// by humans as opposed to other scenarios such as pumping silence or populating a meter.
    /// </summary>
    SrcDefaultQuality = 0x08000000
}
