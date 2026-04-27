namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioClock interface enables a client to monitor a stream's data rate and the current position in the stream.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nn-audioclient-iaudioclock"></a></remarks>
[Guid("CD63314F-3FBA-4A1B-812C-EF96358728E7")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IAudioClock
{
    /// <summary>
    /// The GetFrequency method gets the device frequency.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclock-getfrequency"></a></remarks>
    [PreserveSig]
    int GetFrequency(out ulong frequency);

    /// <summary>
    /// The GetPosition method gets the current device position.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclock-getposition"></a></remarks>
    [PreserveSig]
    int GetPosition(out ulong devicePosition, out ulong qpcPosition);

    /// <summary>
    /// The GetCharacteristics method is reserved for future use.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-iaudioclock-getcharacteristics"></a></remarks>
    [PreserveSig]
    int GetCharacteristics(out uint characteristics);
}
