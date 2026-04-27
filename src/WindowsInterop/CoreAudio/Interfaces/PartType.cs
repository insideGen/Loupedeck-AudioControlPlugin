namespace WindowsInterop.CoreAudio;

/// <summary>
/// The PartType enumeration defines constants that indicate whether a part in a device topology is a connector or subunit.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/ne-devicetopology-parttype"></a></remarks>
public enum PartTypeEnum
{
    Connector = 0,
    Subunit
}
