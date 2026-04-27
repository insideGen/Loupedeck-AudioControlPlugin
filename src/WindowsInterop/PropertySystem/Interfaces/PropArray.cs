namespace WindowsInterop.PropertySystem;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 0)]
public struct PropArray
{
    public uint cElems;
    public IntPtr pElems;
}
