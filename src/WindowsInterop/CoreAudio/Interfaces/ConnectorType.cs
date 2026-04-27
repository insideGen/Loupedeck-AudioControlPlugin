namespace WindowsInterop.CoreAudio;

/// <summary>
/// The ConnectorType enumeration indicates the type of connection that a connector is part of.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/ne-devicetopology-connectortype"></a></remarks>
public enum ConnectorType
{
    /// <summary>
    /// The connector is part of a connection of unknown type.
    /// </summary>
    UnknownConnector,
    /// <summary>
    /// The connector is part of a physical connection to an auxiliary device that is installed inside the system chassis
    /// (for example, a connection to the analog output of an internal CD player,
    /// or to a built-in microphone or built-in speakers in a laptop computer).
    /// </summary>
    PhysicalInternal,
    /// <summary>
    /// The connector is part of a physical connection to an external device.
    /// That is, the connector is a user-accessible jack that connects to a microphone, speakers,
    /// headphones, S/PDIF input or output device, or line input or output device.
    /// </summary>
    PhysicalExternal,
    /// <summary>
    /// The connector is part of a software-configured I/O connection (typically a DMA channel) between system memory
    /// and an audio hardware device on an audio adapter.
    /// </summary>
    SoftwareIo,
    /// <summary>
    /// The connector is part of a permanent connection that is fixed and cannot be configured under software control.
    /// This type of connection is typically used to connect two audio hardware devices that reside on the same adapter.
    /// </summary>
    SoftwareFixed,
    /// <summary>
    /// The connector is part of a connection to a network.
    /// </summary>
    Network
}
