namespace WindowsInterop.ModernApp;

using System.Runtime.InteropServices;

/// <summary>
/// Represents package identification information, such as name, version, and publisher.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/appmodel/ns-appmodel-package_id"></a></remarks>
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public readonly struct PackageId
{
    public int Reserved { get; }

    public AppxPackageArchitecture ProcessorArchitecture { get; }

    public ushort VersionRevision { get; }

    public ushort VersionBuild { get; }

    public ushort VersionMinor { get; }

    public ushort VersionMajor { get; }

    [field: MarshalAs(UnmanagedType.LPWStr)]
    public string Name { get; }

    [field: MarshalAs(UnmanagedType.LPWStr)]
    public string Publisher { get; }

    [field: MarshalAs(UnmanagedType.LPWStr)]
    public string ResourceId { get; }

    [field: MarshalAs(UnmanagedType.LPWStr)]
    public string PublisherId { get; }
}
