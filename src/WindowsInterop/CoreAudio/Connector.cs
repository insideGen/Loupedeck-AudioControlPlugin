namespace WindowsInterop.CoreAudio;

public class Connector
{
    private readonly IConnector connectorInterface;

    internal Connector(IConnector connector)
    {
        this.connectorInterface = connector;
    }

    /// <summary>
    /// Connects this connector to a connector in another device-topology object
    /// </summary>
    public void ConnectTo(Connector other)
    {
        this.connectorInterface.ConnectTo(other.connectorInterface);
    }

    /// <summary>
    /// Retreives the type of this connector
    /// </summary>
    public ConnectorType Type
    {
        get
        {
            this.connectorInterface.GetType(out ConnectorType connectorType);
            return connectorType;
        }
    }

    /// <summary>
    /// Retreives the data flow of this connector
    /// </summary>
    public DataFlow DataFlow
    {
        get
        {
            this.connectorInterface.GetDataFlow(out DataFlow dataFlow);
            return dataFlow;
        }
    }

    /// <summary>
    /// Disconnects this connector from it's connected connector (if connected)
    /// </summary>
    public void Disconnect()
    {
        this.connectorInterface.Disconnect();
    }

    /// <summary>
    /// Indicates whether this connector is connected to another connector
    /// </summary>
    public bool IsConnected
    {
        get
        {
            this.connectorInterface.IsConnected(out bool connected);
            return connected;
        }
    }

    /// <summary>
    /// Retreives the connector this connector is connected to (if connected)
    /// </summary>
    public Connector ConnectedTo
    {
        get
        {
            this.connectorInterface.GetConnectedTo(out IConnector connectedTo);
            return new Connector(connectedTo);
        }
    }

    /// <summary>
    /// Retreives the global ID of the connector this connector is connected to (if connected)
    /// </summary>
    public string ConnectedToConnectorId
    {
        get
        {
            this.connectorInterface.GetConnectorIdConnectedTo(out string id);
            return id;
        }
    }

    /// <summary>
    /// Retreives the device ID of the audio device this connector is connected to (if connected)
    /// </summary>
    public string ConnectedToDeviceId
    {
        get
        {
            this.connectorInterface.GetDeviceIdConnectedTo(out string id);
            return id;
        }
    }

    public Part Part
    {
        get
        {
            return new Part(this.connectorInterface as IPart);
        }
    }
}
