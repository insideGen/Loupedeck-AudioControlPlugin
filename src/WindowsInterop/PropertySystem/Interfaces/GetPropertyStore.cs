namespace WindowsInterop.PropertySystem;

/// <summary>
/// Indicates flags that modify the property store object retrieved by methods that create a property store,
/// such as IShellItem2::GetPropertyStore or IPropertyStoreFactory::GetPropertyStore.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/propsys/ne-propsys-getpropertystoreflags"></a></remarks>
public enum GetPropertyStore
{
    DEFAULT = 0x00000000,
    HANDLERPROPERTIESONLY = 0x00000001,
    READWRITE = 0x00000002,
    TEMPORARY = 0x00000004,
    FASTPROPERTIESONLY = 0x00000008,
    OPENSLOWITEM = 0x00000010,
    DELAYCREATION = 0x00000020,
    BESTEFFORT = 0x00000040,
    NO_OPLOCK = 0x00000080,
    MASK_VALID = 0x000000FF
}
