namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioMeterInformation interface represents a peak meter on an audio stream to or from an audio endpoint device.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nn-endpointvolume-iaudiometerinformation"></a></remarks>
[Guid("C02216F6-8C67-4B5B-9D00-D008E73E0064")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IAudioMeterInformation
{
    /// <summary>
    /// The GetPeakValue method gets the peak sample value for the channels in the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudiometerinformation-getpeakvalue"></a></remarks>
    int GetPeakValue(out float peak);

    /// <summary>
    /// The GetMeteringChannelCount method gets the number of channels in the audio stream that are monitored by peak meters.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudiometerinformation-getmeteringchannelcount"></a></remarks>
    int GetMeteringChannelCount(out int channelCount);

    /// <summary>
    /// The GetChannelsPeakValues method gets the peak sample values for all the channels in the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudiometerinformation-getchannelspeakvalues"></a></remarks>
    int GetChannelsPeakValues(int channelCount, [In] IntPtr peakValues);

    /// <summary>
    /// The QueryHardwareSupport method queries the audio endpoint device for its hardware-supported functions.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudiometerinformation-queryhardwaresupport"></a></remarks>
    int QueryHardwareSupport(out int hardwareSupportMask);
}
