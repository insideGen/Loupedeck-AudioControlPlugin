namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The WAVEFORMATEXTENSIBLE structure defines the format of waveform-audio data for formats
/// having more than two channels or higher sample resolutions than allowed by WAVEFORMATEX.
/// It can also be used to define any format that can be defined by WAVEFORMATEX.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmreg/ns-mmreg-waveformatextensible"></a></remarks>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
public class WaveFormatExtensible : WaveFormat
{
    protected short validBitsPerSampleOrSamplesPerBlock;
    protected int channelMask;
    protected Guid subFormat;

    public short ValidBitsPerSampleOrSamplesPerBlock => this.validBitsPerSampleOrSamplesPerBlock;
    public int ChannelMask => this.channelMask;
    public Guid SubFormat => this.subFormat;
}
