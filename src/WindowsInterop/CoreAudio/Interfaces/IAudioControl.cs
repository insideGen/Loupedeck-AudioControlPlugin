namespace WindowsInterop.CoreAudio;

public interface IAudioControl
{
    string Id { get; }
    string DisplayName { get; }
    bool Muted { get; set; }
    float VolumeScalar { get; set; }
    float[] PeakValues { get; }
    string IconPath { get; }
}
