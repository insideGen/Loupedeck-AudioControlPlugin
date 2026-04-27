namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioClient interface enables a client to create and initialize an audio stream between an audio application
/// and the audio engine (for a shared-mode stream) or the hardware buffer of an audio endpoint device (for an exclusive-mode stream).
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nn-audioclient-iaudioclient"></a></remarks>
[Guid("1CB9AD4C-DBFA-4C32-B178-C2F568A703B2")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IAudioClient
{
    /// <summary>
    /// The Initialize method initializes the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-initialize"></a></remarks>
    [PreserveSig]
    int Initialize(AudioClientShareMode shareMode, AudioClientStreamFlags streamFlags, long bufferDuration, long periodicity, [In] WaveFormat format, [In] ref Guid audioSessionGuid);

    /// <summary>
    /// The GetBufferSize method retrieves the size (maximum capacity) of the endpoint buffer.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-getbuffersize"></a></remarks>
    int GetBufferSize(out uint numBufferFrames);

    /// <summary>
    /// The GetStreamLatency method retrieves the maximum latency for the current stream
    /// and can be called any time after the stream has been initialized.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-getstreamlatency"></a></remarks>
    [return: MarshalAs(UnmanagedType.I8)]
    long GetStreamLatency();

    /// <summary>
    /// The GetCurrentPadding method retrieves the number of frames of padding in the endpoint buffer.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-getcurrentpadding"></a></remarks>
    int GetCurrentPadding(out int numPaddingFrames);

    /// <summary>
    /// The IsFormatSupported method indicates whether the audio endpoint device supports a particular stream format.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-isformatsupported"></a></remarks>
    [PreserveSig]
    int IsFormatSupported(AudioClientShareMode shareMode, [In] WaveFormat format, [Out] out IntPtr closestMatchPointer);

    /// <summary>
    /// The GetMixFormat method retrieves the stream format that the audio engine uses for its internal processing
    /// of shared-mode streams.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-getmixformat"></a></remarks>
    int GetMixFormat(out IntPtr deviceFormatPointer);

    /// <summary>
    /// The GetDevicePeriod method retrieves the length of the periodic interval separating successive processing passes
    /// by the audio engine on the data in the endpoint buffer.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-getdeviceperiod"></a></remarks>
    int GetDevicePeriod(out long defaultDevicePeriod, out long minimumDevicePeriod);

    /// <summary>
    /// The Start method starts the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-start"></a></remarks>
    int Start();

    /// <summary>
    /// The Stop method stops the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-stop"></a></remarks>
    int Stop();

    /// <summary>
    /// The Reset method resets the audio stream.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-reset"></a></remarks>
    int Reset();

    /// <summary>
    /// The SetEventHandle method sets the event handle that the system signals
    /// when an audio buffer is ready to be processed by the client.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-seteventhandle"></a></remarks>
    int SetEventHandle(IntPtr eventHandle);

    /// <summary>
    /// The GetService method accesses additional services from the audio client object.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclient-getservice"></a></remarks>
    [PreserveSig]
    int GetService([In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceId, [Out, MarshalAs(UnmanagedType.IUnknown)] out object interfacePointer);
}
