namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

using WindowsInterop.PropertySystem;

/// <summary>
/// MM Device Enumerator.
/// </summary>
public class MMDeviceEnumerator : IDisposable
{
    public event EventHandler<DeviceStateEventArgs> DeviceStateChanged;
    public event EventHandler<DeviceIdEventArgs> DeviceAdded;
    public event EventHandler<DeviceIdEventArgs> DeviceRemoved;
    public event EventHandler<DefaultDeviceEventArgs> DefaultDeviceChanged;
    public event EventHandler<PropertyValueEventArgs> DevicePropertyChanged;

    private readonly IMMDeviceEnumerator mmDeviceEnumeratorComObj;
    private readonly MMNotificationClient mmNotificationClient;

    private readonly IPolicyConfig policyConfigComObj;

    /// <summary>
    /// Creates a new MM Device Enumerator.
    /// </summary>
    public MMDeviceEnumerator()
    {
        Type mmDeviceEnumeratorType = Type.GetTypeFromCLSID(new Guid("BCDE0395-E52F-467C-8E3D-C4579291692E"));
        this.mmDeviceEnumeratorComObj = Activator.CreateInstance(mmDeviceEnumeratorType) as IMMDeviceEnumerator;

        this.mmNotificationClient = new MMNotificationClient();
        this.mmNotificationClient.DeviceStateChanged += (object sender, DeviceStateEventArgs e) => this.DeviceStateChanged?.Invoke(this, e);
        this.mmNotificationClient.DeviceAdded += (object sender, DeviceIdEventArgs e) => this.DeviceAdded?.Invoke(this, e);
        this.mmNotificationClient.DeviceRemoved += (object sender, DeviceIdEventArgs e) => this.DeviceRemoved?.Invoke(this, e);
        this.mmNotificationClient.DefaultDeviceChanged += (object sender, DefaultDeviceEventArgs e) => this.DefaultDeviceChanged?.Invoke(this, e);
        this.mmNotificationClient.PropertyValueChanged += (object sender, PropertyValueEventArgs e) => this.DevicePropertyChanged?.Invoke(this, e);
        Marshal.ThrowExceptionForHR(this.mmDeviceEnumeratorComObj.RegisterEndpointNotificationCallback(this.mmNotificationClient));

        Type policyConfigType = Type.GetTypeFromCLSID(new Guid("870AF99C-171D-4F9E-AF0D-E63DF40C2BC9"));
        this.policyConfigComObj = Activator.CreateInstance(policyConfigType) as IPolicyConfig;
    }

    /// <summary>
    /// Enumerate audio endpoints.
    /// </summary>
    public MMDeviceCollection EnumAudioEndpoints(DataFlow dataFlow, DeviceState stateMask)
    {
        Marshal.ThrowExceptionForHR(this.mmDeviceEnumeratorComObj.EnumAudioEndpoints(dataFlow, stateMask, out IMMDeviceCollection devices));
        return new MMDeviceCollection(devices);
    }

    /// <summary>
    /// Get default audio endpoint.
    /// </summary>
    public bool GetDefaultAudioEndpoint(DataFlow dataFlow, Role role, out MMDevice endpoint)
    {
        try
        {
            Marshal.ThrowExceptionForHR(this.mmDeviceEnumeratorComObj.GetDefaultAudioEndpoint(dataFlow, role, out IMMDevice iendpoint));
            endpoint = new MMDevice(iendpoint);
            return true;
        }
        catch
        {
            endpoint = null;
            return false;
        }
    }

    /// <summary>
    /// Get default audio endpoint Id.
    /// </summary>
    public bool GetDefaultAudioEndpointId(DataFlow dataFlow, Role role, out string endpointId)
    {
        if (this.GetDefaultAudioEndpoint(dataFlow, role, out MMDevice endpoint))
        {
            endpointId = endpoint.Id;
            endpoint.Dispose();
            return true;
        }
        endpointId = null;
        return false;
    }

    /// <summary>
    /// Get an audio endpoint device that is identified by an endpoint ID string.
    /// </summary>
    public MMDevice GetDevice(string id)
    {
        Marshal.ThrowExceptionForHR(this.mmDeviceEnumeratorComObj.GetDevice(id, out IMMDevice device));
        return new MMDevice(device);
    }

    /// <summary>
    /// Check if a default audio endpoint exists.
    /// </summary>
    public bool HasDefaultAudioEndpoint(DataFlow dataFlow, Role role)
    {
        const int E_NOTFOUND = unchecked((int)0x80070490);
        int hresult = this.mmDeviceEnumeratorComObj.GetDefaultAudioEndpoint(dataFlow, role, out IMMDevice device);
        if (hresult == 0x0)
        {
            Marshal.ReleaseComObject(device);
            return true;
        }
        else if (hresult == E_NOTFOUND)
        {
            return false;
        }
        Marshal.ThrowExceptionForHR(hresult);
        return false;
    }

    /// <summary>
    /// Set default audio endpoint.
    /// </summary>
    public void SetDefaultAudioEndpoint(string deviceId, Role role)
    {
        Marshal.ThrowExceptionForHR(this.policyConfigComObj.SetDefaultEndpoint(deviceId, role));
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Marshal.ThrowExceptionForHR(this.mmDeviceEnumeratorComObj.UnregisterEndpointNotificationCallback(this.mmNotificationClient));
        Marshal.ReleaseComObject(this.mmDeviceEnumeratorComObj);
        Marshal.ReleaseComObject(this.policyConfigComObj);
    }

    ~MMDeviceEnumerator()
    {
        this.Dispose();
    }
}
