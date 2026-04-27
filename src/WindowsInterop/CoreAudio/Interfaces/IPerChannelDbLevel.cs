namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IPerChannelDbLevel interface represents a generic subunit control interface
/// that provides per-channel control over the volume level, in decibels, of an audio stream
/// or of a frequency band in an audio stream.
/// A positive volume level represents gain, and a negative value represents attenuation.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nn-devicetopology-iperchanneldblevel"></a></remarks>
[Guid("7FB7B48F-531D-44A2-BCB3-5AD5A134B3DC")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IPerChannelDbLevel
{
    /// <summary>
    /// The GetChannelCount method gets the number of channels in the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iperchanneldblevel-getchannelcount"></a></remarks>
    int GetChannelCount(out uint channels);

    /// <summary>
    /// The GetLevelRange method gets the range, in decibels, of the volume level of the specified channel.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iperchanneldblevel-getlevelrange"></a></remarks>
    int GetLevelRange(uint channel, out float minLevelDb, out float maxLevelDb, out float stepping);

    /// <summary>
    /// The GetLevel method gets the volume level, in decibels, of the specified channel.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iperchanneldblevel-getlevel"></a></remarks>
    int GetLevel(uint channel, out float levelDb);

    /// <summary>
    /// The SetLevel method sets the volume level, in decibels, of the specified channel.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iperchanneldblevel-setlevel"></a></remarks>
    int SetLevel(uint channel, float levelDb, ref Guid eventGuidContext);

    /// <summary>
    /// The SetLevelUniform method sets all channels in the audio stream to the same uniform volume level, in decibels.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iperchanneldblevel-setleveluniform"></a></remarks>
    int SetLevelUniform(float levelDb, ref Guid guidEventContext);

    /// <summary>
    /// The SetLevelAllChannels method sets the volume levels, in decibels, of all the channels in the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-iperchanneldblevel-setlevelallchannels"></a></remarks>
    int SetLevelAllChannel(float[] levelsDb, uint channels, ref Guid guidEventContext);
}
