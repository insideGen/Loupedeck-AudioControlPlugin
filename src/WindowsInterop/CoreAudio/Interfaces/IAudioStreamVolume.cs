namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioStreamVolume interface enables a client to control and monitor the volume levels
/// for all of the channels in an audio stream.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nn-audioclient-iaudiostreamvolume"></a></remarks>
[Guid("93014887-242D-4068-8A15-CF5E93B90FE3")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IAudioStreamVolume
{
    /// <summary>
    /// The GetChannelCount method retrieves the number of channels in the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudiostreamvolume-getchannelcount"></a></remarks>
    [PreserveSig]
    int GetChannelCount([Out] out uint count);

    /// <summary>
    /// The SetChannelVolume method sets the volume level for the specified channel in the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudiostreamvolume-setchannelvolume"></a></remarks>
    [PreserveSig]
    int SetChannelVolume([In] uint index, [In] float level);

    /// <summary>
    /// The GetChannelVolume method retrieves the volume level for the specified channel in the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudiostreamvolume-getchannelvolume"></a></remarks>
    [PreserveSig]
    int GetChannelVolume([In] uint index, [Out] out float level);

    /// <summary>
    /// The SetAllVolumes method sets the individual volume levels for all the channels in the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudiostreamvolume-setallvolumes"></a></remarks>
    [PreserveSig]
    int SetAllVoumes([In] uint count, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4, SizeParamIndex = 0)] float[] volumes);

    /// <summary>
    /// The GetAllVolumes method retrieves the volume levels for all the channels in the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudiostreamvolume-getallvolumes"></a></remarks>
    [PreserveSig]
    int GetAllVolumes([In] uint count, [MarshalAs(UnmanagedType.LPArray)] float[] volumes);
}
