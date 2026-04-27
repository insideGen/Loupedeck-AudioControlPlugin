namespace WindowsInterop.ModernApp;

using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;

[Guid("BEB94909-E451-438B-B5A7-D79E767B75D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IAppxFactory
{
    void _VtblGap0_2(); // skip 2 methods
    IAppxManifestReader CreateManifestReader(IStream inputStream);
}
