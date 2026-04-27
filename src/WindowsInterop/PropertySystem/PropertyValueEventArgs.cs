namespace WindowsInterop.PropertySystem;

public readonly struct PropertyValueEventArgs
{
    public string DeviceId { get; }
    public PropertyKey Key { get; }

    public PropertyValueEventArgs(string deviceId, PropertyKey key)
    {
        this.DeviceId = deviceId;
        this.Key = key;
    }
}
