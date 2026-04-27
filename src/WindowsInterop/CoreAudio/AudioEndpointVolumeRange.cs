namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

public class AudioEndpointVolumeRange
{
    private readonly float volumeMinDecibels;
    private readonly float volumeMaxDecibels;
    private readonly float volumeIncrementDecibels;

    internal AudioEndpointVolumeRange(IAudioEndpointVolume parent)
    {
        Marshal.ThrowExceptionForHR(parent.GetVolumeRange(out this.volumeMinDecibels, out this.volumeMaxDecibels, out this.volumeIncrementDecibels));
    }

    public float MinDecibels
    {
        get
        {
            return this.volumeMinDecibels;
        }
    }

    public float MaxDecibels
    {
        get
        {
            return this.volumeMaxDecibels;
        }
    }

    public float IncrementDecibels
    {
        get
        {
            return this.volumeIncrementDecibels;
        }
    }
}
