namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IDeviceTopology interface provides access to the topology of an audio device.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nn-devicetopology-idevicetopology"></a></remarks>
[Guid("2A07407E-6497-4A18-9787-32F79BD0D98F")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IDeviceTopology
{
    /// <summary>
    /// The GetConnectorCount method gets the number of connectors in the device-topology object.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-idevicetopology-getconnectorcount"></a></remarks>
    int GetConnectorCount(out int count);

    /// <summary>
    /// The GetConnector method gets the connector that is specified by a connector number.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-idevicetopology-getconnector"></a></remarks>
    int GetConnector(int index, out IConnector connector);

    /// <summary>
    /// The GetSubunitCount method gets the number of subunits in the device topology.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-idevicetopology-getsubunitcount"></a></remarks>
    int GetSubunitCount(out int count);

    /// <summary>
    /// The GetSubunit method gets the subunit that is specified by a subunit number.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-idevicetopology-getsubunit"></a></remarks>
    int GetSubunit(int index, out ISubunit subunit);

    /// <summary>
    /// The GetPartById method gets a part that is identified by its local ID.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-idevicetopology-getpartbyid"></a></remarks>
    int GetPartById(int id, out IPart part);

    /// <summary>
    /// The GetDeviceId method gets the device identifier of the device that is represented by the device-topology object.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-idevicetopology-getdeviceid"></a></remarks>
    int GetDeviceId([MarshalAs(UnmanagedType.LPWStr)] out string id);

    /// <summary>
    /// The GetSignalPath method gets a list of parts in the signal path that links two parts, if the path exists.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-idevicetopology-getsignalpath"></a></remarks>
    int GetSignalPath(IPart from, IPart to, bool rejectMixedPaths, out IPartsList parts);
}
