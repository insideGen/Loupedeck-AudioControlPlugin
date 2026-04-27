namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioSessionManager2 interface enables an application to manage submixes for the audio device.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nn-audiopolicy-iaudiosessionmanager2"></a></remarks>
[Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IAudioSessionManager2 : IAudioSessionManager
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
    new int GetAudioSessionControl([In, Optional][MarshalAs(UnmanagedType.LPStruct)] Guid sessionId, [In][MarshalAs(UnmanagedType.U4)] uint streamFlags, [Out][MarshalAs(UnmanagedType.Interface)] out IAudioSessionControl sessionControl);

    /// <summary>
    /// Retrieves a simple audio volume control.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionmanager-getsimpleaudiovolume"></a></remarks>
    /// <param name="sessionId">A new or existing session ID.</param>
    /// <param name="streamFlags">Audio session flags.</param>
    /// <param name="audioVolume">Receives an <see cref="ISimpleAudioVolume"/> interface for the audio session.</param>
    /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
    [PreserveSig]
    new int GetSimpleAudioVolume([In, Optional][MarshalAs(UnmanagedType.LPStruct)] Guid sessionId, [In][MarshalAs(UnmanagedType.U4)] uint streamFlags, [Out][MarshalAs(UnmanagedType.Interface)] out ISimpleAudioVolume audioVolume);

    /// <summary>
    /// The GetSessionEnumerator method gets a pointer to the audio session enumerator object.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionmanager2-getsessionenumerator"></a></remarks>
    [PreserveSig]
    int GetSessionEnumerator(out IAudioSessionEnumerator sessionEnumerator);

    /// <summary>
    /// The RegisterSessionNotification method registers the application to receive a notification when a session is created.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionmanager2-registersessionnotification"></a></remarks>
    [PreserveSig]
    int RegisterSessionNotification(IAudioSessionNotification sessionNotification);

    /// <summary>
    /// The UnregisterSessionNotification method deletes the registration to receive a notification when a session is created.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionmanager2-unregistersessionnotification"></a></remarks>
    [PreserveSig]
    int UnregisterSessionNotification(IAudioSessionNotification sessionNotification);

    /// <summary>
    /// The RegisterDuckNotification method registers the application with the session manager to receive ducking notifications.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionmanager2-registerducknotification"></a></remarks>
    [PreserveSig]
    int RegisterDuckNotification(string sessionId, IAudioSessionNotification audioVolumeDuckNotification);

    /// <summary>
    /// The UnregisterDuckNotification method deletes a previous registration by the application to receive notifications.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audiopolicy/nf-audiopolicy-iaudiosessionmanager2-unregisterducknotification"></a></remarks>
    [PreserveSig]
    int UnregisterDuckNotification(IntPtr audioVolumeDuckNotification);
}
