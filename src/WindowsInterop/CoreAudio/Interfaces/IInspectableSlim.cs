namespace WindowsInterop.CoreAudio.Interfaces;

using System.Runtime.InteropServices;

[Guid("00000000-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IInspectableSlim
{
    [PreserveSig]
    HRESULT GetIids(IntPtr count, ref IntPtr iids);
}
