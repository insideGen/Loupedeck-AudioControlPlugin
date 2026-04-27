namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The ISubunit interface represents a hardware subunit (for example, a volume control) that lies in the data path
/// between a client and an audio endpoint device.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nn-devicetopology-isubunit"></a></remarks>
[Guid("82149A85-DBA6-4487-86BB-EA8F7FEFCC71")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface ISubunit
{
}
