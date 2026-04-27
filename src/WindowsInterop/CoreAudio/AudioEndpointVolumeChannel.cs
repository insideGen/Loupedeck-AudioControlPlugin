namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

public class AudioEndpointVolumeChannel
{
    private readonly uint channel;
    private readonly IAudioEndpointVolume audioEndpointVolume;

    private Guid notificationGuid = Guid.Empty;

    /// <summary>
    /// GUID to pass to AudioEndpointVolumeCallback
    /// </summary>
    public Guid NotificationGuid
    {
        get => this.notificationGuid;
        set => this.notificationGuid = value;
    }

    internal AudioEndpointVolumeChannel(IAudioEndpointVolume parent, int channel)
    {
        this.channel = (uint)channel;
        this.audioEndpointVolume = parent;
    }

    public float VolumeLevel
    {
        get
        {
            Marshal.ThrowExceptionForHR(this.audioEndpointVolume.GetChannelVolumeLevel(this.channel, out float leveldB));
            return leveldB;
        }
        set
        {
            Marshal.ThrowExceptionForHR(this.audioEndpointVolume.SetChannelVolumeLevel(this.channel, value, ref this.notificationGuid));
        }
    }

    public float VolumeLevelScalar
    {
        get
        {
            Marshal.ThrowExceptionForHR(this.audioEndpointVolume.GetChannelVolumeLevelScalar(this.channel, out float level));
            return level;
        }
        set
        {
            Marshal.ThrowExceptionForHR(this.audioEndpointVolume.SetChannelVolumeLevelScalar(this.channel, value, ref this.notificationGuid));
        }
    }
}
