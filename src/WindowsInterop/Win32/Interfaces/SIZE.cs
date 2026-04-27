namespace WindowsInterop.Win32;

using System.Runtime.InteropServices;

/// <summary>
/// The SIZE structure defines the width and height of a rectangle.
/// </summary>
/// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/windef/ns-windef-size"></a></remarks>
[StructLayout(LayoutKind.Sequential)]
public struct SIZE
{
    public int cx;
    public int cy;
}
