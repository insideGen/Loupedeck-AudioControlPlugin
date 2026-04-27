namespace WindowsInterop.ModernApp;

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using System.Text;

using WindowsInterop.Win32;

public sealed class AppxPackage
{
    private readonly List<AppxApp> _apps = new List<AppxApp>();
    private IAppxManifestProperties _properties;

    private AppxPackage()
    {
    }

    public string FullName { get; private set; }
    public string Path { get; private set; }
    public string Publisher { get; private set; }
    public string PublisherId { get; private set; }
    public string ResourceId { get; private set; }
    public string FamilyName { get; private set; }
    public string ApplicationUserModelId { get; private set; }
    public string Logo { get; private set; }
    public string PublisherDisplayName { get; private set; }
    public string Description { get; private set; }
    public string DisplayName { get; private set; }
    public bool IsFramework { get; private set; }
    public Version Version { get; private set; }
    public AppxPackageArchitecture ProcessorArchitecture { get; private set; }

    public IReadOnlyList<AppxApp> Apps
    {
        get
        {
            return this._apps;
        }
    }

    public IEnumerable<AppxPackage> DependencyGraph
    {
        get
        {
            return QueryPackageInfo(this.FullName, PackageConstants.PACKAGE_FILTER_ALL_LOADED).Where(p => p.FullName != this.FullName);
        }
    }

    public string FindLightUnplatedVariant(string resourceName)
    {
        if (resourceName == null)
        {
            return null;
        }

        string name = System.IO.Path.GetFileNameWithoutExtension(resourceName);
        string ext = System.IO.Path.GetExtension(resourceName);
        string dir = System.IO.Path.Combine(this.Path, System.IO.Path.GetDirectoryName(resourceName));

        // Preferred sizes in descending order
        int[] targetSizes = { 96, 80, 72, 64, 48, 44, 40, 36, 32, 30, 24, 20, 16 };
        try
        {
            foreach (int size in targetSizes)
            {
                string candidate = System.IO.Path.Combine(dir, $"{name}.targetsize-{size}_altform-lightunplated{ext}");
                if (System.IO.File.Exists(candidate))
                {
                    return candidate;
                }
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
        return null;
    }

    public string FindHighestScaleQualifiedImagePath(string resourceName)
    {
        if (resourceName == null)
        {
            throw new ArgumentNullException("resourceName");
        }

        const string scaleToken = ".scale-";
        List<int> sizes = new List<int>();
        string name = System.IO.Path.GetFileNameWithoutExtension(resourceName);
        string ext = System.IO.Path.GetExtension(resourceName);
        IEnumerable<string> logoFiles;
        try
        {
            logoFiles = Directory.EnumerateFiles(System.IO.Path.Combine(this.Path, System.IO.Path.GetDirectoryName(resourceName)), name + scaleToken + "*" + ext);
        }
        catch
        {
            return null;
        }
        foreach (string file in logoFiles)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
            int pos = fileName.IndexOf(scaleToken) + scaleToken.Length;
            string sizeText = fileName.Substring(pos);
            if (int.TryParse(sizeText, out int size))
            {
                sizes.Add(size);
            }
        }
        if (sizes.Count == 0)
        {
            return null;
        }

        sizes.Sort();
        return System.IO.Path.Combine(this.Path, System.IO.Path.GetDirectoryName(resourceName), name + scaleToken + sizes.Last() + ext);
    }

    public override string ToString()
    {
        return this.FullName;
    }

    public static AppxPackage FromWindow(IntPtr handle)
    {
        User32.GetWindowThreadProcessId(handle, out int processId);
        if (processId == 0)
        {
            return null;
        }

        return FromProcess(processId);
    }

    public static AppxPackage FromProcess(Process process)
    {
        if (process == null)
        {
            process = Process.GetCurrentProcess();
        }

        try
        {
            return FromProcess(process.Handle);
        }
        catch
        {
            // Probably access denied on .Handle
            return null;
        }
    }

    public static AppxPackage FromProcess(int processId)
    {
        IntPtr hProcess = Kernel32.OpenProcess(Kernel32.ProcessFlags.PROCESS_QUERY_LIMITED_INFORMATION, false, processId);
        try
        {
            return FromProcess(hProcess);
        }
        finally
        {
            if (hProcess != IntPtr.Zero)
            {
                Kernel32.CloseHandle(hProcess);
            }
        }
    }

    public static AppxPackage FromProcess(IntPtr hProcess)
    {
        if (hProcess == IntPtr.Zero)
        {
            return null;
        }

        // hProcess must have been opened with QueryLimitedInformation
        int len = 0;
        Kernel32.GetPackageFullName(hProcess, ref len, null);
        if (len == 0)
        {
            return null;
        }

        StringBuilder sb = new StringBuilder(len);
        string fullName = Kernel32.GetPackageFullName(hProcess, ref len, sb) == 0 ? sb.ToString() : null;
        if (string.IsNullOrEmpty(fullName)) // Not an AppX
        {
            return null;
        }

        AppxPackage package = QueryPackageInfo(fullName, PackageConstants.PACKAGE_FILTER_HEAD).First();

        len = 0;
        Kernel32.GetApplicationUserModelId(hProcess, ref len, null);
        sb = new StringBuilder(len);
        package.ApplicationUserModelId = Kernel32.GetApplicationUserModelId(hProcess, ref len, sb) == 0 ? sb.ToString() : null;
        return package;
    }

    public string GetPropertyStringValue(string name)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }

