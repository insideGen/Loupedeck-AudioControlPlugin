namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IMMDeviceCollection interface represents a collection of multimedia device resources.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nn-mmdeviceapi-immdevicecollection"></a></remarks>
[Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IMMDeviceCollection
{
    /// <summary>
    /// The GetCount method retrieves a count of the devices in the device collection.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immdevicecollection-getcount"></a></remarks>
    int GetCount(out int count);

    /// <summary>
    /// The Item method retrieves a pointer to the specified item in the device collection.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immdevicecollection-item"></a></remarks>
    int Item(int deviceNumber, out IMMDevice device);
}
