namespace Loupedeck.AudioControlPlugin;

internal static class CaptureDevice
{
    public const string UNMUTED_RESOURCE_PATH = "microphone-unmuted.png";
    public const string MUTED_RESOURCE_PATH = "microphone-muted.png";

    public static string GetUnmutedIconPath(float volume)
    {
        return UNMUTED_RESOURCE_PATH;
    }

    public static string GetMutedIconPath()
    {
        return MUTED_RESOURCE_PATH;
    }
}
