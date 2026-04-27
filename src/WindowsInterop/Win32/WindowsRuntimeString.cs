namespace WindowsInterop.Win32;

using System.Runtime.InteropServices;

/// <summary>
/// Native P/Invoke wrappers for Windows Runtime String (HSTRING) APIs.
/// Replaces the need for Microsoft.Windows.CsWinRT package dependency.
/// </summary>
internal static class WindowsRuntimeString
{
    /// <summary>
    /// Creates a new HSTRING from a managed string.
    /// </summary>
    /// <param name="sourceString">The source string to convert.</param>
    /// <param name="length">The length of the source string in characters.</param>
    /// <param name="hstring">Receives the created HSTRING handle.</param>
    /// <returns>HRESULT indicating success or failure.</returns>
    [DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = CallingConvention.StdCall, PreserveSig = true)]
    private static extern int WindowsCreateString([MarshalAs(UnmanagedType.LPWStr)] string sourceString, int length, out IntPtr hstring);

    /// <summary>
    /// Deletes a HSTRING and frees its memory.
    /// </summary>
    /// <param name="hstring">The HSTRING handle to delete.</param>
    /// <returns>HRESULT indicating success or failure.</returns>
    [DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = CallingConvention.StdCall, PreserveSig = true)]
    private static extern int WindowsDeleteString(IntPtr hstring);

    /// <summary>
    /// Gets the raw buffer from a HSTRING.
    /// </summary>
    /// <param name="hstring">The HSTRING handle.</param>
    /// <param name="length">Receives the length of the string in characters.</param>
    /// <returns>Pointer to the raw string buffer (UTF-16).</returns>
    [DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = CallingConvention.StdCall, PreserveSig = true)]
    private static extern IntPtr WindowsGetStringRawBuffer(IntPtr hstring, out int length);

    /// <summary>
    /// Creates a HSTRING from a managed string.
    /// </summary>
    /// <param name="str">The string to convert, or null for an empty HSTRING.</param>
    /// <returns>The HSTRING handle.</returns>
    /// <exception cref="COMException">Thrown if HSTRING creation fails.</exception>
    public static IntPtr FromManaged(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return IntPtr.Zero;
        }

        int hr = WindowsCreateString(str, str.Length, out IntPtr hstring);
        if (hr < 0)
        {
            Marshal.ThrowExceptionForHR(hr);
        }

        return hstring;
    }

    /// <summary>
    /// Converts a HSTRING to a managed string.
    /// </summary>
    /// <param name="hstring">The HSTRING handle to convert.</param>
    /// <returns>The managed string, or null if the HSTRING is empty.</returns>
    public static string FromAbi(IntPtr hstring)
    {
        if (hstring == IntPtr.Zero)
        {
            return null;
        }

        IntPtr buffer = WindowsGetStringRawBuffer(hstring, out int length);
        if (buffer == IntPtr.Zero || length == 0)
        {
            return string.Empty;
        }

        return Marshal.PtrToStringUni(buffer, length);
    }

    /// <summary>
    /// Disposes a HSTRING and frees its memory.
    /// </summary>
    /// <param name="hstring">The HSTRING handle to dispose.</param>
    public static void DisposeAbi(IntPtr hstring)
    {
        if (hstring != IntPtr.Zero)
        {
            WindowsDeleteString(hstring);
        }
    }
}
