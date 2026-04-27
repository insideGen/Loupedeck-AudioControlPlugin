namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IPartsList interface represents a list of parts, each of which is an object with an IPart interface
/// that represents a connector or subunit.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nn-devicetopology-ipartslist"></a></remarks>
[Guid("6DAA848C-5EB0-45CC-AEA5-998A2CDA1FFB")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IPartsList
{
    /// <summary>
    /// The GetCount method gets the number of parts in the parts list.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-ipartslist-getcount"></a></remarks>
    int GetCount(out uint count);

    /// <summary>
    /// The GetPart method gets a part from the parts list.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-ipartslist-getpart"></a></remarks>
    int GetPart(uint index, out IPart part);
}
