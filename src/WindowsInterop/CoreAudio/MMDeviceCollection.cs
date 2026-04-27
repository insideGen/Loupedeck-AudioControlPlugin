namespace WindowsInterop.CoreAudio;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MMDeviceCollection : IEnumerable<MMDevice>, IDisposable
{
    private readonly IMMDeviceCollection realMMDeviceCollection;

    public int Count
    {
        get
        {
            Marshal.ThrowExceptionForHR(this.realMMDeviceCollection.GetCount(out int count));
            return count;
        }
    }

    public MMDevice this[int index]
    {
        get
        {
            this.realMMDeviceCollection.Item(index, out IMMDevice device);
            return new MMDevice(device);
        }
    }

    internal MMDeviceCollection(IMMDeviceCollection parent)
    {
        this.realMMDeviceCollection = parent;
    }

    public IEnumerator<MMDevice> GetEnumerator()
    {
        int count = this.Count;
        for (int index = 0; index < count; index++)
        {
            yield return this[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Marshal.ReleaseComObject(this.realMMDeviceCollection);
    }

    ~MMDeviceCollection()
    {
        this.Dispose();
    }
}
