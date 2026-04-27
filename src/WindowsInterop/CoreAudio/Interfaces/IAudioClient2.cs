namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioClient2 interface is derived from the IAudioClient interface, with a set of additional methods
/// that enable a Windows Audio Session API (WASAPI) audio client to do the following: opt in for offloading,
/// query stream properties, and get information from the hardware that handles offloading.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nn-audioclient-iaudioclient2"></a></remarks>
[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("726778CD-F60A-4EDA-82DE-E47610CD78AA")]
internal interface IAudioClient2 : IAudioClient
{
    /// <summary>
    /// The Initialize method initializes the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-initialize"></a></remarks>
    [PreserveSig]
    new int Initialize(AudioClientShareMode shareMode, AudioClientStreamFlags streamFlags, long bufferDuration, long periodicity, [In] WaveFormat format, [In] ref Guid audioSessionGuid);

    /// <summary>
    /// The GetBufferSize method retrieves the size (maximum capacity) of the endpoint buffer.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-getbuffersize"></a></remarks>
    new int GetBufferSize(out uint numBufferFrames);

    /// <summary>
    /// The GetStreamLatency method retrieves the maximum latency for the current stream
    /// and can be called any time after the stream has been initialized.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-getstreamlatency"></a></remarks>
    [return: MarshalAs(UnmanagedType.I8)]
    new long GetStreamLatency();

    /// <summary>
    /// The GetCurrentPadding method retrieves the number of frames of padding in the endpoint buffer.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-getcurrentpadding"></a></remarks>
    new int GetCurrentPadding(out int numPaddingFrames);

    /// <summary>
    /// The IsFormatSupported method indicates whether the audio endpoint device supports a particular stream format.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-isformatsupported"></a></remarks>
    [PreserveSig]
    new int IsFormatSupported(AudioClientShareMode shareMode, [In] WaveFormat format, [Out] out IntPtr closestMatchPointer);

    /// <summary>
    /// The GetMixFormat method retrieves the stream format that the audio engine uses for its internal processing
    /// of shared-mode streams.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-getmixformat"></a></remarks>
    new int GetMixFormat(out IntPtr deviceFormatPointer);

    /// <summary>
    /// The GetDevicePeriod method retrieves the length of the periodic interval separating successive processing passes
    /// by the audio engine on the data in the endpoint buffer.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-getdeviceperiod"></a></remarks>
    new int GetDevicePeriod(out long defaultDevicePeriod, out long minimumDevicePeriod);

    /// <summary>
    /// The Start method starts the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-start"></a></remarks>
    new int Start();

    /// <summary>
    /// The Stop method stops the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-stop"></a></remarks>
    new int Stop();

    /// <summary>
    /// The Reset method resets the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-reset"></a></remarks>
    new int Reset();

    /// <summary>
    /// The SetEventHandle method sets the event handle that the system signals
    /// when an audio buffer is ready to be processed by the client.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-seteventhandle"></a></remarks>
    new int SetEventHandle(IntPtr eventHandle);

    /// <summary>
    /// The GetService method accesses additional services from the audio client object.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-getservice"></a></remarks>
    [PreserveSig]
    new int GetService([In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceId, [Out, MarshalAs(UnmanagedType.IUnknown)] out object interfacePointer);

    /// <summary>
    /// The IsOffloadCapable method retrieves information about whether or not the endpoint
    /// on which a stream is created is capable of supporting an offloaded audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient2-isoffloadcapable"></a></remarks>
    void IsOffloadCapable(AudioStreamCategory category, out bool offloadCapable);

    /// <summary>
    /// Sets the properties of the audio stream by populating an AudioClientProperties structure.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient2-setclientproperties"></a></remarks>
    void SetClientProperties([In] IntPtr propertiesPointer);

    /// <summary>
    /// The GetBufferSizeLimits method returns the buffer size limits of the hardware audio engine in 100-nanosecond units.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient2-getbuffersizelimits"></a></remarks>
    void GetBufferSizeLimits(IntPtr formatPointer, bool eventDriven, out long minBufferDuration, out long maxBufferDuration);
}
