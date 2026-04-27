namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The ISimpleAudioVolume interface enables a client to control the master volume level of an audio session.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nn-audioclient-isimpleaudiovolume"></a></remarks>
[Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface ISimpleAudioVolume
{
    /// <summary>
    /// Sets the master volume level for the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-isimpleaudiovolume-setmastervolume"></a></remarks>
    /// <param name="levelNorm">The new volume level expressed as a normalized value between 0.0 and 1.0.</param>
    /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
    /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
    [PreserveSig]
    int SetMasterVolume([In][MarshalAs(UnmanagedType.R4)] float level, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    /// <summary>
    /// Retrieves the client volume level for the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-isimpleaudiovolume-getmastervolume"></a></remarks>
    /// <param name="levelNorm">Receives the volume level expressed as a normalized value between 0.0 and 1.0. </param>
    /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
    [PreserveSig]
    int GetMasterVolume([Out][MarshalAs(UnmanagedType.R4)] out float level);

    /// <summary>
    /// Sets the muting state for the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-isimpleaudiovolume-setmute"></a></remarks>
    /// <param name="isMuted">The new muting state.</param>
    /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
    /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
    [PreserveSig]
    int SetMute([In][MarshalAs(UnmanagedType.Bool)] bool isMuted, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    /// <summary>
    /// Retrieves the current muting state for the audio session.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-isimpleaudiovolume-getmute"></a></remarks>
    /// <param name="isMuted">Receives the muting state.</param>
    /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
    [PreserveSig]
    int GetMute([Out][MarshalAs(UnmanagedType.Bool)] out bool isMuted);
}
