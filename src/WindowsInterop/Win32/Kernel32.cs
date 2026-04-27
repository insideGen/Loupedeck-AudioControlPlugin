namespace WindowsInterop.Win32;

using System.Runtime.InteropServices;
using System.Text;

using WindowsInterop.ModernApp;

public class Kernel32
{
    public const int MAX_AUMID_LEN = 512;
    public const int PACKAGE_FAMILY_NAME_MAX_LENGTH_INCL_Z = 65 * SIZEOF_WCHAR;
    public const int PACKAGE_FILTER_HEAD = 0x00000010;
    public const int PACKAGE_INFORMATION_BASIC = 0x00000000;
    public const int PACKAGE_RELATIVE_APPLICATION_ID_MAX_LENGTH_INCL_Z = 65 * SIZEOF_WCHAR;
    public const int SIZEOF_WCHAR = 2;
    public const int WAIT_TIMEOUT = 0x00000102;

    public static IntPtr RT_GROUP_ICON = new IntPtr(14);
    public static IntPtr RT_ICON = new IntPtr(3);

    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-loadlibraryexw"></a></remarks>
    [Flags]
    public enum LoadLibraryFlags : int
    {
        LOAD_LIBRARY_AS_DATAFILE = 0x02,
        LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x20
    }

    /// <summary>
    /// Process Security and Access Rights.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/procthread/process-security-and-access-rights"></a></remarks>
    [Flags]
    public enum ProcessFlags : uint
    {
        PROCESS_QUERY_LIMITED_INFORMATION = 0x00001000,
        SYNCHRONIZE = 0x00100000
    }

    /// <summary>
    /// Describes possible machine architectures.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/sysinfo/image-file-machine-constants"></a></remarks>
    [Flags]
    public enum IMAGE_FILE_MACHINE : int
    {
        I386 = 0x014C,
        AMD64 = 0x8664,
        ARM64 = 0xAA64
    }

    /// <summary>
    /// Opens an existing local process object.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-openprocess"></a></remarks>
    [DllImport("kernel32.dll", PreserveSig = true)]
    public static extern IntPtr OpenProcess(ProcessFlags desiredAccess, bool inheritHandle, int processId);

    /// <summary>
    /// Closes an open object handle.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/handleapi/nf-handleapi-closehandle"></a></remarks>
    [DllImport("kernel32.dll", PreserveSig = true)]
    public static extern bool CloseHandle(IntPtr handleObject);

    /// <summary>
    /// Waits until the specified object is in the signaled state or the time-out interval elapses.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/synchapi/nf-synchapi-waitforsingleobject"></a></remarks>
    [DllImport("kernel32.dll", PreserveSig = true)]
    public static extern int WaitForSingleObject(IntPtr handle, int milliseconds);

    /// <summary>
    /// Retrieves the process identifier of the specified process.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getprocessid"></a></remarks>
    [DllImport("kernel32.dll", PreserveSig = true)]
    public static extern int GetProcessId(IntPtr handle);

    /// <summary>
    /// Gets the package identifier (ID) for the specified process.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/appmodel/nf-appmodel-getpackageid"></a></remarks>
    [DllImport("kernel32.dll", PreserveSig = true)]
    public static extern HRESULT GetPackageId(IntPtr process, ref int bufferLength, IntPtr packageId);

    /// <summary>
    /// Retrieves information about MS-DOS device names. The function can obtain the current mapping for a particular
    /// MS-DOS device name. The function can also obtain a list of all existing MS-DOS device names.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-querydosdevicea"></a></remarks>
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern int QueryDosDevice([In] string lpDeviceName, [Out] StringBuilder lpTargetPath, [In] int ucchMax);

    /// <summary>
    /// Retrieves the full name of the executable image for the specified process.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-queryfullprocessimagenamew"></a></remarks>
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
    public static extern int QueryFullProcessImageName(IntPtr process, int flags, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder exeName, ref int size);

