namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The WAVEHDR structure defines the header used to identify a waveform-audio buffer.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmeapi/ns-mmeapi-wavehdr"></a></remarks>
[StructLayout(LayoutKind.Sequential)]
public sealed class WaveHeader
{
    /// <summary>
    /// Pointer to the waveform buffer.
    /// </summary>
    public IntPtr data;
    /// <summary>
    /// Length, in bytes, of the buffer.
    /// </summary>
    public int bufferLength;
    /// <summary>
    /// When the header is used in input, specifies how much data is in the buffer.
    /// </summary>
    public int bytesRecorded;
    /// <summary>
    /// User data.
    /// </summary>
    public IntPtr user;
    /// <summary>
    /// A bitwise OR of zero or more flags.
    /// </summary>
    public WaveHeaderFlags flags;
    /// <summary>
    /// Number of times to play the loop. This member is used only with output buffers.
    /// </summary>
    public int loops;
    /// <summary>
    /// Reserved.
    /// </summary>
    public IntPtr next;
    /// <summary>
    /// Reserved.
    /// </summary>
    public IntPtr reserved;
}
