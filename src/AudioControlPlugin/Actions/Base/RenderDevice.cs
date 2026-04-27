namespace Loupedeck.AudioControlPlugin;

internal static class RenderDevice
{
    public const string UNMUTED_0_RESOURCE_PATH = "speaker-unmuted-0.png";
    public const string UNMUTED_1_RESOURCE_PATH = "speaker-unmuted-1.png";
    public const string UNMUTED_2_RESOURCE_PATH = "speaker-unmuted-2.png";
    public const string UNMUTED_3_RESOURCE_PATH = "speaker-unmuted-3.png";
    public const string MUTED_RESOURCE_PATH = "speaker-muted.png";

    public static string GetUnmutedIconPath(float volume)
    {
        if (volume <= 0.0f)
        {
            return UNMUTED_0_RESOURCE_PATH;
        }
        else if (volume < 0.33f)
        {
            return UNMUTED_1_RESOURCE_PATH;
        }
        else if (volume < 0.66f)
        {
            return UNMUTED_2_RESOURCE_PATH;
        }
        else
        {
            return UNMUTED_3_RESOURCE_PATH;
        }
    }

    public static string GetMutedIconPath()
    {
        return MUTED_RESOURCE_PATH;
    }
}
