namespace WindowsInterop.ModernApp;

using System.Runtime.InteropServices;

[Guid("4E1BD148-55A0-4480-A3D1-15544710637C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IAppxManifestReader
{
    void _VtblGap0_1(); // skip 1 method
    IAppxManifestProperties GetProperties();
    void _VtblGap1_5(); // skip 5 methods
    IAppxManifestApplicationsEnumerator GetApplications();
}
