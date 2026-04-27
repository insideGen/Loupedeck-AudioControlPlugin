namespace WindowsInterop.CoreAudio;

public interface IAudioControlSession : IAudioControl
{
    string InstanceId { get; }
    string DeviceId { get; }
    string ExePath { get; }
    string ExeId { get; }
    int ProcessId { get; }
    AudioSessionState State { get; }
    bool IsSystemSoundsSession { get; }
}
