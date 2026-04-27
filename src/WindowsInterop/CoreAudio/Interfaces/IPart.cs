namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IPart interface represents a part (connector or subunit) of a device topology.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nn-devicetopology-ipart"></a></remarks>
[Guid("AE2DE0E4-5BCA-4F2D-AA46-5D13F8FDB3A9")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IPart
{
    /// <summary>
    /// The GetName method gets the friendly name of this part.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-ipart-getname"></a></remarks>
    int GetName([Out, MarshalAs(UnmanagedType.LPWStr)] out string name);

    /// <summary>
    /// The GetLocalId method gets the local ID of this part.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-ipart-getlocalid"></a></remarks>
    int GetLocalId([Out] out uint id);

    /// <summary>
    /// The GetGlobalId method gets the global ID of this part.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-ipart-getglobalid"></a></remarks>
    int GetGlobalId([Out, MarshalAs(UnmanagedType.LPWStr)] out string id);

    /// <summary>
    /// The GetPartType method gets the part type of this part.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-ipart-getparttype"></a></remarks>
    int GetPartType([Out] out PartTypeEnum partType);

    /// <summary>
    /// The GetSubType method gets the part subtype of this part.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-ipart-getsubtype"></a></remarks>
    int GetSubType(out Guid subType);

    /// <summary>
    /// The GetControlInterfaceCount method gets the number of control interfaces that this part supports.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-ipart-getcontrolinterfacecount"></a></remarks>
    int GetControlInterfaceCount([Out] out uint count);

    /// <summary>
    /// The GetControlInterface method gets a reference to the specified control interface, if this part supports it.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-ipart-getcontrolinterface"></a></remarks>
    int GetControlInterface([In] uint index, [Out, MarshalAs(UnmanagedType.IUnknown)] out IControlInterface controlInterface);

    /// <summary>
    /// The EnumPartsIncoming method gets a list of all the incoming parts—that is,
    /// the parts that reside on data paths that are upstream from this part.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-ipart-enumpartsincoming"></a></remarks>
    [PreserveSig]
    int EnumPartsIncoming([Out] out IPartsList parts);

    /// <summary>
    /// The EnumPartsOutgoing method retrieves a list of all the outgoing parts—that is,
    /// the parts that reside on data paths that are downstream from this part.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-ipart-enumpartsoutgoing"></a></remarks>
    [PreserveSig]
    int EnumPartsOutgoing([Out] out IPartsList parts);

    /// <summary>
    /// The GetTopologyObject method gets a reference to the IDeviceTopology interface
    /// of the device-topology object that contains this part.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-ipart-gettopologyobject"></a></remarks>
    int GetTopologyObject([Out] out object topologyObject);

    /// <summary>
    /// The Activate method activates a function-specific interface on a connector or subunit.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-ipart-activate"></a></remarks>
    [PreserveSig]
    int Activate([In] ClsCtx dwClsContext, [In] ref Guid refiid, [MarshalAs(UnmanagedType.IUnknown)] out object interfacePointer);

    /// <summary>
    /// The RegisterControlChangeCallback method registers the IControlChangeNotify interface,
    /// which the client implements to receive notifications of status changes in this part.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-ipart-registercontrolchangecallback"></a></remarks>
    int RegisterControlChangeCallback([In] ref Guid refiid, [In] IControlChangeNotify notify);

    /// <summary>
    /// The UnregisterControlChangeCallback method removes the registration of an IControlChangeNotify interface
    /// that the client previously registered by a call to the IPart::RegisterControlChangeCallback method.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nf-devicetopology-ipart-unregistercontrolchangecallback"></a></remarks>
    int UnregisterControlChangeCallback([In] IControlChangeNotify notify);
}
