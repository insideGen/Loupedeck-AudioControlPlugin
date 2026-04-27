namespace WindowsInterop.CoreAudio;

/// <summary>
/// Windows CoreAudio DeviceTopology
/// </summary>
public class DeviceTopology : IDisposable
{
    private readonly IDeviceTopology realDeviceTopologyInterface;

    /// <summary>
    /// Retrieves the number of connections associated with this device-topology object
    /// </summary>
    public int ConnectorCount
    {
        get
        {
            this.realDeviceTopologyInterface.GetConnectorCount(out int count);
            return count;
        }
    }

    /// <summary>
    /// Retrieves the device id of the device represented by this device-topology object
    /// </summary>
    public string DeviceId
    {
        get
        {
            this.realDeviceTopologyInterface.GetDeviceId(out string id);
            return id;
        }
    }

    internal DeviceTopology(IDeviceTopology deviceTopology)
    {
        this.realDeviceTopologyInterface = deviceTopology;
    }

    /// <summary>
    /// Retrieves the connector at the supplied index
    /// </summary>
    public Connector GetConnector(int index)
    {
        this.realDeviceTopologyInterface.GetConnector(index, out IConnector connectorInterface);
        return new Connector(connectorInterface);
    }

    public void Dispose()
    {
        //Marshal.ReleaseComObject(this.realDeviceTopologyInterface);
        GC.SuppressFinalize(this);
    }

    ~DeviceTopology()
    {
        this.Dispose();
    }
}
