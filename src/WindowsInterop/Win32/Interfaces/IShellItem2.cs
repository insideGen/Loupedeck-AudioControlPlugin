namespace WindowsInterop.Win32;

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using WindowsInterop.PropertySystem;

/// <summary>
/// Extends IShellItem with methods that retrieve various property values of the item.
/// IShellItem and IShellItem2 are the preferred representations of items in any new code.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-ishellitem2"></a></remarks>
[Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IShellItem2 : IShellItem
{
    /// <summary>
    /// Binds to a handler for an item as specified by the handler ID value (BHID).
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem-bindtohandler"></a></remarks>
    new HRESULT BindToHandler([In, MarshalAs(UnmanagedType.Interface)] IntPtr pbc, [In] ref Guid bhid, [In] ref Guid riid, out IntPtr ppv);

    /// <summary>
    /// Gets the parent of an IShellItem object.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem-getparent"></a></remarks>
    new HRESULT GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

    /// <summary>
    /// Gets the display name of the IShellItem object.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem-getdisplayname"></a></remarks>
    new HRESULT GetDisplayName([In] SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

    /// <summary>
    /// Gets a requested set of attributes of the IShellItem object.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem-getattributes"></a></remarks>
    new HRESULT GetAttributes([In] uint sfgaoMask, out uint psfgaoAttribs);

    /// <summary>
    /// Compares two IShellItem objects.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem-compare"></a></remarks>
    new HRESULT Compare([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In] uint hint, out int piOrder);

    /// <summary>
    /// Gets a property store object for specified property store flags.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem2-getpropertystore"></a></remarks>
    HRESULT GetPropertyStore(GetPropertyStore flags, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] IPropertyStore ppv);

    /// <summary>
    /// Uses the specified ICreateObject instead of CoCreateInstance to create an instance
    /// of the property handler associated with the Shell item on which this method is called.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem2-getpropertystorewithcreateobject"></a></remarks>
    HRESULT GetPropertyStoreWithCreateObject(GetPropertyStore flags, [MarshalAs(UnmanagedType.IUnknown)] object punkCreateObject, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] IPropertyStore ppv);

    /// <summary>
    /// Gets property store object for specified property keys.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem2-getpropertystoreforkeys"></a></remarks>
    HRESULT GetPropertyStoreForKeys(IntPtr rgKeys, uint cKeys, GetPropertyStore flags, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] IPropertyStore ppv);

    /// <summary>
    /// Gets a property description list object given a reference to a property key.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem2-getpropertydescriptionlist"></a></remarks>
    HRESULT GetPropertyDescriptionList(IntPtr keyType, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] IPropertyStore ppv);

    /// <summary>
    /// Ensures that any cached information in this item is updated.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem2-update"></a></remarks>
    HRESULT Update(IBindCtx pbc);

    /// <summary>
    /// Gets a PROPVARIANT structure from a specified property key.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem2-getproperty"></a></remarks>
    HRESULT GetProperty(ref PropertyKey key, [In, Out] PropVariant pv);

    /// <summary>
    /// Gets the class identifier (CLSID) value of specified property key.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem2-getclsid"></a></remarks>
    HRESULT GetCLSID(ref PropertyKey key, out Guid clsid);

    /// <summary>
    /// Gets the date and time value of a specified property key.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem2-getfiletime"></a></remarks>
    HRESULT GetFileTime(ref PropertyKey key, out System.Runtime.InteropServices.ComTypes.FILETIME ft);

    /// <summary>
    /// Gets the Int32 value of specified property key.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem2-getint32"></a></remarks>
    HRESULT GetInt32(ref PropertyKey key, out int i);

    /// <summary>
    /// Gets the string value of a specified property key.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem2-getstring"></a></remarks>
    HRESULT GetString(ref PropertyKey key, [Out, MarshalAs(UnmanagedType.LPWStr)] string str);

    /// <summary>
    /// Gets the UInt32 value of a specified property key.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem2-getuint32"></a></remarks>
    HRESULT GetUInt32(ref PropertyKey key, out uint ui);

    /// <summary>
    /// Gets the UInt64 value of a specified property key.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem2-getuint64"></a></remarks>
    HRESULT GetUInt64(ref PropertyKey key, out ulong ull);

    /// <summary>
    /// Gets the boolean value of a specified property key.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem2-getbool"></a></remarks>
    HRESULT GetBool(ref PropertyKey key, [Out, MarshalAs(UnmanagedType.Bool)] bool f);
}
