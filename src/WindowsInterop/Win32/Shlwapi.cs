namespace WindowsInterop.Win32;

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

public static class Shlwapi
{
    /// <summary>
    /// Extracts a specified text resource when given that resource in the form
    /// of an indirect string (a string that begins with the '@' symbol).
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shlwapi/nf-shlwapi-shloadindirectstring"></a></remarks>
    [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true)]
    public static extern int SHLoadIndirectString(string pszSource, StringBuilder pszOutBuf, int cchOutBuf, IntPtr ppvReserved);

    /// <summary>
    /// Opens or creates a file and retrieves a stream to read or write to that file.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shlwapi/nf-shlwapi-shcreatestreamonfileex"></a></remarks>
    [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
    public static extern int SHCreateStreamOnFileEx(string fileName, int grfMode, int attributes, bool create, IntPtr reserved, out IStream stream);

    /// <summary>
    /// Parses a file location string that contains a file location and icon index, and returns separate values.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shlwapi/nf-shlwapi-pathparseiconlocationw"></a></remarks>
    [DllImport("shlwapi.dll", EntryPoint = "PathParseIconLocationW", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true)]
    public static extern int PathParseIconLocation([MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconFile);
}
