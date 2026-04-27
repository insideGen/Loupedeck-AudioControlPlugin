namespace WindowsInterop.CoreAudio;

using System.Runtime.InteropServices;

public class Part
{
    private static Guid IID_IAudioVolumeLevel = new Guid("7FB7B48F-531D-44A2-BCB3-5AD5A134B3DC");
    private static Guid IID_IAudioMute = new Guid("DF45AEEA-B74A-4B6B-AFAD-2366B6AA012E");
    private static Guid IID_IAudioEndpointVolume = new Guid("5CDF2C82-841E-4546-9722-0CF74078229A");
    private static Guid IID_IKsJackDescription = new Guid("4509F757-2D46-4637-8E62-CE7DB944F57B");

    private const int E_NOTFOUND = unchecked((int)0x80070490);

    private readonly IPart partInterface;

    private DeviceTopology deviceTopology;

    internal Part(IPart part)
    {
        this.partInterface = part;
    }

    public string Name
    {
        get
        {
            this.partInterface.GetName(out string name);
            return name;
        }
    }

    public uint LocalId
    {
        get
        {
            this.partInterface.GetLocalId(out uint id);
            return id;
        }
    }

    public string GlobalId
    {
        get
        {
            this.partInterface.GetGlobalId(out string id);
            return id;
        }
    }

    public PartTypeEnum PartType
    {
        get
        {
            this.partInterface.GetPartType(out PartTypeEnum partType);
            return partType;
        }
    }

    public Guid GetSubType
    {
        get
        {
            this.partInterface.GetSubType(out Guid subType);
            return subType;
        }
    }

    public uint ControlInterfaceCount
    {
        get
        {
            this.partInterface.GetControlInterfaceCount(out uint count);
            return count;
        }
    }

    public IControlInterface GetControlInterface(uint index)
    {
        this.partInterface.GetControlInterface(index, out IControlInterface controlInterface);
        return controlInterface;
    }

    public PartsList PartsIncoming
    {
        get
        {
            int hr = this.partInterface.EnumPartsIncoming(out IPartsList parts);
            return hr == 0 ? new PartsList(parts) : hr == E_NOTFOUND ? new PartsList(null) : throw new COMException(nameof(IPart.EnumPartsIncoming), hr);
        }
    }

    public PartsList PartsOutgoing
    {
        get
        {
            int hr = this.partInterface.EnumPartsOutgoing(out IPartsList parts);
            return hr == 0 ? new PartsList(parts) : hr == E_NOTFOUND ? new PartsList(null) : throw new COMException(nameof(IPart.EnumPartsOutgoing), hr);
        }
    }

    public DeviceTopology DeviceTopology
    {
        get
        {
            if (this.deviceTopology == null)
            {
                this.GetDeviceTopology();
            }
            return this.deviceTopology;
        }
    }

    public AudioVolumeLevel AudioVolumeLevel
    {
        get
        {
            int hr = this.partInterface.Activate(ClsCtx.ALL, ref Part.IID_IAudioVolumeLevel, out object interfacePointer);
            return hr == 0 ? new AudioVolumeLevel(interfacePointer as IAudioVolumeLevel) : null;
        }
    }

    public AudioMute AudioMute
    {
        get
        {
            int hr = this.partInterface.Activate(ClsCtx.ALL, ref Part.IID_IAudioMute, out object interfacePointer);
            return hr == 0 ? new AudioMute(interfacePointer as IAudioMute) : null;
        }
    }

    public KsJackDescription JackDescription
    {
        get
        {
            int hr = this.partInterface.Activate(ClsCtx.ALL, ref Part.IID_IKsJackDescription, out object interfacePointer);
            return hr == 0 ? new KsJackDescription(interfacePointer as IKsJackDescription) : null;
        }
    }

    private void GetDeviceTopology()
    {
        Marshal.ThrowExceptionForHR(this.partInterface.GetTopologyObject(out object topologyObject));
        this.deviceTopology = new DeviceTopology(topologyObject as IDeviceTopology);
    }
}
