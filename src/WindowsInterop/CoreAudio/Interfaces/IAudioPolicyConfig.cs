namespace WindowsInterop.CoreAudio;

public interface IAudioPolicyConfig : IDisposable
{
    void SetPersistedDefaultAudioEndpoint(uint processId, DataFlow flow, Role role, string deviceId);
    string GetPersistedDefaultAudioEndpoint(uint processId, DataFlow flow, Role role);
    void ClearAllPersistedApplicationDefaultEndpoints();
}
