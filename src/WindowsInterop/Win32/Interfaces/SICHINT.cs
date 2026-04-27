namespace WindowsInterop.Win32;

/// <summary>
/// Used to determine how to compare two Shell items. IShellItem::Compare uses this enumerated type.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/ne-shobjidl_core-_sichintf"></a></remarks>
[Flags]
public enum SICHINT : uint
{
    /// <summary>iOrder based on display in a folder view</summary>
    DISPLAY = 0x00000000,
    /// <summary>exact instance compare</summary>
    ALLFIELDS = 0x80000000,
    /// <summary>iOrder based on canonical name (better performance)</summary>
    CANONICAL = 0x10000000,
    /// <summary>Windows 7 and later.</summary>
    TEST_FILESYSPATH_IF_NOT_EQUAL = 0x20000000
}
