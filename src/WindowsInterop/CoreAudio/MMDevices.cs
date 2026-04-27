namespace WindowsInterop.CoreAudio;

using System.Collections.ObjectModel;
using System.Linq;

using WindowsInterop.PropertySystem;

public class MMDevices : IDisposable
{
    public event EventHandler<DeviceIdEventArgs> DeviceAdded;
    public event EventHandler<DeviceIdEventArgs> DeviceRemoved;
    public event EventHandler<DeviceStateEventArgs> DeviceStateChanged;
    public event EventHandler<PropertyValueEventArgs> PropertyValueChanged;
    public event EventHandler<DefaultDeviceEventArgs> DefaultDeviceChanged;

    public MMDeviceEnumerator MMDeviceEnumerator { get; }
    public DataFlow DataFlow { get; }
    public DeviceState StateMask { get; }
    public ObservableCollection<MMDevice> Items { get; }
    public DefaultDeviceId DefaultDeviceId { get; private set; }

    public MMDevices(DataFlow dataFlow, DeviceState stateMask)
    {
        if (dataFlow == DataFlow.All)
        {
            throw new ArgumentException("DataFlow.All is not supported.");
        }

        this.DataFlow = dataFlow;
        this.StateMask = stateMask;

        this.MMDeviceEnumerator = new MMDeviceEnumerator();
        this.MMDeviceEnumerator.DeviceAdded += this.OnDeviceAdded;
        this.MMDeviceEnumerator.DeviceRemoved += this.OnDeviceRemoved;
        this.MMDeviceEnumerator.DeviceStateChanged += this.OnDeviceStateChanged;
        this.MMDeviceEnumerator.DevicePropertyChanged += this.OnPropertyValueChanged;
        this.MMDeviceEnumerator.DefaultDeviceChanged += this.OnDefaultDeviceChanged;

        this.Items = new ObservableCollection<MMDevice>(this.MMDeviceEnumerator.EnumAudioEndpoints(this.DataFlow, this.StateMask));

        this.DefaultDeviceId = new DefaultDeviceId();
        if (this.MMDeviceEnumerator.GetDefaultAudioEndpointId(this.DataFlow, Role.Communications, out string communicationsEndpointId))
        {
            this.DefaultDeviceId.Communication = communicationsEndpointId;
        }
        if (this.MMDeviceEnumerator.GetDefaultAudioEndpointId(this.DataFlow, Role.Console, out string consoleEndpointId))
        {
            this.DefaultDeviceId.Console = consoleEndpointId;
        }
        if (this.MMDeviceEnumerator.GetDefaultAudioEndpointId(this.DataFlow, Role.Multimedia, out string multimediaEndpointId))
        {
            this.DefaultDeviceId.Multimedia = multimediaEndpointId;
        }
    }

    public bool IsDefaultDevice(string deviceId)
    {
        return (!string.IsNullOrWhiteSpace(this.DefaultDeviceId.Communication) && this.DefaultDeviceId.Communication == deviceId)
            || (!string.IsNullOrWhiteSpace(this.DefaultDeviceId.Console) && this.DefaultDeviceId.Console == deviceId)
            || (!string.IsNullOrWhiteSpace(this.DefaultDeviceId.Multimedia) && this.DefaultDeviceId.Multimedia == deviceId);
    }

    private void OnDeviceStateChanged(object sender, DeviceStateEventArgs e)
    {
        MMDevice deviceInCollection = this.Items.FirstOrDefault((MMDevice d) => d.Id == e.DeviceId);
        MMDevice deviceUpdated = this.MMDeviceEnumerator.GetDevice(e.DeviceId);
        if (deviceUpdated != null && deviceUpdated.DataFlow == this.DataFlow)
        {
            if (deviceInCollection != null && deviceUpdated.State != this.StateMask)
            {
                this.Items.Remove(deviceInCollection);
                deviceInCollection.Dispose();
                deviceUpdated.Dispose();
                this.DeviceStateChanged?.Invoke(this, e);
            }
            else if (deviceInCollection == null && this.StateMask == deviceUpdated.State)
            {
                this.Items.Add(deviceUpdated);
                this.DeviceStateChanged?.Invoke(this, e);
            }
        }
    }

    private void OnDeviceAdded(object sender, DeviceIdEventArgs e)
    {
        MMDevice device = this.Items.FirstOrDefault((MMDevice d) => d.Id == e.DeviceId);
        if (device == null)
        {
            MMDevice newDevice = this.MMDeviceEnumerator.GetDevice(e.DeviceId);
            if (newDevice.DataFlow == this.DataFlow && newDevice.State == this.StateMask)
            {
                this.Items.Add(newDevice);
                this.DeviceAdded?.Invoke(this, e);
            }
            else
            {
                newDevice.Dispose();
            }
        }
    }

    private void OnDeviceRemoved(object sender, DeviceIdEventArgs e)
    {
        if (this.Items.FirstOrDefault((MMDevice d) => d.Id == e.DeviceId) is MMDevice device)
        {
            this.Items.Remove(device);
            device.Dispose();
            this.DeviceRemoved?.Invoke(this, e);
        }
    }

    private void OnDefaultDeviceChanged(object sender, DefaultDeviceEventArgs e)
    {
        if (e.Flow == this.DataFlow)
        {
            if (e.Role == Role.Communications)
            {
                this.DefaultDeviceId.Communication = e.DefaultDeviceId;
            }
            else if (e.Role == Role.Console)
            {
                this.DefaultDeviceId.Console = e.DefaultDeviceId;
            }
            else if (e.Role == Role.Multimedia)
            {
                this.DefaultDeviceId.Multimedia = e.DefaultDeviceId;
            }
        }
        this.DefaultDeviceChanged?.Invoke(sender, e);
    }

    private void OnPropertyValueChanged(object sender, PropertyValueEventArgs e)
    {
        if (this.Items.FirstOrDefault((MMDevice d) => d.Id == e.DeviceId) is MMDevice device)
        {
            device.LoadProperties();
            this.PropertyValueChanged?.Invoke(this, e);
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        this.Items.ToList().ForEach((MMDevice device) => device.Dispose());
        this.MMDeviceEnumerator.Dispose();
    }

    ~MMDevices()
    {
        this.Dispose();
    }
}
