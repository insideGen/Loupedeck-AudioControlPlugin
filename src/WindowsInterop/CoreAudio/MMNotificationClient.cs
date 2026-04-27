namespace WindowsInterop.CoreAudio;

using WindowsInterop.PropertySystem;

internal class MMNotificationClient : IMMNotificationClient
{
    public event EventHandler<DeviceStateEventArgs> DeviceStateChanged;
    public event EventHandler<DeviceIdEventArgs> DeviceAdded;
    public event EventHandler<DeviceIdEventArgs> DeviceRemoved;
    public event EventHandler<DefaultDeviceEventArgs> DefaultDeviceChanged;
    public event EventHandler<PropertyValueEventArgs> PropertyValueChanged;

    public MMNotificationClient()
    {
    }

    void IMMNotificationClient.OnDeviceStateChanged(string deviceId, DeviceState newState)
    {
        this.DeviceStateChanged?.Invoke(null, new DeviceStateEventArgs(deviceId, newState));
    }

    void IMMNotificationClient.OnDeviceAdded(string deviceId)
    {
        this.DeviceAdded?.Invoke(null, new DeviceIdEventArgs(deviceId));
    }

    void IMMNotificationClient.OnDeviceRemoved(string deviceId)
    {
        this.DeviceRemoved?.Invoke(null, new DeviceIdEventArgs(deviceId));
    }

    void IMMNotificationClient.OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
    {
        this.DefaultDeviceChanged?.Invoke(null, new DefaultDeviceEventArgs(flow, role, defaultDeviceId));
    }

    void IMMNotificationClient.OnPropertyValueChanged(string deviceId, PropertyKey key)
    {
        this.PropertyValueChanged?.Invoke(null, new PropertyValueEventArgs(deviceId, key));
    }
}
