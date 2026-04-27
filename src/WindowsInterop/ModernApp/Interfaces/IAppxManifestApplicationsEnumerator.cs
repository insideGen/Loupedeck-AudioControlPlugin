namespace WindowsInterop.ModernApp;

using System.Runtime.InteropServices;

[Guid("9EB8A55A-F04B-4D0D-808D-686185D4847A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IAppxManifestApplicationsEnumerator
{
    IAppxManifestApplication GetCurrent();
    bool GetHasCurrent();
    bool MoveNext();
}
