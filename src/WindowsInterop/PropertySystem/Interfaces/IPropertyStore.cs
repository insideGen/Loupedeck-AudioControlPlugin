namespace WindowsInterop.PropertySystem;

using System.Runtime.InteropServices;

/// <summary>
/// This interface exposes methods used to enumerate and manipulate property values.
/// </summary>
/// <remarks><a href="https://docs.microsoft.com/en-us/windows/win32/api/propsys/nn-propsys-ipropertystore"></a></remarks>
[Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IPropertyStore
{
    /// <summary>
    /// This method returns a count of the number of properties that are attached to the file.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/propsys/nf-propsys-ipropertystore-getcount"></a></remarks>
    int GetCount(out int count);

    /// <summary>
    /// Gets a property key from the property array of an item.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/propsys/nf-propsys-ipropertystore-getat"></a></remarks>
    int GetAt(int index, out PropertyKey key);

    /// <summary>
    /// This method retrieves the data for a specific property.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/propsys/nf-propsys-ipropertystore-getvalue"></a></remarks>
    int GetValue(ref PropertyKey key, out PropVariant value);

    /// <summary>
    /// This method sets a property value or replaces or removes an existing value.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/propsys/nf-propsys-ipropertystore-setvalue"></a></remarks>
    int SetValue(ref PropertyKey key, ref PropVariant value);

    /// <summary>
    /// After a change has been made, this method saves the changes.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/propsys/nf-propsys-ipropertystore-commit"></a></remarks>
    int Commit();
}
