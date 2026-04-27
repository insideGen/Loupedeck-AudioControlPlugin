namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// MME Wave function interop
/// </summary>
public class WaveInterop
{
    [Flags]
    public enum WaveInOutOpenFlags
    {
        /// <summary>
        /// CALLBACK_NULL
        /// No callback
        /// </summary>
        CallbackNull = 0,
        /// <summary>
        /// CALLBACK_FUNCTION
        /// dwCallback is a FARPROC 
        /// </summary>
        CallbackFunction = 0x30000,
        /// <summary>
        /// CALLBACK_EVENT
        /// dwCallback is an EVENT handle 
        /// </summary>
        CallbackEvent = 0x50000,
        /// <summary>
        /// CALLBACK_WINDOW
        /// dwCallback is a HWND 
        /// </summary>
        CallbackWindow = 0x10000,
        /// <summary>
        /// CALLBACK_THREAD
        /// callback is a thread ID 
        /// </summary>
        CallbackThread = 0x20000
        /*
        WAVE_FORMAT_QUERY = 1,
        WAVE_MAPPED = 4,
        WAVE_FORMAT_DIRECT = 8
        */
    }

    public enum WaveMessage
    {
        /// <summary>
        /// WIM_OPEN
        /// </summary>
        WaveInOpen = 0x3BE,
        /// <summary>
        /// WIM_CLOSE
        /// </summary>
        WaveInClose = 0x3BF,
        /// <summary>
        /// WIM_DATA
        /// </summary>
        WaveInData = 0x3C0,
        /// <summary>
        /// WOM_CLOSE
        /// </summary>
        WaveOutClose = 0x3BC,
        /// <summary>
        /// WOM_DONE
        /// </summary>
        WaveOutDone = 0x3BD,
        /// <summary>
        /// WOM_OPEN
        /// </summary>
        WaveOutOpen = 0x3BB
    }

    /// <summary>
    /// The waveInOpen function opens the given waveform-audio input device for recording.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmeapi/nf-mmeapi-waveinopen"></a></remarks>
    [DllImport("winmm.dll")]
    public static extern int waveInOpen(out IntPtr hWaveIn, IntPtr uDeviceID, WaveFormat lpFormat, IntPtr callbackWindowHandle, IntPtr dwInstance, WaveInOutOpenFlags dwFlags);

    /// <summary>
    /// The waveInClose function closes the given waveform-audio input device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmeapi/nf-mmeapi-waveinclose"></a></remarks>
    [DllImport("winmm.dll")]
    public static extern int waveInClose(IntPtr hWaveIn);

    /// <summary>
    /// The waveInStart function starts input on the given waveform-audio input device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmeapi/nf-mmeapi-waveinstart"></a></remarks>
    [DllImport("winmm.dll")]
    public static extern int waveInStart(IntPtr hWaveIn);

    /// <summary>
    /// The waveInStop function stops waveform-audio input.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmeapi/nf-mmeapi-waveinstop"></a></remarks>
    [DllImport("winmm.dll")]
    public static extern int waveInStop(IntPtr hWaveIn);

    /// <summary>
    /// The waveInReset function stops input on the given waveform-audio input device and resets the current
    /// position to zero. All pending buffers are marked as done and returned to the application.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmeapi/nf-mmeapi-waveinreset"></a></remarks>
    [DllImport("winmm.dll")]
    public static extern int waveInReset(IntPtr hWaveIn);

    /// <summary>
    /// The waveInUnprepareHeader function cleans up the preparation performed by the waveInPrepareHeader function.
    /// This function must be called after the device driver fills a buffer and returns it to the application.
    /// You must call this function before freeing the buffer.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmeapi/nf-mmeapi-waveinunprepareheader"></a></remarks>
    [DllImport("winmm.dll")]
    public static extern int waveInUnprepareHeader(IntPtr hWaveIn, WaveHeader lpWaveInHdr, int uSize);

    /// <summary>
    /// The waveInPrepareHeader function prepares a buffer for waveform-audio input.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmeapi/nf-mmeapi-waveinprepareheader"></a></remarks>
    [DllImport("winmm.dll")]
    public static extern int waveInPrepareHeader(IntPtr hWaveIn, WaveHeader lpWaveInHdr, int uSize);

    /// <summary>
    /// The waveInAddBuffer function sends an input buffer to the given waveform-audio input device.
    /// When the buffer is filled, the application is notified.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmeapi/nf-mmeapi-waveinaddbuffer"></a></remarks>
    [DllImport("winmm.dll")]
    public static extern int waveInAddBuffer(IntPtr hWaveIn, WaveHeader pwh, int cbwh);
}
