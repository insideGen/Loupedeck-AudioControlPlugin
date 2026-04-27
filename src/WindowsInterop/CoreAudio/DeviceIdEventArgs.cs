namespace WindowsInterop.CoreAudio;

public readonly struct DeviceIdEventArgs
{
    public string DeviceId { get; }

    public DeviceIdEventArgs(string deviceId)
    {
        this.DeviceId = deviceId;
    }
}
