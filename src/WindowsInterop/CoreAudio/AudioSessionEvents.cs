namespace WindowsInterop.CoreAudio;

internal class AudioSessionEvents : IAudioSessionEvents
{
    public event EventHandler<(string displayName, Guid eventContext)> DisplayNameChanged;
    public event EventHandler<(string iconPath, Guid eventContext)> IconPathChanged;
    public event EventHandler<(float volume, bool isMuted, Guid eventContext)> SimpleVolumeChanged;
    public event EventHandler<(int channelCount, IntPtr newVolumes, int channelIndex, Guid eventContext)> ChannelVolumeChanged;
    public event EventHandler<(Guid groupingId, Guid eventContext)> GroupingParamChanged;
    public event EventHandler<AudioSessionState> StateChanged;
    public event EventHandler<AudioSessionDisconnectReason> SessionDisconnected;

    public AudioSessionEvents()
    {
    }

    void IAudioSessionEvents.OnDisplayNameChanged(string displayName, ref Guid eventContext)
    {
        this.DisplayNameChanged?.Invoke(null, (displayName, eventContext));
    }

    void IAudioSessionEvents.OnIconPathChanged(string iconPath, ref Guid eventContext)
    {
        this.IconPathChanged?.Invoke(null, (iconPath, eventContext));
    }

    void IAudioSessionEvents.OnSimpleVolumeChanged(float volume, bool isMuted, ref Guid eventContext)
    {
        this.SimpleVolumeChanged?.Invoke(null, (volume, isMuted, eventContext));
    }

    void IAudioSessionEvents.OnChannelVolumeChanged(int channelCount, IntPtr newVolumes, int channelIndex, ref Guid eventContext)
    {
        this.ChannelVolumeChanged?.Invoke(null, (channelCount, newVolumes, channelIndex, eventContext));
    }

    void IAudioSessionEvents.OnGroupingParamChanged(ref Guid groupingId, ref Guid eventContext)
    {
        this.GroupingParamChanged?.Invoke(null, (groupingId, eventContext));
    }

    void IAudioSessionEvents.OnStateChanged(AudioSessionState state)
    {
        this.StateChanged?.Invoke(null, state);
    }

    void IAudioSessionEvents.OnSessionDisconnected(AudioSessionDisconnectReason disconnectReason)
    {
        this.SessionDisconnected?.Invoke(null, disconnectReason);
    }
}
