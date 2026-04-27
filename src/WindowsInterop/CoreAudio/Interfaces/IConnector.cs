namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IConnector interface represents a point of connection between components.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nn-devicetopology-iconnector"></a></remarks>
[Guid("9C2C4058-23F5-41DE-877A-DF3AF236A09E")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IConnector
{
    /// <summary>
    /// The GetType method gets the type of this connector.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iconnector-gettype"></a></remarks>
    int GetType(out ConnectorType type);

    /// <summary>
    /// The GetDataFlow method gets the direction of data flow through this connector.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iconnector-getdataflow"></a></remarks>
    int GetDataFlow(out DataFlow flow);

    /// <summary>
    /// The ConnectTo method connects this connector to a connector in another device-topology object.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iconnector-connectto"></a></remarks>
    int ConnectTo([In] IConnector connectTo);

    /// <summary>
    /// The Disconnect method disconnects this connector from another connector.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iconnector-disconnect"></a></remarks>
    int Disconnect();

    /// <summary>
    /// The IsConnected method indicates whether this connector is connected to another connector.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iconnector-isconnected"></a></remarks>
    int IsConnected(out bool connected);

    /// <summary>
    /// The GetConnectedTo method gets the connector to which this connector is connected.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iconnector-getconnectedto"></a></remarks>
    int GetConnectedTo(out IConnector connectedTo);

    /// <summary>
    /// The GetConnectorIdConnectedTo method gets the global ID of the connector, if any, that this connector is connected to.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iconnector-getconnectoridconnectedto"></a></remarks>
    int GetConnectorIdConnectedTo([MarshalAs(UnmanagedType.LPWStr)] out string id);

    /// <summary>
    /// The GetDeviceIdConnectedTo method gets the device identifier of the audio device, if any, that this connector is connected to.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iconnector-getdeviceidconnectedto"></a></remarks>
    int GetDeviceIdConnectedTo([MarshalAs(UnmanagedType.LPWStr)] out string id);
}
