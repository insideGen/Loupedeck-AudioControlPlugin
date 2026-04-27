namespace WindowsInterop.Win32;

using System.Runtime.InteropServices;

/// <summary>
/// Exposes methods that retrieve information about a Shell item.
/// IShellItem and IShellItem2 are the preferred representations of items in any new code.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-ishellitem"></a></remarks>
[Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IShellItem
{
    /// <summary>
    /// Binds to a handler for an item as specified by the handler ID value (BHID).
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem-bindtohandler"></a></remarks>
    HRESULT BindToHandler([In, MarshalAs(UnmanagedType.Interface)] IntPtr pbc, [In] ref Guid bhid, [In] ref Guid riid, out IntPtr ppv);

    /// <summary>
    /// Gets the parent of an IShellItem object.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem-getparent"></a></remarks>
    HRESULT GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

    /// <summary>
    /// Gets the display name of the IShellItem object.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem-getdisplayname"></a></remarks>
    HRESULT GetDisplayName([In] SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

    /// <summary>
    /// Gets a requested set of attributes of the IShellItem object.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem-getattributes"></a></remarks>
    HRESULT GetAttributes([In] uint sfgaoMask, out uint psfgaoAttribs);

    /// <summary>
    /// Compares two IShellItem objects.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitem-compare"></a></remarks>
    HRESULT Compare([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In] uint hint, out int piOrder);
}
