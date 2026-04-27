namespace WindowsInterop.PropertySystem;

public readonly struct PropertyStoreKeyValue
{
    public readonly PropertyKey Key;
    public readonly PropVariant Value;

    public PropertyStoreKeyValue(PropertyKey key, PropVariant value)
    {
        this.Key = key;
        this.Value = value;
    }
}
