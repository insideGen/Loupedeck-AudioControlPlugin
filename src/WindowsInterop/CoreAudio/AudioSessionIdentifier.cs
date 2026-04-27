namespace WindowsInterop.CoreAudio;

using WindowsInterop.Win32;

public class AudioSessionIdentifier
{
    public string DeviceId { get; } = null;
    public string ExePath { get; } = null;
    public string ExeId { get; } = null;

    public AudioSessionIdentifier(string deviceId, string exePath, string exeId)
    {
        this.DeviceId = deviceId;
        this.ExePath = exePath;
        this.ExeId = exeId;
    }

    public static AudioSessionIdentifier FromString(string value)
    {
        try
        {
            string[] level1 = value.Split('|');
            string[] level2 = level1[1].Split(new string[] { "%b" }, StringSplitOptions.None);
            string deviceId = level1[0];
            string exePath = level2[0] == "#" ? null : DevicePathMapper.FromDevicePath(level2[0]);
            string exeId = level2[1];
            return new AudioSessionIdentifier(deviceId, exePath, exeId);
        }
        catch
        {
            return null;
        }
    }

    public override string ToString()
    {
        string exePath = this.ExePath is null ? "#" : DevicePathMapper.FromDriveLetter(this.ExePath);
        return $"{this.DeviceId}|{exePath}%b{this.ExeId}";
    }
}

public class AudioSessionInstanceIdentifier : AudioSessionIdentifier
{
    public int Reserved { get; } = -1;
    public int ProcessId { get; } = -1;

    public AudioSessionInstanceIdentifier(string deviceId, string exePath, string exeId, int reserved, int processId) : base(deviceId, exePath, exeId)
    {
        this.Reserved = reserved;
        this.ProcessId = processId;
    }

    public static new AudioSessionInstanceIdentifier FromString(string value)
    {
        try
        {
            int reserved = 1;
            int processId = -1;
            string[] level1 = value.Split('|');
            if (level1.Length > 2)
            {
                string[] level2 = level1[2].Split(new string[] { "%b" }, StringSplitOptions.None);
                int.TryParse(level2[0], out reserved);
                int.TryParse(level2[1], out processId);
            }
            AudioSessionIdentifier asi = AudioSessionIdentifier.FromString($"{level1[0]}|{level1[1]}");
            return new AudioSessionInstanceIdentifier(asi.DeviceId, asi.ExePath, asi.ExeId, reserved, processId);

        }
        catch
        {
            return null;
        }
    }

    public override string ToString()
    {
        string processId = this.ProcessId <= 0 ? "#" : this.ProcessId.ToString();
        return $"{base.ToString()}|{this.Reserved}%b{processId}";
    }
}
