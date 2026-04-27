namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IChannelAudioVolume interface enables a client to control and monitor the volume levels for all of the channels in the audio session that the stream belongs to.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nn-audioclient-ichannelaudiovolume"></a></remarks>
[Guid("1C158861-B533-4B30-B1CF-E853E51C59B8")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IChannelAudioVolume
{
    /// <summary>
    /// The GetChannelCount method retrieves the number of channels in the stream format for the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-ichannelaudiovolume-getchannelcount"></a></remarks>
    [PreserveSig]
    int GetChannelCount();

    /// <summary>
    /// The SetChannelVolume method sets the volume level for the specified channel in the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-ichannelaudiovolume-setchannelvolume"></a></remarks>
    [PreserveSig]
    int SetChannelVolume(uint index, float level, ref Guid eventContent);

    /// <summary>
    /// The GetChannelVolume method retrieves the volume level for the specified channel in the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-ichannelaudiovolume-getchannelvolume"></a></remarks>
    [PreserveSig]
    int GetChannelVolume(uint index, out float level);

    /// <summary>
    /// The SetAllVolumes method sets the individual volume levels for all the channels in the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-ichannelaudiovolume-setallvolumes"></a></remarks>
    [PreserveSig]
    int SetAllVolumes(uint count, [MarshalAs(UnmanagedType.LPArray)] float[] levels, ref Guid eventContent);

    /// <summary>
    /// The GetAllVolumes method retrieves the volume levels for all the channels in the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-ichannelaudiovolume-getallvolumes"></a></remarks>
    [PreserveSig]
    int GetAllVolumes(uint count, [MarshalAs(UnmanagedType.LPArray)] float[] levels);
}
