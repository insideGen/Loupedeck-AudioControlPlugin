namespace WindowsInterop.CoreAudio;

public class KsJackDescription
{
    private readonly IKsJackDescription ksJackDescriptionInterface;

    internal KsJackDescription(IKsJackDescription ksJackDescription)
    {
        this.ksJackDescriptionInterface = ksJackDescription;
    }

    public uint Count
    {
        get
        {
            this.ksJackDescriptionInterface.GetJackCount(out uint count);
            return count;
        }
    }

    public string this[uint index]
    {
        get
        {
            this.ksJackDescriptionInterface.GetJackDescription(index, out string description);
            return description;
        }
    }
}
