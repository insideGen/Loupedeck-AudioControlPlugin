namespace WindowsInterop.CoreAudio;

public readonly struct DefaultDeviceEventArgs
{
    public DataFlow Flow { get; }
    public Role Role { get; }
    public string DefaultDeviceId { get; }

    public DefaultDeviceEventArgs(DataFlow flow, Role role, string defaultDeviceId)
    {
        this.Flow = flow;
        this.Role = role;
        this.DefaultDeviceId = defaultDeviceId;
    }
}
