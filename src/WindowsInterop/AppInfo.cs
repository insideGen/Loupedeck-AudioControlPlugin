namespace WindowsInterop;

using System.Diagnostics;
using System.IO;
using System.Text;

using WindowsInterop.ModernApp;
using WindowsInterop.Win32;

public class AppInfo
{
    public int ProcessId { get; private set; }
    public string ExePath { get; private set; }
    public string DisplayName { get; private set; }
    public string LogoPath { get; private set; }

    public AppInfo()
    {
    }

    public static AppInfo FromPath(string path)
    {
        AppInfo appInfo = null;
        if (!string.IsNullOrWhiteSpace(path))
        {
            string displayName = string.Empty;
            if (File.Exists(path))
            {
                displayName = FileVersionInfo.GetVersionInfo(path).ProductName;
            }
            if (string.IsNullOrEmpty(displayName))
            {
                displayName = Path.GetFileNameWithoutExtension(path);
            }
            appInfo = new AppInfo();
            appInfo.ProcessId = 0;
            appInfo.ExePath = path;
            appInfo.DisplayName = displayName;
            appInfo.LogoPath = path;
        }
        return appInfo;
    }

    public static AppInfo FromProcess(IntPtr hProcess)
    {
        AppInfo appInfo = null;
        if (hProcess != IntPtr.Zero)
        {
            int capacity = 2000;
            StringBuilder builder = new StringBuilder(capacity);
            if (Kernel32.QueryFullProcessImageName(hProcess, 0, builder, ref capacity) != 0)
            {
                appInfo = FromPath(builder.ToString());
            }
            if (AppxPackage.IsPackagedProcess(hProcess))
            {
                appInfo ??= new AppInfo();

                AppxPackage appxPackage = AppxPackage.FromProcess(hProcess);
                // Preserve exe path from QueryFullProcessImageName; fall back to package path if unavailable
                if (string.IsNullOrEmpty(appInfo.ExePath))
                {
                    appInfo.ExePath = appxPackage.Path;
                }

                string displayName = appxPackage.DisplayName;
                if (!string.IsNullOrEmpty(displayName) && displayName.StartsWith("ms-resource:"))
                {
                    displayName = AppxPackage.LoadResourceString(appxPackage.FullName, displayName);
                }
                if (!string.IsNullOrEmpty(displayName))
                {
                    appInfo.DisplayName = displayName;
                }

                string logoPath = null;

                // Find the app whose executable matches, to get its icon (like Windows does)
                string exeFileName = !string.IsNullOrEmpty(appInfo.ExePath) ? Path.GetFileName(appInfo.ExePath) : null;
                AppxApp matchingApp = appxPackage.Apps.FirstOrDefault(a => !string.IsNullOrEmpty(a.Executable) && string.Equals(Path.GetFileName(a.Executable), exeFileName, StringComparison.OrdinalIgnoreCase));
                matchingApp ??= appxPackage.Apps.FirstOrDefault();

                // Prefer Square44x44Logo (same as Windows taskbar/volume mixer), fall back to package Logo
                string relativeLogo = matchingApp?.Square44x44Logo ?? matchingApp?.Logo ?? appxPackage.Logo;

                if (!string.IsNullOrEmpty(relativeLogo) && !string.IsNullOrEmpty(appxPackage.Path))
                {
                    // Try lightunplated variant first (contains real app colors, not white-on-transparent)
                    logoPath = appxPackage.FindLightUnplatedVariant(relativeLogo) ?? appxPackage.FindHighestScaleQualifiedImagePath(relativeLogo);
                    if (string.IsNullOrEmpty(logoPath))
                    {
                        string fullLogo = Path.Combine(appxPackage.Path, relativeLogo);
                        if (File.Exists(fullLogo))
                        {
                            logoPath = fullLogo;
                        }
                    }
                }
                appInfo.LogoPath = logoPath ?? appxPackage.ApplicationUserModelId;
            }
            if (appInfo != null)
            {
                appInfo.ProcessId = Kernel32.GetProcessId(hProcess);
            }
        }
        return appInfo;
    }

    public static AppInfo FromProcess(int processId)
    {
        AppInfo appInfo = null;
        if (processId > 0)
        {
            IntPtr hProcess = Kernel32.OpenProcess(Kernel32.ProcessFlags.PROCESS_QUERY_LIMITED_INFORMATION, false, processId);
            if (hProcess != IntPtr.Zero)
            {
                try
                {
                    appInfo = FromProcess(hProcess);
                }
                finally
                {
                    Kernel32.CloseHandle(hProcess);
                }
            }
        }
        return appInfo;
    }
}
