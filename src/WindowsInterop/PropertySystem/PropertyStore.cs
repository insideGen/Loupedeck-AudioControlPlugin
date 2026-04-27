namespace WindowsInterop.PropertySystem;

using System.Runtime.InteropServices;

public class PropertyStore : IDisposable
{
    private readonly IPropertyStore propertyStoreComObj;

    public int Count
    {
        get
        {
            Marshal.ThrowExceptionForHR(this.propertyStoreComObj.GetCount(out int propCount));
            return propCount;
        }
    }

    public PropertyStoreKeyValue this[int index]
    {
        get
        {
            PropertyKey key = this.Get(index);
            Marshal.ThrowExceptionForHR(this.propertyStoreComObj.GetValue(ref key, out PropVariant value));
            return new PropertyStoreKeyValue(key, value);
        }
    }

    public PropertyStoreKeyValue? this[PropertyKey key]
    {
        get
        {
            for (int i = 0; i < this.Count; i++)
            {
                PropertyKey iKey = this.Get(i);
                if ((iKey.FormatId == key.FormatId) && (iKey.PropertyId == key.PropertyId))
                {
                    Marshal.ThrowExceptionForHR(this.propertyStoreComObj.GetValue(ref iKey, out PropVariant value));
                    return new PropertyStoreKeyValue(iKey, value);
                }
            }
            return null;
        }
    }

    internal PropertyStore(IPropertyStore propertyStoreComObj)
    {
        this.propertyStoreComObj = propertyStoreComObj;
    }

    public PropertyKey Get(int index)
    {
        Marshal.ThrowExceptionForHR(this.propertyStoreComObj.GetAt(index, out PropertyKey key));
        return key;
    }

    public bool Contains(PropertyKey key)
    {
        for (int i = 0; i < this.Count; i++)
        {
            PropertyKey iKey = this.Get(i);
            if (iKey == key)
            {
                return true;
            }
        }
        return false;
    }

    public PropVariant GetValue(int index)
    {
        PropertyKey key = this.Get(index);
        Marshal.ThrowExceptionForHR(this.propertyStoreComObj.GetValue(ref key, out PropVariant value));
        return value;
    }

    public T GetValue<T>(PropertyKey key)
    {
        PropVariant propVar = default;
        try
        {
            Marshal.ThrowExceptionForHR(this.propertyStoreComObj.GetValue(ref key, out propVar));
            switch (propVar.varType)
            {
                case VarEnum.VT_LPWSTR:
                    return (T)Convert.ChangeType(Marshal.PtrToStringUni(propVar.pwszVal), typeof(T));
                case VarEnum.VT_EMPTY:
                    return default;
                default:
                    throw new NotImplementedException();
            }
        }
        finally
        {
            PropVariant.PropVariantClear(ref propVar);
        }
    }

    public void SetValue(PropertyKey key, PropVariant value)
    {
        Marshal.ThrowExceptionForHR(this.propertyStoreComObj.SetValue(ref key, ref value));
    }

    public void Commit()
    {
        Marshal.ThrowExceptionForHR(this.propertyStoreComObj.Commit());
    }

    public void Dispose()
    {
        Marshal.ReleaseComObject(this.propertyStoreComObj);
        GC.SuppressFinalize(this);
    }

    ~PropertyStore()
    {
        this.Dispose();
    }
}
