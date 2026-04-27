namespace WindowsInterop;

using Microsoft.Win32.SafeHandles;

public abstract class HMODULE : SafeHandleZeroOrMinusOneIsInvalid
{
    public HMODULE() : base(ownsHandle: true)
    {
    }

    protected override bool ReleaseHandle()
    {
        return Win32.Kernel32.FreeLibrary(base.handle);
    }
}
