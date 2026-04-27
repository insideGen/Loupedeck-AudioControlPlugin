namespace WindowsInterop.CoreAudio;

/// <summary>
/// The DataFlow enumeration defines constants that indicate the direction
/// in which audio data flows between an audio endpoint device and an application.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/ne-mmdeviceapi-edataflow"></a></remarks>
public enum DataFlow
{
    /// <summary>
    /// Audio rendering stream.<br />
    /// Audio data flows from the application to the audio endpoint device, which renders the stream.
    /// </summary>
    Render,
    /// <summary>
    /// Audio capture stream.<br />
    /// Audio data flows from the audio endpoint device that captures the stream, to the application.
    /// </summary>
    Capture,
    /// <summary>
    /// Audio rendering or capture stream.<br />
    /// Audio data can flow either from the application to the audio endpoint device,
    /// or from the audio endpoint device to the application.
    /// </summary>
    All
}
