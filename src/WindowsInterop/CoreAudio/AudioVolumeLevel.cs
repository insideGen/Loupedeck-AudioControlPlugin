namespace WindowsInterop.CoreAudio;

public class AudioVolumeLevel
{
    private readonly IAudioVolumeLevel audioVolumeLevelInterface;

    internal AudioVolumeLevel(IAudioVolumeLevel audioVolumeLevel)
    {
        this.audioVolumeLevelInterface = audioVolumeLevel;
    }

    public uint ChannelCount
    {
        get
        {
            this.audioVolumeLevelInterface.GetChannelCount(out uint result);
            return result;
        }
    }

    public void GetLevelRange(uint channel, out float minLevelDb, out float maxLevelDb, out float stepping)
    {
        this.audioVolumeLevelInterface.GetLevelRange(channel, out minLevelDb, out maxLevelDb, out stepping);
    }

    public float GetLevel(uint channel)
    {
        this.audioVolumeLevelInterface.GetLevel(channel, out float result);
        return result;
    }

    public void SetLevel(uint channel, float value)
    {
        Guid guid = Guid.Empty;
        this.audioVolumeLevelInterface.SetLevel(channel, value, ref guid);
    }

    public void SetLevelUniform(float value)
    {
        Guid guid = Guid.Empty;
        this.audioVolumeLevelInterface.SetLevelUniform(value, ref guid);
    }

    public void SetLevelAllChannel(float[] values, uint channels)
    {
        Guid guid = Guid.Empty;
        this.audioVolumeLevelInterface.SetLevelAllChannel(values, channels, ref guid);
    }
}
