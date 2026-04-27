namespace WindowsInterop.Win32;

using System.Runtime.InteropServices;

public static class Shell32
{
    public const int KF_FLAG_DONT_VERIFY = 0x00004000;

    /// <summary>
    /// The ExtractIconEx function creates an array of handles to large or small icons
    /// extracted from the specified executable file, DLL, or icon file.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-extracticonexw"></a></remarks>
    [DllImport("shell32.dll", EntryPoint = "ExtractIconExW", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern int ExtractIconEx(string file, int iconIndex, out IntPtr iconLarge, out IntPtr iconSmall, int icons);

    /// <summary>
    /// Creates a Shell item object for a single file that exists inside a known folder.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-shcreateiteminknownfolder"></a></remarks>
    [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public static extern IShellItem2 SHCreateItemInKnownFolder([MarshalAs(UnmanagedType.LPStruct)] Guid kfid, uint dwKFFlags, [MarshalAs(UnmanagedType.LPWStr)] string pszItem, [MarshalAs(UnmanagedType.LPStruct)] Guid riid);

    /// <summary>
    /// Creates and initializes a Shell item object from a parsing name.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-shcreateitemfromparsingname"></a></remarks>
    [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
    [return: MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)]
    public static extern IShellItem2 SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IntPtr pbc, [MarshalAs(UnmanagedType.LPStruct)] Guid riid);
}
