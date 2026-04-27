namespace WindowsInterop.CoreAudio;

/// <summary>
/// The AudioVolumeNotificationData structure describes a change in the volume level or muting state of an audio endpoint device.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/ns-endpointvolume-audio_volume_notification_data"></a></remarks>
public struct AudioVolumeNotificationData
{
    public Guid EventContext;
    public bool Muted;
    public float MasterVolume;
    public uint Channels;
    public float ChannelVolume;

    public AudioVolumeNotificationData(Guid eventContext, bool muted, float masterVolume, uint channels, float channelVolume)
    {
        this.EventContext = eventContext;
        this.Muted = muted;
        this.MasterVolume = masterVolume;
        this.Channels = channels;
        this.ChannelVolume = channelVolume;
    }
}
