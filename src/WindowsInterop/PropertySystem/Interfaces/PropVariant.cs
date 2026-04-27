namespace WindowsInterop.PropertySystem;

using System.Runtime.InteropServices;

/// <summary>
/// The PROPVARIANT structure is used in the ReadMultiple and WriteMultiple methods of
/// IPropertyStorage to define the type tag and the value of a property in a property set.
/// The PROPVARIANT structure is also used by the GetValue and SetValue methods of IPropertyStore,
/// which replaces IPropertySetStorage as the primary way to program item properties in Windows Vista.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/propidl/ns-propidl-propvariant"></a></remarks>
[StructLayout(LayoutKind.Explicit)]
public struct PropVariant
{
    [FieldOffset(0)] public VarEnum varType;
    [FieldOffset(2)] public ushort wReserved1;
    [FieldOffset(4)] public ushort wReserved2;
    [FieldOffset(6)] public ushort wReserved3;
    [FieldOffset(8)] public sbyte cVal;
    [FieldOffset(8)] public byte bVal;
    [FieldOffset(8)] public short iVal;
    [FieldOffset(8)] public ushort uiVal;
    [FieldOffset(8)] public long lVal;
    [FieldOffset(8)] public ulong ulVal;
    [FieldOffset(8)] public int intVal;
    [FieldOffset(8)] public uint uintVal;
    [FieldOffset(8)] public long hVal;
    [FieldOffset(8)] public long uhVal;
    [FieldOffset(8)] public float fltVal;
    [FieldOffset(8)] public double dblVal;
    [FieldOffset(8)] public short boolVal;
    [FieldOffset(8)] public int scode;
    [FieldOffset(8)] public System.Runtime.InteropServices.ComTypes.FILETIME filetime;
    [FieldOffset(8)] public Blob blobVal;
    [FieldOffset(8)] public IntPtr pszVal;
    [FieldOffset(8)] public IntPtr pwszVal;
    [FieldOffset(8)] public IntPtr punkVal;
    [FieldOffset(8)] public PropArray ca;

    [DllImport("api-ms-win-core-com-l1-1-1.dll")]
    public static extern int PropVariantClear(ref PropVariant propVar);

    [DllImport("api-ms-win-core-com-l1-1-1.dll")]
    public static extern int PropVariantClear(IntPtr propVar);
}
