namespace WindowsInterop.CoreAudio;

public class AudioMute
{
    private readonly IAudioMute audioMuteInterface;

    internal AudioMute(IAudioMute audioMute)
    {
        this.audioMuteInterface = audioMute;
    }

    public bool IsMuted
    {
        get
        {
            this.audioMuteInterface.GetMute(out bool isMuted);
            return isMuted;
        }
        set
        {
            this.audioMuteInterface.SetMute(value, Guid.Empty);
        }
    }
}
