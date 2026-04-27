namespace WindowsInterop.Win32;

using System.Runtime.InteropServices;

public class Gdi32
{
    /// <summary>
    /// The DeleteObject function deletes a logical pen, brush, font, bitmap, region, or palette,
    /// freeing all system resources associated with the object. After the object is deleted,
    /// the specified handle is no longer valid.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/wingdi/nf-wingdi-deleteobject"></a></remarks>
    [DllImport("gdi32.dll", PreserveSig = true)]
    public static extern bool DeleteObject(IntPtr objectHandle);
}
