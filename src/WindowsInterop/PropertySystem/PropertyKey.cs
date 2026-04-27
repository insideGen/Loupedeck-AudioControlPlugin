namespace WindowsInterop.PropertySystem;

using System.Runtime.InteropServices;

/// <summary>
/// Specifies the FMTID/PID identifier that programmatically identifies a property.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/wtypes/ns-wtypes-propertykey"></a></remarks>
[StructLayout(LayoutKind.Sequential)]
public struct PropertyKey : IEquatable<PropertyKey>
{
    public Guid FormatId;
    public UIntPtr PropertyId;

    public PropertyKey(Guid formatId, UIntPtr propertyId)
    {
        this.FormatId = formatId;
        this.PropertyId = propertyId;
    }

    public bool Equals(PropertyKey other)
    {
        bool equals = true;
        equals &= this.FormatId == other.FormatId;
        equals &= this.PropertyId == other.PropertyId;
        return equals;
    }

    public override bool Equals(object obj) => obj is PropertyKey other && this.Equals(other);

    public override int GetHashCode() => (this.FormatId, this.PropertyId).GetHashCode();

    public static bool operator ==(PropertyKey left, PropertyKey right) => left.Equals(right);

    public static bool operator !=(PropertyKey left, PropertyKey right) => !left.Equals(right);
}
