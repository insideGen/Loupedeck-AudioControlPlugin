namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;
using System.Text;

public class AudioSessionControl : IAudioControlSession, IDisposable
{
    public event EventHandler<(string displayName, Guid eventContext)> DisplayNameChanged;
    public event EventHandler<(string iconPath, Guid eventContext)> IconPathChanged;
    public event EventHandler<(float volume, bool isMuted, Guid eventContext)> SimpleVolumeChanged;
    public event EventHandler<(int channelCount, IntPtr newVolumes, int channelIndex, Guid eventContext)> ChannelVolumeChanged;
    public event EventHandler<(Guid groupingId, Guid eventContext)> GroupingParamChanged;
    public event EventHandler<AudioSessionState> StateChanged;
    public event EventHandler<AudioSessionDisconnectReason> SessionDisconnected;

    private readonly IAudioSessionControl2 audioSessionControlInterface;
    private readonly AudioSessionEvents audioSessionEvents;

    private string _displayName = null;
    private string _iconPath = null;
    private string _exePath = null;

    public AudioMeterInformation AudioMeterInformation { get; private set; }

    public SimpleAudioVolume SimpleAudioVolume { get; private set; }

    public int ProcessId { get; private set; }

    public string SessionIdentifier { get; private set; }

    public AudioSessionIdentifier ASI { get; private set; }

    public string SessionInstanceIdentifier { get; private set; }

    public AudioSessionInstanceIdentifier ASII { get; private set; }

    public bool IsSystemSoundsSession { get; private set; }

    public AudioSessionState State { get; private set; }

    public string DisplayName
    {
        get => this._displayName;
        private set
        {
            if (!string.IsNullOrEmpty(value))
            {
                this._displayName = value;
                Marshal.ThrowExceptionForHR(this.audioSessionControlInterface.SetDisplayName(value, Guid.Empty));
            }
        }
    }

    public string IconPath
    {
        get => this._iconPath;
        private set
        {
            if (!string.IsNullOrEmpty(value))
            {
                this._iconPath = value;
                Marshal.ThrowExceptionForHR(this.audioSessionControlInterface.SetIconPath(value, Guid.Empty));
            }
        }
    }

    public AudioSessionControl(IAudioSessionControl audioSessionControl)
    {
        this.audioSessionControlInterface = audioSessionControl as IAudioSessionControl2;
        if (this.audioSessionControlInterface is IAudioMeterInformation meters)
        {
            this.AudioMeterInformation = new AudioMeterInformation(meters);
        }
        if (this.audioSessionControlInterface is ISimpleAudioVolume volume)
        {
            this.SimpleAudioVolume = new SimpleAudioVolume(volume);
        }
        this.LoadProperties();
        this.audioSessionEvents = new AudioSessionEvents();
        this.audioSessionEvents.DisplayNameChanged += this.OnDisplayNameChanged;
        this.audioSessionEvents.IconPathChanged += this.OnIconPathChanged;
        this.audioSessionEvents.SimpleVolumeChanged += this.OnSimpleVolumeChanged;
        this.audioSessionEvents.ChannelVolumeChanged += this.OnChannelVolumeChanged;
        this.audioSessionEvents.GroupingParamChanged += this.OnGroupingParamChanged;
        this.audioSessionEvents.StateChanged += this.OnStateChanged;
        this.audioSessionEvents.SessionDisconnected += this.OnSessionDisconnected;
        Marshal.ThrowExceptionForHR(this.audioSessionControlInterface.RegisterAudioSessionNotification(this.audioSessionEvents));
    }

