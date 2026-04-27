namespace WindowsInterop.Win32;

using System.Runtime.InteropServices;

/// <summary>
/// Exposes a method to return either icons or thumbnails for Shell items.
/// If no thumbnail or icon is available for the requested item, a per-class icon may be provided from the Shell.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-ishellitemimagefactory"></a></remarks>
[Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IShellItemImageFactory
{
    /// <summary>
    /// Gets an HBITMAP that represents an IShellItem. The default behavior is to load a thumbnail.
    /// If there is no thumbnail for the current IShellItem, it retrieves an HBITMAP for the icon of the item.
    /// The thumbnail or icon is extracted if it is not currently cached.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitemimagefactory-getimage"></a></remarks>
    HRESULT GetImage(SIZE size, SIIGBF flags, out IntPtr hBitmap);
}
