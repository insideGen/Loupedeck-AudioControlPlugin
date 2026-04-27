namespace WindowsInterop.ModernApp;

/// <summary>
/// Specifies how packages are to be processed.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/appxpkg/package-constants"></a></remarks>
[Flags]
public enum PackageConstants
{
    PACKAGE_FILTER_ALL_LOADED = 0x00000000,
    PACKAGE_PROPERTY_FRAMEWORK = 0x00000001,
    PACKAGE_PROPERTY_RESOURCE = 0x00000002,
    PACKAGE_PROPERTY_BUNDLE = 0x00000004,
    PACKAGE_FILTER_HEAD = 0x00000010,
    PACKAGE_FILTER_DIRECT = 0x00000020,
    PACKAGE_FILTER_RESOURCE = 0x00000040,
    PACKAGE_FILTER_BUNDLE = 0x00000080,
    PACKAGE_INFORMATION_BASIC = 0x00000000,
    PACKAGE_INFORMATION_FULL = 0x00000100,
    PACKAGE_PROPERTY_DEVELOPMENT_MODE = 0x00010000
}
