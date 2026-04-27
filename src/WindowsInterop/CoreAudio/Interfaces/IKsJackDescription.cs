namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IKsJackDescription interface provides information about the jacks or internal connectors
/// that provide a physical connection between a device on an audio adapter and an external
/// or internal endpoint device (for example, a microphone or CD player).
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nn-devicetopology-iksjackdescription"></a></remarks>
[Guid("4509F757-2D46-4637-8E62-CE7DB944F57B")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IKsJackDescription
{
    /// <summary>
    /// The GetJackCount method gets the number of jacks required to connect to an audio endpoint device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iksjackdescription-getjackcount"></a></remarks>
    int GetJackCount([Out] out uint count);

    /// <summary>
    /// The GetJackDescription method gets a description of an audio jack.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iksjackdescription-getjackdescription"></a></remarks>
    int GetJackDescription([In] uint jack, [Out, MarshalAs(UnmanagedType.LPWStr)] out string description);
}
