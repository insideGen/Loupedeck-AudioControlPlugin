namespace WindowsInterop.CoreAudio;

public class PartsList
{
    private readonly IPartsList partsListInterface;

    internal PartsList(IPartsList partsList)
    {
        this.partsListInterface = partsList;
    }

    public uint Count
    {
        get
        {
            uint count = 0;
            this.partsListInterface?.GetCount(out count);
            return count;
        }
    }

    public Part this[uint index]
    {
        get
        {
            if (this.partsListInterface == null)
            {
                throw new IndexOutOfRangeException();
            }
            this.partsListInterface.GetPart(index, out IPart part);
            return new Part(part);
        }
    }
}
