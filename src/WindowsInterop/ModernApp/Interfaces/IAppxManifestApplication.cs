namespace WindowsInterop.ModernApp;

using System.Runtime.InteropServices;

[Guid("5DA89BF4-3773-46BE-B650-7E744863B7E8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IAppxManifestApplication
{
    [PreserveSig]
    int GetStringValue([MarshalAs(UnmanagedType.LPWStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] out string vaue);
}
