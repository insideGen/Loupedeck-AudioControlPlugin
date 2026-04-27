namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

public class SimpleAudioVolume : IDisposable
{
    private readonly ISimpleAudioVolume simpleAudioVolume;

    internal SimpleAudioVolume(ISimpleAudioVolume realSimpleVolume)
    {
        this.simpleAudioVolume = realSimpleVolume;
    }

    public float Volume
    {
        get
        {
            Marshal.ThrowExceptionForHR(this.simpleAudioVolume.GetMasterVolume(out float level));
            return level;
        }
        set
        {
            float normLevel = value;
            if (normLevel < 0.0f)
            {
                normLevel = 0.0f;
            }
            else if (normLevel > 1.0f)
            {
                normLevel = 1.0f;
            }
            Marshal.ThrowExceptionForHR(this.simpleAudioVolume.SetMasterVolume(normLevel, Guid.Empty));
        }
    }

    public bool Mute
    {
        get
        {
            Marshal.ThrowExceptionForHR(this.simpleAudioVolume.GetMute(out bool isMuted));
            return isMuted;
        }
        set
        {
            Marshal.ThrowExceptionForHR(this.simpleAudioVolume.SetMute(value, Guid.Empty));
        }
    }

    public void Dispose()
    {
        //Marshal.ReleaseComObject(this.simpleAudioVolume);
        GC.SuppressFinalize(this);
    }

    ~SimpleAudioVolume()
    {
        this.Dispose();
    }
}
