namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

public sealed class AudioSes
{
    [DllImport("AudioSes.dll", PreserveSig = false)]
    public static extern void DllGetActivationFactory([In] HSTRING iid, [Out] out IntPtr factory);
}
