namespace WindowsInterop.PropertySystem;

public static class PropertyKeys
{
    public static PropertyKey PKEY_AppUserModel_Background = new PropertyKey
    {
        FormatId = Guid.Parse("{86D40B4D-9069-443C-819A-2A54090DCCEC}"),
        PropertyId = new UIntPtr(4)
    };

    public static PropertyKey PKEY_AppUserModel_PackageFullName = new PropertyKey
    {
        FormatId = Guid.Parse("{9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3}"),
        PropertyId = new UIntPtr(21)
    };

    public static PropertyKey PKEY_AppUserModel_PackageInstallPath = new PropertyKey
    {
        FormatId = Guid.Parse("{9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3}"),
        PropertyId = new UIntPtr(15)
    };

    public static PropertyKey PKEY_AudioEndPoint_Interface = new PropertyKey
    {
        FormatId = Guid.Parse("{A45C254E-DF1C-4EFD-8020-67D146A850E0}"),
        PropertyId = new UIntPtr(2)
    };

    public static PropertyKey PKEY_AudioEndpoint_PhysicalSpeakers = new PropertyKey
    {
        FormatId = Guid.Parse("{0x1DA5D803, 0xD492, 0x4EDD, {0x8C, 0x23, 0xE0, 0xC0, 0xFF, 0xEE, 0x7F, 0x0E}}"),
        PropertyId = new UIntPtr(3)
    };

    public static PropertyKey PKEY_Device_DeviceDesc = new PropertyKey
    {
        FormatId = Guid.Parse("{0xA45C254E, 0xDF1C, 0x4EFD, {0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0}}"),
        PropertyId = new UIntPtr(2)
    };

    public static PropertyKey PKEY_Device_EnumeratorName = new PropertyKey
    {
        FormatId = Guid.Parse("{0xA45C254E, 0xDF1C, 0x4EFD, {0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0}}"),
        PropertyId = new UIntPtr(24)
    };

    public static PropertyKey PKEY_Device_FriendlyName = new PropertyKey
    {
        FormatId = Guid.Parse("{0xA45C254E, 0xDF1C, 0x4EFD, {0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0}}"),
        PropertyId = new UIntPtr(14)
    };

    public static PropertyKey PKEY_Device_IconPath = new PropertyKey
    {
        FormatId = Guid.Parse("{0x259ABFFC, 0x50A7, 0x47CE, {0xAF, 0x8, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66}}"),
        PropertyId = new UIntPtr(12)
    };

    public static PropertyKey PKEY_Device_InstanceId = new PropertyKey
    {
        FormatId = Guid.Parse("{0x78C34FC8, 0x104A, 0x4ACA, {0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57}}"),
        PropertyId = new UIntPtr(2)
    };

    public static PropertyKey PKEY_DeviceInterface_FriendlyName = new PropertyKey
    {
        FormatId = Guid.Parse("{0x026E516E, 0xB814, 0x414B, {0x83, 0xCD, 0x85, 0x6D, 0x6F, 0xEF, 0x48, 0x22}}"),
        PropertyId = new UIntPtr(2)
    };

    public static PropertyKey PKEY_ItemNameDisplay = new PropertyKey
    {
        FormatId = Guid.Parse("{B725F130-47EF-101A-A5F1-02608C9EEBAC}"),
        PropertyId = new UIntPtr(10)
    };

    public static PropertyKey PKEY_Tile_SmallLogoPath = new PropertyKey
    {
        FormatId = Guid.Parse("{86D40B4D-9069-443C-819A-2A54090DCCEC}"),
        PropertyId = new UIntPtr(2)
    };
}
