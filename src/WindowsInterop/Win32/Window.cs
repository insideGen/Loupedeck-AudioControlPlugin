namespace WindowsInterop.Win32;

using System.Drawing;
using System.Text;

public class Window : IDisposable
{
    private int _processId;
    private string _fullProcessName = null;
    private string _title = null;
    private Icon _icon = null;

    public Guid Guid { get; }

    public IntPtr Handle { get; }

    public int ProcessId
    {
        get
        {
            return this._processId;
        }
    }

    public string FullProcessName
    {
        get
        {
            if (this._fullProcessName == null)
            {
                this.GetFullProcessName();
            }
            return this._fullProcessName;
        }
    }

    public string Title
    {
        get
        {
            if (this._title == null)
            {
                this.GetTitle();
            }
            return this._title;
        }
    }

    public Icon Icon
    {
        get
        {
            if (this._icon == null)
            {
                this.GetIcon();
            }
            return this._icon;
        }
    }

    public Window(IntPtr handle)
    {
        this.Guid = Guid.NewGuid();
        this.Handle = handle;
        this.GetProcessId();
    }

    private int GetProcessId()
    {
        User32.GetWindowThreadProcessId(this.Handle, out this._processId);
        return this._processId;
    }

    private string GetFullProcessName()
    {
        this._fullProcessName = "";
        int capacity = 2000;
        StringBuilder builder = new StringBuilder(capacity);
        IntPtr hProcess = Kernel32.OpenProcess(Kernel32.ProcessFlags.PROCESS_QUERY_LIMITED_INFORMATION, false, this.ProcessId);
        if (hProcess != IntPtr.Zero)
        {
            try
            {
                if (Kernel32.QueryFullProcessImageName(hProcess, 0, builder, ref capacity) != 0)
                {
                    this._fullProcessName = builder.ToString();
                }
            }
            finally
            {
                Kernel32.CloseHandle(hProcess);
            }
        }
        return this._fullProcessName;
    }

    private string GetTitle()
    {
        this._title = "";
        int length = User32.GetWindowTextLength(this.Handle) + 1;
        StringBuilder title = new StringBuilder(length);
        User32.GetWindowText(this.Handle, title, length);
        this._title = title.ToString();
        return this._title;
    }

    private Icon GetIcon()
    {
        IntPtr iconHandle = User32.GetClassLongPtr(this.Handle, User32.CLASS_LONG_INDEX.GCL_HICON);
        if (iconHandle == IntPtr.Zero)
        {
            iconHandle = User32.SendMessage(this.Handle, User32.WM_GETICON, User32.ICON_BIG, 0);
        }
        if (iconHandle != IntPtr.Zero)
        {
            try
            {
                if (this._icon != null)
                {
                    this._icon.Dispose();
                }
                this._icon = Icon.FromHandle(iconHandle);
            }
            finally
            {
                User32.DestroyIcon(iconHandle);
            }
        }
        return this._icon;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        this._icon?.Dispose();
    }

    ~Window()
    {
        this.Dispose();
    }
}
