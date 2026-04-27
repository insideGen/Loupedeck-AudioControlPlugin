namespace WindowsInterop.Win32;

using System.IO;
using System.Text;

public static class DevicePathMapper
{
    public static string FromDevicePath(string devicePath)
    {
        if (string.IsNullOrWhiteSpace(devicePath))
        {
            return devicePath;
        }
        else
        {
            DriveInfo drive = Array.Find(DriveInfo.GetDrives(), d => devicePath.StartsWith(d.GetDevicePath(), StringComparison.InvariantCultureIgnoreCase));
            return drive != null ? devicePath.ReplaceFirst(drive.GetDevicePath(), drive.GetDriveLetter()) : null;
        }
    }

    public static string FromDriveLetter(string driveLetterPath)
    {
        if (string.IsNullOrWhiteSpace(driveLetterPath))
        {
            return driveLetterPath;
        }
        else
        {
            DriveInfo drive = Array.Find(DriveInfo.GetDrives(), d => driveLetterPath.StartsWith(d.GetDriveLetter(), StringComparison.InvariantCultureIgnoreCase));
            return drive != null ? driveLetterPath.ReplaceFirst(drive.GetDriveLetter(), drive.GetDevicePath()) : null;
        }
    }

    private static string GetDevicePath(this DriveInfo driveInfo)
    {
        StringBuilder devicePathBuilder = new StringBuilder(128);
        return Kernel32.QueryDosDevice(driveInfo.GetDriveLetter(), devicePathBuilder, devicePathBuilder.Capacity + 1) != 0 ? devicePathBuilder.ToString() : null;
    }

    private static string GetDriveLetter(this DriveInfo driveInfo)
    {
        return driveInfo.Name.Substring(0, 2);
    }

    private static string ReplaceFirst(this string text, string search, string replace)
    {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }
}
