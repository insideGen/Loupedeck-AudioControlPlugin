namespace WindowsInterop.ModernApp;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct PackageInfo
{
    public int Reserved;

    public int Flags;

    [field: MarshalAs(UnmanagedType.LPWStr)]
    public string Path;

    [field: MarshalAs(UnmanagedType.LPWStr)]
    public string PackageFullName;

    [field: MarshalAs(UnmanagedType.LPWStr)]
    public string PackageFamilyName;

    public PackageId PackageId;
}
