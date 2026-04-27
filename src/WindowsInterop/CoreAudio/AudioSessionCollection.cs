namespace WindowsInterop.CoreAudio;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class AudioSessionCollection : IEnumerable<AudioSessionControl>, IDisposable
{
    public event EventHandler<Exception> SessionLoadError;

    private readonly IAudioSessionEnumerator audioSessionEnumerator;

    public int Count
    {
        get
        {
            Marshal.ThrowExceptionForHR(this.audioSessionEnumerator.GetCount(out int sessionCount));
            return sessionCount;
        }
    }

    public AudioSessionControl this[int index]
    {
        get
        {
            Marshal.ThrowExceptionForHR(this.audioSessionEnumerator.GetSession(index, out IAudioSessionControl session));
            return new AudioSessionControl(session);
        }
    }

    internal AudioSessionCollection(IAudioSessionEnumerator realEnumerator)
    {
        this.audioSessionEnumerator = realEnumerator;
    }

    public IEnumerator<AudioSessionControl> GetEnumerator()
    {
        int count = this.Count;
        for (int index = 0; index < count; index++)
        {
            AudioSessionControl session = null;
            try
            {
                session = this[index];
            }
            catch (Exception ex)
            {
                // Skip sessions that fail to initialize (e.g. access denied, sandboxed UWP processes)
                SessionLoadError?.Invoke(this, ex);
            }
            if (session != null)
            {
                yield return session;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public void Dispose()
    {
        Marshal.ReleaseComObject(this.audioSessionEnumerator);
        GC.SuppressFinalize(this);
    }

    ~AudioSessionCollection()
    {
        this.Dispose();
    }
}
