namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IControlChangeNotify interface provides notifications when the status of a part (connector or subunit) changes.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nn-devicetopology-icontrolchangenotify"></a></remarks>
[Guid("9C2C4058-23F5-41DE-877A-DF3AF236A09E")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IControlChangeNotify
{
    /// <summary>
    /// The OnNotify method notifies the client when the status of a connector or subunit changes.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-icontrolchangenotify-onnotify"></a></remarks>
    [PreserveSig]
    int OnNotify([In] uint controlId, [In] IntPtr context);
}