    /// <summary>
    /// Determines whether the specified process is running under WOW64;
    /// also returns additional machine process and architecture information.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/wow64apiset/nf-wow64apiset-iswow64process2"></a></remarks>
    [DllImport("kernel32.dll", PreserveSig = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWow64Process2(IntPtr hProcess, out IMAGE_FILE_MACHINE pProcessMachine, out IMAGE_FILE_MACHINE pNativeMachine);

    /// <summary>
    /// Gets the application user model ID for the specified process.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/appmodel/nf-appmodel-getapplicationusermodelid"></a></remarks>
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
    public static extern int GetApplicationUserModelId(IntPtr hProcess, ref int applicationUserModelIdLength, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder applicationUserModelId);

    /// <summary>
    /// Loads the specified module into the address space of the calling process.
    /// The specified module may cause other modules to be loaded.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-loadlibraryexw"></a></remarks>
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
    public static extern HMODULE LoadLibraryEx([MarshalAs(UnmanagedType.LPWStr)] string path, IntPtr reserved, LoadLibraryFlags flags);

    /// <summary>
    /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count.
    /// When the reference count reaches zero, the module is unloaded from the address space of the calling
    /// process and the handle is no longer valid.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-freelibrary"></a></remarks>
    [DllImport("kernel32.dll", PreserveSig = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeLibrary(IntPtr moduleHandle);

    /// <summary>
    /// Determines the location of a resource with the specified type and name in the specified module.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-findresourcew"></a></remarks>
    [DllImport("kernel32.dll", EntryPoint = "FindResourceW", PreserveSig = true)]
    public static extern IntPtr FindResource(HMODULE hModule, IntPtr lpName, IntPtr lpType);

    /// <summary>
    /// Retrieves a pointer to the specified resource in memory.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-lockresource"></a></remarks>
    [DllImport("kernel32.dll", PreserveSig = true)]
    public static extern IntPtr LockResource(IntPtr hResData);

    /// <summary>
    /// Retrieves the size, in bytes, of the specified resource.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-sizeofresource"></a></remarks>
    [DllImport("kernel32.dll", PreserveSig = true)]
    public static extern int SizeofResource(HMODULE hModule, IntPtr hResInfo);

    /// <summary>
    /// Retrieves a handle that can be used to obtain a pointer to the first byte of the specified resource in memory.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-loadresource"></a></remarks>
    [DllImport("kernel32.dll", PreserveSig = true)]
    public static extern IntPtr LoadResource(HMODULE hModule, IntPtr hResInfo);

    /// <summary>
    /// Deconstructs an application user model ID to its package family name and package relative application ID (PRAID).
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/appmodel/nf-appmodel-parseapplicationusermodelid"></a></remarks>
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
    public static extern int ParseApplicationUserModelId([MarshalAs(UnmanagedType.LPWStr)] string applicationUserModelId, ref int packageFamilyNameLength, StringBuilder packageFamilyName, ref int packageRelativeApplicationIdLength, StringBuilder packageRelativeApplicationId);

    /// <summary>
    /// Finds the packages with the specified family name for the current user.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/appmodel/nf-appmodel-findpackagesbypackagefamily"></a></remarks>
    [DllImport("kernel32.dll", EntryPoint = "FindPackagesByPackageFamily", CharSet = CharSet.Unicode, PreserveSig = true)]
    public static extern int FindPackagesByPackageFamilyInitial([MarshalAs(UnmanagedType.LPWStr)] string packageFamilyName, int packageFilters, ref int count, IntPtr packageFullNames, ref int bufferLength, IntPtr buffer, IntPtr packageProperties);

    /// <summary>
    /// Finds the packages with the specified family name for the current user.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/appmodel/nf-appmodel-findpackagesbypackagefamily"></a></remarks>
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
    public static extern int FindPackagesByPackageFamily([MarshalAs(UnmanagedType.LPWStr)] string packageFamilyName, int packageFilters, ref int count, IntPtr[] packageFullNames, ref int bufferLength, IntPtr buffer, IntPtr packageProperties);

    /// <summary>
    /// Opens the package information of the specified package.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/appmodel/nf-appmodel-openpackageinfobyfullname"></a></remarks>
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
    public static extern int OpenPackageInfoByFullName([MarshalAs(UnmanagedType.LPWStr)] string packageFullName, int reserved, out IntPtr packageInfoReference);

    /// <summary>
    /// Gets the IDs of apps in the specified package.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/appmodel/nf-appmodel-getpackageapplicationids"></a></remarks>
    [DllImport("kernel32.dll", PreserveSig = true)]
    public static extern int GetPackageApplicationIds(IntPtr packageInfoReference, ref int bufferLength, IntPtr buffer, out int count);

    /// <summary>
    /// Closes a reference to the specified package information.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/appmodel/nf-appmodel-closepackageinfo"></a></remarks>
    [DllImport("kernel32.dll", PreserveSig = true)]
    public static extern int ClosePackageInfo(IntPtr packageInfoReference);

    /// <summary>
    /// Gets the package information for the specified package.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/appmodel/nf-appmodel-getpackageinfo"></a></remarks>
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern int GetPackageInfo(IntPtr packageInfoReference, PackageConstants flags, ref int bufferLength, IntPtr buffer, out int count);

    /// <summary>
    /// Gets the package full name for the specified process.
    /// </summary>
    /// <remarks><a href="https://learn.microsoft.com/en-us/windows/win32/api/appmodel/nf-appmodel-getpackagefullname"></a></remarks>
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern int GetPackageFullName(IntPtr hProcess, ref int packageFullNameLength, StringBuilder packageFullName);
}
