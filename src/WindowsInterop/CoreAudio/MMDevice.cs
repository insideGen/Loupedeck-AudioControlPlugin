namespace WindowsInterop.CoreAudio
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    using WindowsInterop.PropertySystem;

    public class MMDevice : IAudioControlDevice, IDisposable
    {
        public event EventHandler<string> PropertyChanged;
        public event EventHandler<Exception> SessionLoadError;

        private readonly IMMDevice deviceComObj;

        private AudioClient _audioClient = null;
        private AudioEndpointVolume _audioEndpointVolume = null;
        private AudioMeterInformation _audioMeterInformation = null;
        private AudioSessionManager _audioSessionManager = null;
        private DeviceTopology _deviceTopology = null;
        private PropertyStore _propertyStore = null;

        public AudioClient AudioClient
        {
            get
            {
                if (this._audioClient == null)
                {
                    try
                    {
                        Marshal.ThrowExceptionForHR(this.deviceComObj.Activate(typeof(IAudioClient).GUID, ClsCtx.ALL, IntPtr.Zero, out object audioClient));
                        this._audioClient = new AudioClient(audioClient as IAudioClient);
                    }
                    catch
                    {
                        this._audioClient = null;
                    }
                }
                return this._audioClient;
            }
        }

        public AudioEndpointVolume AudioEndpointVolume
        {
            get
            {
                if (this._audioEndpointVolume == null)
                {
                    try
                    {
                        Marshal.ThrowExceptionForHR(this.deviceComObj.Activate(typeof(IAudioEndpointVolume).GUID, ClsCtx.ALL, IntPtr.Zero, out object audioEndpointVolume));
                        this._audioEndpointVolume = new AudioEndpointVolume(this, audioEndpointVolume as IAudioEndpointVolume);
                    }
                    catch
                    {
                        this._audioEndpointVolume = null;
                    }
                }
                return this._audioEndpointVolume;
            }
        }

        public AudioMeterInformation AudioMeterInformation
        {
            get
            {
                if (this._audioMeterInformation == null)
                {
                    try
                    {
                        Marshal.ThrowExceptionForHR(this.deviceComObj.Activate(typeof(IAudioMeterInformation).GUID, ClsCtx.ALL, IntPtr.Zero, out object audioMeterInformation));
                        this._audioMeterInformation = new AudioMeterInformation(audioMeterInformation as IAudioMeterInformation);
                    }
                    catch
                    {
                        this._audioMeterInformation = null;
                    }
                }
                return this._audioMeterInformation;
            }
        }

        public AudioSessionManager AudioSessionManager
        {
            get
            {
                if (this._audioSessionManager == null)
                {
                    try
                    {
                        Marshal.ThrowExceptionForHR(this.deviceComObj.Activate(typeof(IAudioSessionManager).GUID, ClsCtx.ALL, IntPtr.Zero, out object audioSessionManager));
                        this._audioSessionManager = new AudioSessionManager(audioSessionManager as IAudioSessionManager);
                        this._audioSessionManager.SessionLoadError += (s, ex) => this.SessionLoadError?.Invoke(s, ex);
                    }
                    catch
                    {
                        this._audioSessionManager = null;
                    }
                }
                return this._audioSessionManager;
            }
        }

        public DeviceTopology DeviceTopology
        {
            get
            {
                if (this._deviceTopology == null)
                {
                    try
                    {
                        Marshal.ThrowExceptionForHR(this.deviceComObj.Activate(typeof(IDeviceTopology).GUID, ClsCtx.ALL, IntPtr.Zero, out object deviceTopology));
                        this._deviceTopology = new DeviceTopology(deviceTopology as IDeviceTopology);
                    }
                    catch
                    {
                        this._deviceTopology = null;
                    }
                }
                return this._deviceTopology;
            }
        }

        public PropertyStore PropertyStore
        {
            get
            {
                if (this._propertyStore == null)
                {
                    try
                    {
                        Marshal.ThrowExceptionForHR(this.deviceComObj.OpenPropertyStore(StorageAccessMode.Read, out IPropertyStore propstore));
                        this._propertyStore = new PropertyStore(propstore);
                    }
                    catch
                    {
                        this._propertyStore = null;
                    }
                }
                return this._propertyStore;
            }
        }

        public string Id { get; } = null;
        public DataFlow DataFlow { get; }
        public DeviceState State { get; private set; }
        public string DeviceName { get; private set; } = null;
        public string DeviceDescription { get; private set; } = null;
        public string InterfaceName { get; private set; } = null;
        public string InstanceId { get; private set; } = null;
        public string IconPath { get; private set; } = null;

        public MMDevice(IMMDevice device)
        {
            this.deviceComObj = device;

            Marshal.ThrowExceptionForHR(this.deviceComObj.GetId(out string id));
            this.Id = id;

            IMMEndpoint mmEndpoint = this.deviceComObj as IMMEndpoint;
            mmEndpoint.GetDataFlow(out DataFlow dataFlow);
            this.DataFlow = dataFlow;

            this.GetState();

            this.LoadProperties(raiseEvent: false);
        }

        public DeviceState GetState()
        {
            Marshal.ThrowExceptionForHR(this.deviceComObj.GetState(out DeviceState state));
            this.State = state;
            return state;
        }

        public void LoadProperties(bool raiseEvent = true)
        {
            if (this.PropertyStore != null)
            {
                try
                {
                    string deviceName = this.PropertyStore.GetValue<string>(PropertyKeys.PKEY_Device_FriendlyName);
                    if (this.DeviceName != deviceName)
                    {
                        this.DeviceName = deviceName;
                        if (raiseEvent)
                        {
                            this.PropertyChanged?.Invoke(this, "DeviceName");
                        }
                    }
                }
                catch
                {
                }

                try
                {
                    string deviceDescription = this.PropertyStore.GetValue<string>(PropertyKeys.PKEY_Device_DeviceDesc);
                    if (this.DeviceDescription != deviceDescription)
                    {
                        this.DeviceDescription = deviceDescription;
                        if (raiseEvent)
                        {
                            this.PropertyChanged?.Invoke(this, "DeviceDescription");
                        }
                    }
                }
                catch
                {
                }

                try
                {
                    string interfaceName = this.PropertyStore.GetValue<string>(PropertyKeys.PKEY_DeviceInterface_FriendlyName);
                    if (this.InterfaceName != interfaceName)
                    {
                        this.InterfaceName = interfaceName;
                        if (raiseEvent)
                        {
                            this.PropertyChanged?.Invoke(this, "InterfaceName");
                        }
                    }
                }
                catch
                {
                }

                try
                {
                    string instanceId = this.PropertyStore.GetValue<string>(PropertyKeys.PKEY_Device_InstanceId);
                    if (this.InstanceId != instanceId)
                    {
                        this.InstanceId = instanceId;
                        if (raiseEvent)
                        {
                            this.PropertyChanged?.Invoke(this, "InstanceId");
                        }
                    }
                }
                catch
                {
                }

                try
                {
                    string iconPath = this.PropertyStore.GetValue<string>(PropertyKeys.PKEY_Device_IconPath);
                    if (this.IconPath != iconPath)
                    {
                        this.IconPath = iconPath;
                        if (raiseEvent)
                        {
                            this.PropertyChanged?.Invoke(this, "IconPath");
                        }
                    }
                }
                catch
                {
                }
            }
        }

        string IAudioControl.DisplayName { get => this.DeviceDescription; }
        bool IAudioControl.Muted { get => this.AudioEndpointVolume.Mute; set => this.AudioEndpointVolume.Mute = value; }
        float IAudioControl.VolumeScalar { get => this.AudioEndpointVolume.MasterVolumeLevelScalar; set => this.AudioEndpointVolume.MasterVolumeLevelScalar = value; }
        float[] IAudioControl.PeakValues { get => this.AudioMeterInformation.PeakValues.ToArray(); }
        float IAudioControlDevice.Volume { get => this.AudioEndpointVolume.MasterVolumeLevel; set => this.AudioEndpointVolume.MasterVolumeLevel = value; }
        float IAudioControlDevice.MinDecibels { get => this.AudioEndpointVolume.VolumeRange.MinDecibels; }
        float IAudioControlDevice.MaxDecibels { get => this.AudioEndpointVolume.VolumeRange.MaxDecibels; }
        float IAudioControlDevice.IncrementDecibels { get => this.AudioEndpointVolume.VolumeRange.IncrementDecibels; }
        IEnumerable<IAudioControlSession> IAudioControlDevice.Sessions { get => this.AudioSessionManager.Sessions; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.AudioEndpointVolume?.Dispose();
            this.AudioMeterInformation?.Dispose();
            this.AudioSessionManager?.Dispose();
            this.DeviceTopology?.Dispose();
            this.PropertyStore?.Dispose();
            try
            {
                Marshal.ReleaseComObject(this.deviceComObj);
            }
            catch
            {
            }
        }

        ~MMDevice()
        {
            this.Dispose();
        }
    }
}
