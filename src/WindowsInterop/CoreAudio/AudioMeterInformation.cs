namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

public class AudioMeterInformation : IDisposable
{
    private readonly IAudioMeterInformation realAudioMeterInformation;

    public AudioMeterInformationChannels PeakValues { get; }

    public EndpointHardwareSupport HardwareSupport { get; }

    public float MasterPeakValue
    {
        get
        {
            Marshal.ThrowExceptionForHR(this.realAudioMeterInformation.GetPeakValue(out float peak));
            return peak;
        }
    }

    internal AudioMeterInformation(IAudioMeterInformation realInterface)
    {
        this.realAudioMeterInformation = realInterface;
        this.PeakValues = new AudioMeterInformationChannels(this.realAudioMeterInformation);
        Marshal.ThrowExceptionForHR(this.realAudioMeterInformation.QueryHardwareSupport(out int hardwareSupp));
        this.HardwareSupport = (EndpointHardwareSupport)hardwareSupp;
    }

    public void Dispose()
    {
        //Marshal.ReleaseComObject(this.realAudioMeterInformation);
        GC.SuppressFinalize(this);
    }

    ~AudioMeterInformation()
    {
        this.Dispose();
    }
}
