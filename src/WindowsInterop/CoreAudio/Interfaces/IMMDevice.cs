namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

using WindowsInterop.PropertySystem;

/// <summary>
/// The IMMDevice interface encapsulates the generic features of a multimedia device resource.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nn-mmdeviceapi-immdevice"></a></remarks>
[Guid("D666063F-1587-4E43-81F1-B948E807363F")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IMMDevice
{
    /// <summary>
    /// The Activate method creates a COM object with the specified interface.
    /// </summary>
    /// <remarks><a href="https://docs.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immdevice-activate"></a></remarks>
    int Activate(ref Guid id, ClsCtx clsCtx, IntPtr activationParams, [MarshalAs(UnmanagedType.IUnknown)] out object interfacePointer);

    /// <summary>
    /// The OpenPropertyStore method retrieves an interface to the device's property store.
    /// </summary>
    /// <remarks><a href="https://docs.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immdevice-openpropertystore"></a></remarks>
    int OpenPropertyStore(StorageAccessMode stgmAccess, out IPropertyStore properties);

    /// <summary>
    /// The GetId method retrieves an endpoint ID string that identifies the audio endpoint device.
    /// </summary>
    /// <remarks><a href="https://docs.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immdevice-getid"></a></remarks>
    int GetId([MarshalAs(UnmanagedType.LPWStr)] out string id);

    /// <summary>
    /// The GetState method retrieves the current device state.
    /// </summary>
    /// <remarks><a href="https://docs.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immdevice-getstate"></a></remarks>
    int GetState(out DeviceState state);
}
