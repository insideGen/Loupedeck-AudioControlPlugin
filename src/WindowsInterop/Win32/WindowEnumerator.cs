namespace WindowsInterop.Win32;

using System.Collections.Generic;

public class WindowEnumerator
{
    private readonly List<IntPtr> _handles;

    public WindowEnumerator()
    {
        this._handles = new List<IntPtr>();
    }

    private bool EnumWindowCallback(IntPtr hWnd, IntPtr param)
    {
        this._handles.Add(hWnd);
        return true;
    }

    public IntPtr[] GetHandles(IntPtr hWndParent)
    {
        this._handles.Clear();
        if (hWndParent == IntPtr.Zero)
        {
            User32.EnumWindows(this.EnumWindowCallback, IntPtr.Zero);
        }
        else
        {
            User32.EnumChildWindows(hWndParent, this.EnumWindowCallback, IntPtr.Zero);
        }
        return this._handles.ToArray();
    }

    public static bool TryGetForegroundProcessId(out int processId)
    {
        bool result = false;
        processId = -1;
        IntPtr hFgWnd = User32.GetForegroundWindow();
        if (hFgWnd != IntPtr.Zero)
        {
            using (Window window = new Window(hFgWnd))
            {
                if (window.FullProcessName.Contains("ApplicationFrameHost"))
                {
                    WindowEnumerator windowEnumerator = new WindowEnumerator();
                    IntPtr[] hChildWindows = windowEnumerator.GetHandles(hFgWnd);
                    foreach (IntPtr hChildWnd in hChildWindows)
                    {
                        using (Window chilWnd = new Window(hChildWnd))
                        {
                            if (!chilWnd.FullProcessName.Contains("ApplicationFrameHost"))
                            {
                                processId = chilWnd.ProcessId;
                                result = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    processId = window.ProcessId;
                    result = true;
                }
            }
        }
        return result;
    }
}
