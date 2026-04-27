namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IMMEndpoint interface represents an audio endpoint device.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nn-mmdeviceapi-immendpoint"></a></remarks>
[Guid("1BE09788-6894-4089-8586-9A2A6C265AC5")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IMMEndpoint
{
    /// <summary>
    /// The GetDataFlow method indicates whether the audio endpoint device is a rendering device or a capture device.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/mmdeviceapi/nf-mmdeviceapi-immendpoint-getdataflow"></a></remarks>
    int GetDataFlow(out DataFlow dataFlow);
}
