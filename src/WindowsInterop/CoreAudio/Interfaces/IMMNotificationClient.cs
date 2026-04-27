namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

using WindowsInterop.PropertySystem;

/// <summary>
/// The IMMNotificationClient interface provides notifications when an audio endpoint
/// device is added or removed, when the state or properties of an endpoint device change,
/// or when there is a change in the default role assigned to an endpoint device.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nn-mmdeviceapi-immnotificationclient"></a></remarks>
[Guid("7991EEC9-7E89-4D85-8390-6C703CEC60C0")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IMMNotificationClient
{
    /// <summary>
    /// The OnDeviceStateChanged method indicates that the state of an audio endpoint device has changed.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immnotificationclient-ondevicestatechanged"></a></remarks>
    void OnDeviceStateChanged([MarshalAs(UnmanagedType.LPWStr)] string deviceId, [MarshalAs(UnmanagedType.I4)] DeviceState newState);

    /// <summary>
    /// The OnDeviceAdded method indicates that a new audio endpoint device has been added.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immnotificationclient-ondeviceadded"></a></remarks>
    void OnDeviceAdded([MarshalAs(UnmanagedType.LPWStr)] string deviceId);

    /// <summary>
    /// The OnDeviceRemoved method indicates that an audio endpoint device has been removed.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immnotificationclient-ondeviceremoved"></a></remarks>
    void OnDeviceRemoved([MarshalAs(UnmanagedType.LPWStr)] string deviceId);

    /// <summary>
    /// The OnDefaultDeviceChanged method notifies the client that the default audio endpoint device
    /// for a particular device role has changed.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immnotificationclient-ondefaultdevicechanged"></a></remarks>
    void OnDefaultDeviceChanged(DataFlow flow, Role role, [MarshalAs(UnmanagedType.LPWStr)] string defaultDeviceId);

    /// <summary>
    /// The OnPropertyValueChanged method indicates that the value of a property belonging
    /// to an audio endpoint device has changed.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immnotificationclient-onpropertyvaluechanged"></a></remarks>
    void OnPropertyValueChanged([MarshalAs(UnmanagedType.LPWStr)] string deviceId, PropertyKey key);
}
