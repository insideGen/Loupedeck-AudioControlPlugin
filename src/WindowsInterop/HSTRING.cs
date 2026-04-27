namespace WindowsInterop;

using System.Runtime.InteropServices;
using System.Security;

using WindowsInterop.Win32;

[StructLayout(LayoutKind.Sequential), SecuritySafeCritical]
public readonly struct HSTRING : IEquatable<HSTRING>, IDisposable
{
    readonly IntPtr handle;

    public static HSTRING FromString(string str) => new HSTRING(WindowsRuntimeString.FromManaged(str));

    public static implicit operator IntPtr(HSTRING h)
    {
        return h.handle;
    }

    public static implicit operator string(HSTRING h)
    {
        return h.ToString();
    }

    private HSTRING(IntPtr handle)
    {
        this.handle = handle;
    }

    public static HSTRING Cast(IntPtr h)
    {
        return new HSTRING(h);
    }

    public void Delete()
    {
        WindowsRuntimeString.DisposeAbi(this.handle);
    }

    public override string ToString()
    {
        return WindowsRuntimeString.FromAbi(this.handle);
    }

    public void Dispose()
    {
        this.Delete();
    }

    public bool Equals(HSTRING other)
    {
        return this.handle == other.handle;
    }

    public override bool Equals(object obj)
    {
        return obj is HSTRING other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return this.handle.GetHashCode();
    }
}
