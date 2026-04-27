namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The WAVEFORMATEX structure defines the format of waveform-audio data.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmeapi/ns-mmeapi-waveformatex"></a></remarks>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
public class WaveFormat
{
    protected WaveFormatTags formatTag;
    protected short channels;
    protected int samplesPerSecond;
    protected int avgBytesPerSecond;
    protected short blockAlign;
    protected short bitsPerSample;
    protected short size;

    public WaveFormatTags FormatTag => this.formatTag;
    public short Channels => this.channels;
    public int SamplesPerSecond => this.samplesPerSecond;
    public int AvgBytesPerSecond => this.avgBytesPerSecond;
    public short BlockAlign => this.blockAlign;
    public short BitsPerSample => this.bitsPerSample;
    public short Size => this.size;

    /// <summary>
    /// Creates a new PCM 44.1Khz stereo 16 bit format.
    /// </summary>
    public WaveFormat() : this(44100, 16, 2)
    {
    }

    /// <summary>
    /// Creates a new 16 bit wave format with the specified sample rate and channel count.
    /// </summary>
    /// <param name="sampleRate">Sample Rate</param>
    /// <param name="channels">Number of channels</param>
    public WaveFormat(int sampleRate, int channels) : this(sampleRate, 16, channels)
    {
    }

    /// <summary>
    /// Creates a new PCM format with the specified sample rate, bit depth and channels.
    /// </summary>
    public WaveFormat(int rate, int bits, int channels)
    {
        if (channels < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(channels), "Channels must be 1 or greater");
        }
        // Minimum 16 bytes, sometimes 18 for PCM.
        this.formatTag = WaveFormatTags.Pcm;
        this.channels = (short)channels;
        this.samplesPerSecond = rate;
        this.bitsPerSample = (short)bits;
        this.size = 0;
        this.blockAlign = (short)(channels * (bits / 8));
        this.avgBytesPerSecond = this.samplesPerSecond * this.blockAlign;
    }

    /// <summary>
    /// Helper function to retrieve a WaveFormat structure from a pointer.
    /// </summary>
    /// <param name="pointer">WaveFormat structure</param>
    /// <returns></returns>
    public static WaveFormat MarshalFromPtr(IntPtr pointer)
    {
        WaveFormat waveFormat = Marshal.PtrToStructure<WaveFormat>(pointer);
        switch (waveFormat.formatTag)
        {
            case WaveFormatTags.Extensible:
                waveFormat = Marshal.PtrToStructure<WaveFormatExtensible>(pointer);
                break;
        }
        return waveFormat;
    }
}
