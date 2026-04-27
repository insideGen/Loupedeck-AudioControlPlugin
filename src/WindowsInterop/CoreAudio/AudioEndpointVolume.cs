namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

public class AudioEndpointVolume : IDisposable
{
    public event EventHandler<AudioVolumeNotificationEventArgs> VolumeNotification;

    private readonly MMDevice parent;

    private readonly IAudioEndpointVolume realAudioEndPointVolume;
    private readonly AudioEndpointVolumeCallback audioEndpointVolumeCallback;

    private Guid NotificationGuid { get; set; } = Guid.Empty;

    private float _masterVolumeLevel;
    private float _masterVolumeLevelScalar;
    private bool _mute;

    public EndpointHardwareSupport HardwareSupport { get; }
    public AudioEndpointVolumeRange VolumeRange { get; }
    public AudioEndpointVolumeStepInformation StepInformation { get; }
    public AudioEndpointVolumeChannels Channels { get; }

    public float MasterVolumeLevel
    {
        get
        {
            return this._masterVolumeLevel;
        }
        set
        {
            Marshal.ThrowExceptionForHR(this.realAudioEndPointVolume.SetMasterVolumeLevel(value, this.NotificationGuid));
        }
    }

    public float MasterVolumeLevelScalar
    {
        get
        {
            return this._masterVolumeLevelScalar;
        }
        set
        {
            Marshal.ThrowExceptionForHR(this.realAudioEndPointVolume.SetMasterVolumeLevelScalar(value, this.NotificationGuid));
        }
    }

    public bool Mute
    {
        get
        {
            return this._mute;
        }
        set
        {
            Marshal.ThrowExceptionForHR(this.realAudioEndPointVolume.SetMute(value, this.NotificationGuid));
        }
    }

    internal AudioEndpointVolume(MMDevice parent, IAudioEndpointVolume realAudioEndPointVolume)
    {
        this.parent = parent;
        this.realAudioEndPointVolume = realAudioEndPointVolume;

        Marshal.ThrowExceptionForHR(this.realAudioEndPointVolume.QueryHardwareSupport(out uint hardwareSupp));
        this.HardwareSupport = (EndpointHardwareSupport)hardwareSupp;
        this.VolumeRange = new AudioEndpointVolumeRange(this.realAudioEndPointVolume);
        this.StepInformation = new AudioEndpointVolumeStepInformation(this.realAudioEndPointVolume);
        this.Channels = new AudioEndpointVolumeChannels(this.realAudioEndPointVolume);

        this.GetMasterVolumeLevel();
        this.GetMasterVolumeLevelScalar();
        this.GetMute();

        this.audioEndpointVolumeCallback = new AudioEndpointVolumeCallback();
        this.audioEndpointVolumeCallback.Notify += this.OnNotify;
        Marshal.ThrowExceptionForHR(this.realAudioEndPointVolume.RegisterControlChangeNotify(this.audioEndpointVolumeCallback));
    }

    public float GetMasterVolumeLevel()
    {
        Marshal.ThrowExceptionForHR(this.realAudioEndPointVolume.GetMasterVolumeLevel(out float level));
        this._masterVolumeLevel = level;
        return level;
    }

    public float GetMasterVolumeLevelScalar()
    {
        Marshal.ThrowExceptionForHR(this.realAudioEndPointVolume.GetMasterVolumeLevelScalar(out float level));
        this._masterVolumeLevelScalar = level;
        return level;
    }

    public bool GetMute()
    {
        Marshal.ThrowExceptionForHR(this.realAudioEndPointVolume.GetMute(out bool mute));
        this._mute = mute;
        return mute;
    }

    public void VolumeStepUp()
    {
        Marshal.ThrowExceptionForHR(this.realAudioEndPointVolume.VolumeStepUp(this.NotificationGuid));
    }

    public void VolumeStepDown()
    {
        Marshal.ThrowExceptionForHR(this.realAudioEndPointVolume.VolumeStepDown(this.NotificationGuid));
    }

    private void OnNotify(object sender, IntPtr notifyData)
    {
        AudioVolumeNotificationData data = Marshal.PtrToStructure<AudioVolumeNotificationData>(notifyData);

        // Determine offset in structure of the first float.
        IntPtr offset = Marshal.OffsetOf<AudioVolumeNotificationData>("ChannelVolume");
        // Determine offset in memory of the first float.
        IntPtr firstFloatPtr = (IntPtr)((long)notifyData + (long)offset);
        // Determine the unmanaged size of float.
        long sizeOfFloat = Marshal.SizeOf<float>();
        // Read all floats from memory.
        float[] volumeData = new float[data.Channels];
        for (int i = 0; i < data.Channels; i++)
        {
            volumeData[i] = Marshal.PtrToStructure<float>(firstFloatPtr);
            firstFloatPtr = (IntPtr)((long)firstFloatPtr + sizeOfFloat);
        }

        this.GetMasterVolumeLevel();
        this._masterVolumeLevelScalar = data.MasterVolume;
        this._mute = data.Muted;

        AudioVolumeNotificationEventArgs notificationData = new AudioVolumeNotificationEventArgs(data.EventContext, data.Muted, data.MasterVolume, (uint)volumeData.Length, volumeData);
        this.VolumeNotification?.Invoke(this.parent, notificationData);
    }

    public void Dispose()
    {
        Marshal.ThrowExceptionForHR(this.realAudioEndPointVolume.UnregisterControlChangeNotify(this.audioEndpointVolumeCallback));
        //Marshal.ReleaseComObject(this.realAudioEndPointVolume);
        GC.SuppressFinalize(this);
    }

    ~AudioEndpointVolume()
    {
        this.Dispose();
    }
}
