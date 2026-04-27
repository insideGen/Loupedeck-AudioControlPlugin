namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IMMDeviceEnumerator interface provides methods for enumerating multimedia device resources.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nn-mmdeviceapi-immdeviceenumerator"></a></remarks>
[Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IMMDeviceEnumerator
{
    /// <summary>
    /// The EnumAudioEndpoints method generates a collection of audio endpoint devices that meet the specified criteria.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immdeviceenumerator-enumaudioendpoints"></a></remarks>
    int EnumAudioEndpoints(DataFlow dataFlow, DeviceState stateMask, out IMMDeviceCollection devices);

    /// <summary>
    /// The GetDefaultAudioEndpoint method retrieves the default audio endpoint for the specified data-flow direction and role.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immdeviceenumerator-getdefaultaudioendpoint"></a></remarks>
    [PreserveSig]
    int GetDefaultAudioEndpoint(DataFlow dataFlow, Role role, out IMMDevice endpoint);

    /// <summary>
    /// The GetDevice method retrieves an audio endpoint device that is identified by an endpoint ID string.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immdeviceenumerator-getdevice"></a></remarks>
    int GetDevice(string id, out IMMDevice device);

    /// <summary>
    /// The RegisterEndpointNotificationCallback method registers a client's notification callback interface.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immdeviceenumerator-registerendpointnotificationcallback"></a></remarks>
    int RegisterEndpointNotificationCallback(IMMNotificationClient client);

    /// <summary>
    /// The UnregisterEndpointNotificationCallback method deletes the registration of a notification interface
    /// that the client registered in a previous call to the IMMDeviceEnumerator::RegisterEndpointNotificationCallback method.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immdeviceenumerator-unregisterendpointnotificationcallback"></a></remarks>
    int UnregisterEndpointNotificationCallback(IMMNotificationClient client);
}
