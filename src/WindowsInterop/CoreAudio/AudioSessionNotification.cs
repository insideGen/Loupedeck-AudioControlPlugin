namespace WindowsInterop.CoreAudio;

internal class AudioSessionNotification : IAudioSessionNotification
{
    public event EventHandler<AudioSessionControl> SessionCreated;

    public AudioSessionNotification()
    {
    }

    void IAudioSessionNotification.OnSessionCreated(IAudioSessionControl newSession)
    {
        this.SessionCreated?.Invoke(null, new AudioSessionControl(newSession));
    }
}
