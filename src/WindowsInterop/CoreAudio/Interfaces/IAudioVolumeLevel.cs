namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

/// <summary>
/// The IAudioVolumeLevel interface provides access to a hardware volume control.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/devicetopology/nn-devicetopology-iaudiovolumelevel"></a></remarks>
[Guid("7FB7B48F-531D-44A2-BCB3-5AD5A134B3DC")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IAudioVolumeLevel : IPerChannelDbLevel
{
}
