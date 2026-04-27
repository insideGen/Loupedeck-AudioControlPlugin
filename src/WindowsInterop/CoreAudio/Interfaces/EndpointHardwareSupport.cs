namespace WindowsInterop.CoreAudio;

/// <summary>
/// Endpoint Hardware Support
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/coreaudio/endpoint-hardware-support-xxx-constants"></a></remarks>
[Flags]
public enum EndpointHardwareSupport
{
    /// <summary>
    /// The audio endpoint device supports a hardware volume control.
    /// </summary>
    Volume = 0x00000001,
    /// <summary>
    /// The audio endpoint device supports a hardware mute control.
    /// </summary>
    Mute = 0x00000002,
    /// <summary>
    /// The audio endpoint device supports a hardware peak meter.
    /// </summary>
    Meter = 0x00000004
}
