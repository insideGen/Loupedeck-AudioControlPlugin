namespace WindowsInterop.CoreAudio;

public struct AudioVolumeNotificationEventArgs
{
    public Guid EventContext;
    public bool Muted;
    public float MasterVolume;
    public uint Channels;
    public float[] ChannelVolumes;

    public AudioVolumeNotificationEventArgs(Guid eventContext, bool muted, float masterVolume, uint channels, float[] channelVolumes)
    {
        this.EventContext = eventContext;
        this.Muted = muted;
        this.MasterVolume = masterVolume;
        this.Channels = channels;
        this.ChannelVolumes = channelVolumes;
    }
}
