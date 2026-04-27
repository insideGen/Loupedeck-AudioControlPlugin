namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

[Guid("F8679F50-850A-41CF-9C72-430F290290C8")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IPolicyConfig
{
    [PreserveSig]
    int GetMixFormat(string deviceId, IntPtr format);

    [PreserveSig]
    int GetDeviceFormat(string deviceId, bool _default, IntPtr format);

    [PreserveSig]
    int ResetDeviceFormat(string deviceId);

    [PreserveSig]
    int SetDeviceFormat(string deviceId, IntPtr endpointFormat, IntPtr mixFormat);

    [PreserveSig]
    int GetProcessingPeriod(string deviceId, bool _default, IntPtr defaultPeriod, IntPtr minimumPeriod);

    [PreserveSig]
    int SetProcessingPeriod(string deviceId, IntPtr period);

    [PreserveSig]
    int GetShareMode(string deviceId, IntPtr mode);

    [PreserveSig]
    int SetShareMode(string deviceId, IntPtr mode);

    [PreserveSig]
    int GetPropertyValue(string deviceId, bool store, IntPtr key, IntPtr value);

    [PreserveSig]
    int SetPropertyValue(string deviceId, bool store, IntPtr key, IntPtr value);

    [PreserveSig]
    int SetDefaultEndpoint(string deviceId, Role role);

    [PreserveSig]
    int SetEndpointVisibility(string deviceId, bool visible);
}