    public void LoadProperties()
    {
        Marshal.ThrowExceptionForHR(this.audioSessionControlInterface.GetProcessId(out int pid));
        this.ProcessId = pid;

        Marshal.ThrowExceptionForHR(this.audioSessionControlInterface.GetState(out AudioSessionState state));
        this.State = state;

        Marshal.ThrowExceptionForHR(this.audioSessionControlInterface.GetSessionIdentifier(out string sessionId));
        this.SessionIdentifier = sessionId;
        this.ASI = AudioSessionIdentifier.FromString(this.SessionIdentifier);

        Marshal.ThrowExceptionForHR(this.audioSessionControlInterface.GetSessionInstanceIdentifier(out string sessionInstanceId));
        this.SessionInstanceIdentifier = sessionInstanceId;
        this.ASII = AudioSessionInstanceIdentifier.FromString(this.SessionInstanceIdentifier);

        Marshal.ThrowExceptionForHR(this.audioSessionControlInterface.GetDisplayName(out string displayName));
        if (!string.IsNullOrEmpty(displayName) && displayName.StartsWith("@"))
        {
            StringBuilder sb = new StringBuilder(512);
            if (Win32.Shlwapi.SHLoadIndirectString(displayName, sb, sb.Capacity, IntPtr.Zero) == 0)
            {
                this._displayName = sb.ToString();
            }
        }

        Marshal.ThrowExceptionForHR(this.audioSessionControlInterface.GetIconPath(out string iconPath));
        if (!string.IsNullOrEmpty(iconPath) && iconPath.StartsWith("@"))
        {
            this._iconPath = Environment.ExpandEnvironmentVariables(iconPath.TrimStart('@'));
        }
        else
        {
            this._iconPath = iconPath;
        }

        this.IsSystemSoundsSession = this.audioSessionControlInterface.IsSystemSoundsSession() == HRESULT.S_OK;

        if (this.IsSystemSoundsSession)
        {
            this.ProcessId = 0;
            this._exePath = this.ASII.ExePath;
        }
        else
        {
            AppInfo appInfo = this.ProcessId > 0 ? AppInfo.FromProcess(this.ProcessId) : null;
            if (appInfo != null)
            {
                this._displayName = appInfo.DisplayName;
                this._iconPath = appInfo.LogoPath;
                this._exePath = appInfo.ExePath;
            }
            else
            {
                // Fallback for sandboxed/protected processes (e.g. UWP Store apps)
                // Use the exe path from the session identifier if available
                if (string.IsNullOrEmpty(this._exePath))
                {
                    this._exePath = this.ASII.ExePath ?? this.ASI.ExePath;
                }
                if (string.IsNullOrEmpty(this._displayName) && !string.IsNullOrEmpty(this._exePath))
                {
                    this._displayName = Path.GetFileNameWithoutExtension(this._exePath);
                }
            }
        }
    }

    public Guid GetGroupingParam()
    {
        Marshal.ThrowExceptionForHR(this.audioSessionControlInterface.GetGroupingParam(out Guid groupingId));
        return groupingId;
    }

    public void SetGroupingParam(Guid groupingId, Guid context)
    {
        Marshal.ThrowExceptionForHR(this.audioSessionControlInterface.SetGroupingParam(groupingId, context));
    }

    private void OnDisplayNameChanged(object sender, (string displayName, Guid eventContext) e)
    {
        this.DisplayNameChanged?.Invoke(this, e);
    }

    private void OnIconPathChanged(object sender, (string iconPath, Guid eventContext) e)
    {
        this.IconPathChanged?.Invoke(this, e);
    }

    private void OnSimpleVolumeChanged(object sender, (float volume, bool isMuted, Guid eventContext) e)
    {
        this.SimpleVolumeChanged?.Invoke(this, e);
    }

    private void OnChannelVolumeChanged(object sender, (int channelCount, IntPtr newVolumes, int channelIndex, Guid eventContext) e)
    {
        this.ChannelVolumeChanged?.Invoke(this, e);
    }

    private void OnGroupingParamChanged(object sender, (Guid groupingId, Guid eventContext) e)
    {
        this.GroupingParamChanged?.Invoke(this, e);
    }

    private void OnStateChanged(object sender, AudioSessionState state)
    {
        this.State = state;
        this.StateChanged?.Invoke(this, state);
    }

    private void OnSessionDisconnected(object sender, AudioSessionDisconnectReason disconnectReason)
    {
        this.SessionDisconnected?.Invoke(this, disconnectReason);
    }

    string IAudioControl.Id { get => this.SessionIdentifier; }
    bool IAudioControl.Muted { get => this.SimpleAudioVolume.Mute; set => this.SimpleAudioVolume.Mute = value; }
    float IAudioControl.VolumeScalar { get => this.SimpleAudioVolume.Volume; set => this.SimpleAudioVolume.Volume = value; }
    float[] IAudioControl.PeakValues { get => this.AudioMeterInformation.PeakValues.ToArray(); }
    string IAudioControlSession.InstanceId { get => this.SessionInstanceIdentifier; }
    string IAudioControlSession.DeviceId { get => this.ASI.DeviceId; }
    string IAudioControlSession.ExePath { get => this._exePath; }
    string IAudioControlSession.ExeId { get => this.ASII.ExeId; }

    public void Dispose()
    {
        try
        {
            Marshal.ThrowExceptionForHR(this.audioSessionControlInterface.UnregisterAudioSessionNotification(this.audioSessionEvents));
            //Marshal.ReleaseComObject(this.audioSessionControlInterface);
        }
        catch (Exception)
        {
        }
        this.AudioMeterInformation?.Dispose();
        this.SimpleAudioVolume?.Dispose();
        GC.SuppressFinalize(this);
    }

    ~AudioSessionControl()
    {
        this.Dispose();
    }
}
