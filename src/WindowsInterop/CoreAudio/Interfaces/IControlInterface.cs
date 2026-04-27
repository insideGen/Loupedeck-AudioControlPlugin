namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IControlInterface interface represents a control interface on a part (connector or subunit) in a device topology.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nn-devicetopology-icontrolinterface"></a></remarks>
[Guid("45D37C3F-5140-444A-AE24-400789F3CBF3")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IControlInterface
{
}
