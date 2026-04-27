namespace WindowsInterop.ModernApp;

using System.Runtime.InteropServices;

[Guid("03FAF64D-F26F-4B2C-AAF7-8FE7789B8BCA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IAppxManifestProperties
{
    [PreserveSig]
    int GetBoolValue([MarshalAs(UnmanagedType.LPWStr)] string name, out bool value);
    [PreserveSig]
    int GetStringValue([MarshalAs(UnmanagedType.LPWStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] out string vaue);
}
