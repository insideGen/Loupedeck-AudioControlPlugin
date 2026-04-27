namespace WindowsInterop.Win32;

using System.Runtime.InteropServices;

public class Ntdll
{
    public enum SYSTEM_INFORMATION_CLASS
    {
        SystemProcessInformation = 0x0005
    }

    public enum NTSTATUS : uint
    {
        SUCCESS = 0x0,
        STATUS_INFO_LENGTH_MISMATCH = 0xC0000004
    }

    /// <summary>
    /// Retrieves the specified system information.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winternl/nf-winternl-ntquerysysteminformation"></a></remarks>
    [DllImport("ntdll.dll", PreserveSig = true, EntryPoint = "NtQuerySystemInformation")]
    public static extern NTSTATUS NtQuerySystemInformationInitial(SYSTEM_INFORMATION_CLASS infoClass, IntPtr info, int size, out int length);

    /// <summary>
    /// Retrieves the specified system information.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winternl/nf-winternl-ntquerysysteminformation"></a></remarks>
    [DllImport("ntdll.dll", PreserveSig = true)]
    public static extern NTSTATUS NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS infoClass, IntPtr info, int size, IntPtr length);
}
