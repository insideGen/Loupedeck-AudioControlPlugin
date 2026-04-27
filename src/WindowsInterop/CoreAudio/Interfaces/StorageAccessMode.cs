namespace WindowsInterop.CoreAudio;

/// <summary>
/// The STGM constants are flags that indicate conditions for creating and deleting the object and access modes for the object.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/stg/stgm-constants"></a></remarks>
public enum StorageAccessMode
{
    /// <summary>
    /// Indicates that the object is read-only, meaning that modifications cannot be made.
    /// </summary>
    Read = 0,
    /// <summary>
    /// Enables you to save changes to the object, but does not permit access to its data.
    /// </summary>
    Write = 1,
    /// <summary>
    /// Enables access and modification of object data.
    /// </summary>
    ReadWrite = 2
}
