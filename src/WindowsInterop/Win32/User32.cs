namespace WindowsInterop.Win32
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class User32
    {
        public const int ICON_BIG = 1;
        public const int ICON_SMALL = 0;
        public const int ICON_SMALL2 = 2;
        public const int WM_GETICON = 0x7F;

        public enum CLASS_LONG_INDEX : int
        {
            GCL_HICONSM = -34,
            GCL_HICON = -14
        }

        public enum LoadImageFlags : uint
        {
            LR_DEFAULTCOLOR = 0x00000000,
            LR_SHARED = 0x00008000
        }

        public enum IconCursorVersion : int
        {
            Default = 0x00030000
        }

        /// <summary>
        /// Enumerates all top-level windows on the screen by passing the handle to each window,
        /// in turn, to an application-defined callback function.
        /// </summary>
        /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-enumwindows"></a></remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EnumWindows(EnumProc enumFunc, IntPtr param);
        public delegate bool EnumProc(IntPtr hWnd, IntPtr param);

        /// <summary>
        /// Enumerates the child windows that belong to the specified parent window by passing the handle
        /// to each child window, in turn, to an application-defined callback function.
        /// </summary>
        /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-enumchildwindows"></a></remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildProc enumFunc, IntPtr param);
        public delegate bool EnumChildProc(IntPtr hWnd, IntPtr param);

        /// <summary>
        /// Retrieves a handle to the foreground window (the window with which the user is currently working).
        /// </summary>
        /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getforegroundwindow"></a></remarks>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Copies the text of the specified window's title bar (if it has one) into a buffer.
        /// If the specified window is a control, the text of the control is copied. However,
        /// GetWindowText cannot retrieve the text of a control in another application.
        /// </summary>
        /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowtextw"></a></remarks>
        [DllImport("user32.dll", EntryPoint = "GetWindowTextW", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// Retrieves the length, in characters, of the specified window's title bar text (if the window has a title bar).
        /// If the specified window is a control, the function retrieves the length of the text within the control.
        /// However, GetWindowTextLength cannot retrieve the length of the text of an edit control in another application.
        /// </summary>
        /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowtextlengthw"></a></remarks>
        [DllImport("user32.dll", EntryPoint = "GetWindowTextLengthW", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// </summary>
        /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage"></a></remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        /// <summary>
        /// Retrieves the specified 32-bit (DWORD) value from the WNDCLASSEX structure associated with the specified window.
        /// </summary>
        /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getclasslongw"></a></remarks>
        [DllImport("user32.dll", EntryPoint = "GetClassLongW", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetClassLong(IntPtr hWnd, CLASS_LONG_INDEX index);

        /// <summary>
        /// Retrieves the specified value from the WNDCLASSEX structure associated with the specified window.
        /// </summary>
        /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getclasslongptrw"></a></remarks>
        [DllImport("user32.dll", EntryPoint = "GetClassLongPtrW", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetClassLongPtr(IntPtr hWnd, CLASS_LONG_INDEX index);

        /// <summary>
        /// Destroys an icon and frees any memory the icon occupied.
        /// </summary>
        /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-destroyicon"></a></remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        /// <summary>
        /// Retrieves the identifier of the thread that created the specified window and, optionally,
        /// the identifier of the process that created the window.
        /// </summary>
        /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowthreadprocessid"></a></remarks>
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);

        /// <summary>
        /// Searches through icon (RT_GROUP_ICON) or cursor (RT_GROUP_CURSOR) resource data
        /// for the icon or cursor that best fits the current display device.
        /// </summary>
        /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-lookupiconidfromdirectoryex"></a></remarks>
        [DllImport("user32.dll", PreserveSig = true)]
        public static extern int LookupIconIdFromDirectoryEx(IntPtr presbits, [MarshalAs(UnmanagedType.Bool)] bool fIcon, int cxDesired, int cyDesired, LoadImageFlags Flags);

        /// <summary>
        /// Creates an icon or cursor from resource bits describing the icon.
        /// </summary>
        /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-createiconfromresourceex"></a></remarks>
        [DllImport("user32.dll", PreserveSig = true, SetLastError = true)]
        public static extern IntPtr CreateIconFromResourceEx(IntPtr presbits, int dwResSize, [MarshalAs(UnmanagedType.Bool)] bool fIcon, IconCursorVersion dwVer, int cxDesired, int cyDesired, LoadImageFlags Flags);

        /// <summary>
        /// Returns the dots per inch (dpi) value for the specified window.
        /// </summary>
        /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdpiforwindow"></a></remarks>
        [DllImport("user32.dll", PreserveSig = true)]
        public static extern uint GetDpiForWindow(IntPtr hWnd);

        /// <summary>
        /// Returns the system DPI.
        /// </summary>
        /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdpiforsystem"></a></remarks>
        [DllImport("user32.dll", PreserveSig = true)]
        public static extern uint GetDpiForSystem(IntPtr hWnd);

        /// <summary>
        /// Retrieves the system DPI associated with a given process.
        /// This is useful for avoiding compatibility issues that arise from sharing DPI-sensitive information
        /// between multiple system-aware processes with different system DPI values.
        /// </summary>
        /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getsystemdpiforprocess"></a></remarks>
        [DllImport("user32.dll", PreserveSig = true)]
        public static extern uint GetSystemDpiForProcess(IntPtr hWnd);
    }
}
