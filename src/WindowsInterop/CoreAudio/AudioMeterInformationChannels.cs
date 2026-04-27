namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

public class AudioMeterInformationChannels
{
    private readonly IAudioMeterInformation audioMeterInformation;

    public int Count
    {
        get
        {
            Marshal.ThrowExceptionForHR(this.audioMeterInformation.GetMeteringChannelCount(out int channelCount));
            return channelCount;
        }
    }

    public float this[int index]
    {
        get
        {
            float[] values = this.ToArray();
            if (index >= values.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"Peak index cannot be greater than number of channels ({values.Length})");
            }
            return values[index];
        }
    }

    public AudioMeterInformationChannels(IAudioMeterInformation parent)
    {
        this.audioMeterInformation = parent;
    }

    public float[] ToArray()
    {
        int channels = this.Count;
        float[] peakValues = new float[channels];

        if (channels > 0)
        {
            //IntPtr arrayPtr = Marshal.AllocHGlobal(channels * 4); // 4 bytes in float
            //Marshal.ThrowExceptionForHR(this.audioMeterInformation.GetChannelsPeakValues(channels, arrayPtr));
            //Marshal.Copy(arrayPtr, peakValues, 0, channels);
            //Marshal.FreeHGlobal(arrayPtr);

            GCHandle Params = GCHandle.Alloc(peakValues, GCHandleType.Pinned);
            try
            {
                Marshal.ThrowExceptionForHR(this.audioMeterInformation.GetChannelsPeakValues(channels, Params.AddrOfPinnedObject()));
            }
            catch
            {
            }
            Params.Free();
        }

        return peakValues;
    }
}
