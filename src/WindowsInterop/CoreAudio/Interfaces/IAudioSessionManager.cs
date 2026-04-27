namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioSessionManager interface enables a client to access the session controls and volume controls
/// for both cross-process and process-specific audio sessions.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nn-audiopolicy-iaudiosessionmanager"></a></remarks>
[Guid("BFA971F1-4D5E-40BB-935E-967039BFBEE4")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IAudioSessionManager
{
    /// <summary>
    /// Retrieves an audio session control.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionmanager-getaudiosessioncontrol"></a></remarks>
    /// <param name="sessionId">A new or existing session ID.</param>
    /// <param name="streamFlags">Audio session flags.</param>
    /// <param name="sessionControl">Receives an <see cref="IAudioSessionControl"/> interface for the audio session.</param>
    /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
    [PreserveSig]
    int GetAudioSessionControl([In, Optional][MarshalAs(UnmanagedType.LPStruct)] Guid sessionId, [In][MarshalAs(UnmanagedType.U4)] uint streamFlags, [Out][MarshalAs(UnmanagedType.Interface)] out IAudioSessionControl sessionControl);

    /// <summary>
    /// Retrieves a simple audio volume control.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionmanager-getsimpleaudiovolume"></a></remarks>
    /// <param name="sessionId">A new or existing session ID.</param>
    /// <param name="streamFlags">Audio session flags.</param>
    /// <param name="audioVolume">Receives an <see cref="ISimpleAudioVolume"/> interface for the audio session.</param>
    /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
    [PreserveSig]
    int GetSimpleAudioVolume([In, Optional][MarshalAs(UnmanagedType.LPStruct)] Guid sessionId, [In][MarshalAs(UnmanagedType.U4)] uint streamFlags, [Out][MarshalAs(UnmanagedType.Interface)] out ISimpleAudioVolume audioVolume);
}
