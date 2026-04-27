namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioEndpointVolume interface represents the volume controls on the audio stream to or from an audio endpoint device.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nn-endpointvolume-iaudioendpointvolume"></a></remarks>
[Guid("5CDF2C82-841E-4546-9722-0CF74078229A")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IAudioEndpointVolume
{
    /// <summary>
    /// The RegisterControlChangeNotify method registers a client's notification callback interface.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-registercontrolchangenotify"></a></remarks>
    int RegisterControlChangeNotify(IAudioEndpointVolumeCallback notify);

    /// <summary>
    /// The UnregisterControlChangeNotify method deletes the registration of a client's notification callback interface
    /// that the client registered in a previous call to the IAudioEndpointVolume::RegisterControlChangeNotify method.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-unregistercontrolchangenotify"></a></remarks>
    int UnregisterControlChangeNotify(IAudioEndpointVolumeCallback notify);

    /// <summary>
    /// The GetChannelCount method gets a count of the channels in the audio stream that enters
    /// or leaves the audio endpoint device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-getchannelcount"></a></remarks>
    int GetChannelCount(out int channelCount);

    /// <summary>
    /// The SetMasterVolumeLevel method sets the master volume level, in decibels, of the audio stream that enters
    /// or leaves the audio endpoint device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-setmastervolumelevel"></a></remarks>
    int SetMasterVolumeLevel(float leveldB, ref Guid guidEventContext);

    /// <summary>
    /// The SetMasterVolumeLevelScalar method sets the master volume level of the audio stream that enters
    /// or leaves the audio endpoint device.
    /// The volume level is expressed as a normalized, audio-tapered value in the range from 0.0 to 1.0.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-setmastervolumelevelscalar"></a></remarks>
    int SetMasterVolumeLevelScalar(float level, ref Guid guidEventContext);

    /// <summary>
    /// The GetMasterVolumeLevel method gets the master volume level, in decibels, of the audio stream that enters
    /// or leaves the audio endpoint device.<br />
    /// <a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-getmastervolumelevel"></a>
    /// </summary>
    int GetMasterVolumeLevel(out float leveldB);

    /// <summary>
    /// The GetMasterVolumeLevelScalar method gets the master volume level of the audio stream that enters
    /// or leaves the audio endpoint device.
    /// The volume level is expressed as a normalized, audio-tapered value in the range from 0.0 to 1.0.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-getmastervolumelevelscalar"></a></remarks>
    int GetMasterVolumeLevelScalar(out float level);

    /// <summary>
    /// The SetChannelVolumeLevel method sets the volume level, in decibels, of the specified channel
    /// of the audio stream that enters or leaves the audio endpoint device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-setchannelvolumelevel"></a></remarks>
    int SetChannelVolumeLevel(uint nChannel, float leveldB, ref Guid guidEventContext);

    /// <summary>
    /// The SetChannelVolumeLevelScalar method sets the normalized, audio-tapered volume level of the specified channel
    /// in the audio stream that enters or leaves the audio endpoint device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-setchannelvolumelevelscalar"></a></remarks>
    int SetChannelVolumeLevelScalar(uint nChannel, float level, ref Guid guidEventContext);

    /// <summary>
    /// The GetChannelVolumeLevel method gets the volume level, in decibels, of the specified channel
    /// in the audio stream that enters or leaves the audio endpoint device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-getchannelvolumelevel"></a></remarks>
    int GetChannelVolumeLevel(uint nChannel, out float leveldB);

    /// <summary>
    /// The GetChannelVolumeLevelScalar method gets the normalized, audio-tapered volume level of the specified channel
    /// of the audio stream that enters or leaves the audio endpoint device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-getchannelvolumelevelscalar"></a></remarks>
    int GetChannelVolumeLevelScalar(uint nChannel, out float level);

    /// <summary>
    /// The SetMute method sets the muting state of the audio stream that enters or leaves the audio endpoint device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-setmute"></a></remarks>
    int SetMute([MarshalAs(UnmanagedType.Bool)] bool mute, ref Guid guidEventContext);

    /// <summary>
    /// The GetMute method gets the muting state of the audio stream that enters or leaves the audio endpoint device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-getmute"></a></remarks>
    int GetMute(out bool mute);

    /// <summary>
    /// The GetVolumeStepInfo method gets information about the current step in the volume range.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-getvolumestepinfo"></a></remarks>
    int GetVolumeStepInfo(out uint step, out uint stepCount);

    /// <summary>
    /// The VolumeStepUp method increments, by one step, the volume level of the audio stream that enters
    /// or leaves the audio endpoint device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-volumestepup"></a></remarks>
    int VolumeStepUp(ref Guid guidEventContext);

    /// <summary>
    /// The VolumeStepDown method decrements, by one step, the volume level of the audio stream that enters
    /// or leaves the audio endpoint device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-volumestepdown"></a></remarks>
    int VolumeStepDown(ref Guid guidEventContext);

    /// <summary>
    /// The QueryHardwareSupport method queries the audio endpoint device for its hardware-supported functions.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-queryhardwaresupport"></a></remarks>
    int QueryHardwareSupport(out uint hardwareSupportMask);

    /// <summary>
    /// The GetVolumeRange method gets the volume range, in decibels, of the audio stream that enters
    /// or leaves the audio endpoint device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-getvolumerange"></a></remarks>
    int GetVolumeRange(out float volumeMindB, out float volumeMaxdB, out float volumeIncrementdB);
}