        return GetStringValue(this._properties, name);
    }

    public bool GetPropertyBoolValue(string name)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }

        return GetBoolValue(this._properties, name);
    }

    public string LoadResourceString(string resource)
    {
        return LoadResourceString(this.FullName, resource);
    }

    private static IEnumerable<AppxPackage> QueryPackageInfo(string fullName, PackageConstants flags)
    {
        Kernel32.OpenPackageInfoByFullName(fullName, 0, out IntPtr infoRef);
        if (infoRef != IntPtr.Zero)
        {
            IntPtr infoBuffer = IntPtr.Zero;
            try
            {
                int len = 0;
                Kernel32.GetPackageInfo(infoRef, flags, ref len, IntPtr.Zero, out int count);
                if (len > 0)
                {
                    IAppxFactory factory = (IAppxFactory)new AppxFactory();
                    infoBuffer = Marshal.AllocHGlobal(len);
                    int res = Kernel32.GetPackageInfo(infoRef, flags, ref len, infoBuffer, out count);
                    for (int i = 0; i < count; i++)
                    {
                        PackageInfo info = (PackageInfo)Marshal.PtrToStructure(infoBuffer + i * Marshal.SizeOf(typeof(PackageInfo)), typeof(PackageInfo));
                        AppxPackage package = new AppxPackage();
                        package.FamilyName = info.PackageFamilyName;
                        package.FullName = info.PackageFullName;
                        package.Path = info.Path;
                        package.Publisher = info.PackageId.Publisher;
                        package.PublisherId = info.PackageId.PublisherId;
                        package.ResourceId = info.PackageId.ResourceId;
                        package.ProcessorArchitecture = info.PackageId.ProcessorArchitecture;
                        package.Version = new Version(info.PackageId.VersionMajor, info.PackageId.VersionMinor, info.PackageId.VersionBuild, info.PackageId.VersionRevision);

                        // Read manifest
                        string manifestPath = System.IO.Path.Combine(package.Path, "AppXManifest.xml");
                        const int STGM_SHARE_DENY_NONE = 0x40;
                        Shlwapi.SHCreateStreamOnFileEx(manifestPath, STGM_SHARE_DENY_NONE, 0, false, IntPtr.Zero, out IStream strm);
                        if (strm != null)
                        {
                            IAppxManifestReader reader = factory.CreateManifestReader(strm);
                            package._properties = reader.GetProperties();
                            package.Description = package.GetPropertyStringValue("Description");
                            package.DisplayName = package.GetPropertyStringValue("DisplayName");
                            package.Logo = package.GetPropertyStringValue("Logo");
                            package.PublisherDisplayName = package.GetPropertyStringValue("PublisherDisplayName");
                            package.IsFramework = package.GetPropertyBoolValue("Framework");

                            IAppxManifestApplicationsEnumerator apps = reader.GetApplications();
                            while (apps.GetHasCurrent())
                            {
                                IAppxManifestApplication app = apps.GetCurrent();
                                AppxApp appx = new AppxApp(app);
                                appx.BackgroundColor = GetStringValue(app, "BackgroundColor");
                                appx.Description = GetStringValue(app, "Description");
                                appx.DisplayName = GetStringValue(app, "DisplayName");
                                appx.EntryPoint = GetStringValue(app, "EntryPoint");
                                appx.Executable = GetStringValue(app, "Executable");
                                appx.ForegroundText = GetStringValue(app, "ForegroundText");
                                appx.Id = GetStringValue(app, "Id");
                                appx.Logo = GetStringValue(app, "Logo");
                                appx.MinWidth = GetStringValue(app, "MinWidth");
                                appx.ShortName = GetStringValue(app, "ShortName");
                                appx.SmallLogo = GetStringValue(app, "SmallLogo");
                                appx.Square30x30Logo = GetStringValue(app, "Square30x30Logo");
                                appx.Square44x44Logo = GetStringValue(app, "Square44x44Logo");
                                appx.Square70x70Logo = GetStringValue(app, "Square70x70Logo");
                                appx.Square71x71Logo = GetStringValue(app, "Square71x71Logo");
                                appx.Square150x150Logo = GetStringValue(app, "Square150x150Logo");
                                appx.Square310x310Logo = GetStringValue(app, "Square310x310Logo");
                                appx.StartPage = GetStringValue(app, "StartPage");
                                appx.Wide310x310Logo = GetStringValue(app, "Wide310x310Logo");
                                appx.WideLogo = GetStringValue(app, "WideLogo");
                                package._apps.Add(appx);
                                apps.MoveNext();
                            }
                            Marshal.ReleaseComObject(strm);
                        }
                        yield return package;
                    }
                    Marshal.ReleaseComObject(factory);
                }
            }
            finally
            {
                if (infoBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(infoBuffer);
                }
                Kernel32.ClosePackageInfo(infoRef);
            }
        }
    }

    public static string LoadResourceString(string packageFullName, string resource)
    {
        if (packageFullName == null)
        {
            throw new ArgumentNullException("packageFullName");
        }

        if (string.IsNullOrWhiteSpace(resource))
        {
            return null;
        }

        const string resourceScheme = "ms-resource:";
        if (!resource.StartsWith(resourceScheme))
        {
            return null;
        }

        string part = resource.Substring(resourceScheme.Length);
        string url;

        if (part.StartsWith("/"))
        {
            url = resourceScheme + "//" + part;
        }
        else
        {
            url = resourceScheme + "///resources/" + part;
        }

        string source = string.Format("@{{{0}? {1}}}", packageFullName, url);
        StringBuilder sb = new StringBuilder(1024);
        int i = Shlwapi.SHLoadIndirectString(source, sb, sb.Capacity, IntPtr.Zero);
        if (i != 0)
        {
            return null;
        }

        return sb.ToString();
    }

    private static string GetStringValue(IAppxManifestProperties props, string name)
    {
        if (props == null)
        {
            return null;
        }

        props.GetStringValue(name, out string value);
        return value;
    }

    private static bool GetBoolValue(IAppxManifestProperties props, string name)
    {
        props.GetBoolValue(name, out bool value);
        return value;
    }

    internal static string GetStringValue(IAppxManifestApplication app, string name)
    {
        app.GetStringValue(name, out string value);
        return value;
    }

    public static bool IsPackagedProcess(IntPtr hProcess)
    {
        bool isPackagedProcess = false;
        int bufferSize = 0;
        if (Kernel32.GetPackageId(hProcess, ref bufferSize, IntPtr.Zero) == HRESULT.ERROR_INSUFFICIENT_BUFFER)
        {
            IntPtr hPackageId = Marshal.AllocHGlobal(bufferSize);
            try
            {
                if (Kernel32.GetPackageId(hProcess, ref bufferSize, hPackageId) == HRESULT.S_OK)
                {
                    PackageId packageId = Marshal.PtrToStructure<PackageId>(hPackageId);
                    isPackagedProcess = packageId.Publisher.Length > 0;
                }
            }
            finally
            {
                Marshal.FreeHGlobal(hPackageId);
            }
        }
        return isPackagedProcess;
    }

    public static bool IsPackagedProcess(int processId)
    {
        bool isPackagedProcess = false;
        IntPtr hProcess = Kernel32.OpenProcess(Kernel32.ProcessFlags.PROCESS_QUERY_LIMITED_INFORMATION, false, processId);
        if (hProcess != IntPtr.Zero)
        {
            try
            {
                isPackagedProcess = IsPackagedProcess(hProcess);
            }
            finally
            {
                Kernel32.CloseHandle(hProcess);
            }
        }
        return isPackagedProcess;
    }
}
