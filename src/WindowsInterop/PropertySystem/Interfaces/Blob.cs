namespace WindowsInterop.PropertySystem;

/// <summary>
/// Representation of binary large object container.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/propidl/ns-propidl-propvariant"></a></remarks>
public readonly struct Blob
{
    /// <summary>
    /// Length of binary object.
    /// </summary>
    public readonly int Length;

    /// <summary>
    /// Pointer to buffer storing data.
    /// </summary>
    public readonly IntPtr Data;

    public Blob(int length, IntPtr data)
    {
        this.Length = length;
        this.Data = data;
    }
}
