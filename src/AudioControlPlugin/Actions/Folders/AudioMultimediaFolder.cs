namespace Loupedeck.AudioControlPlugin;

internal class AudioMultimediaFolder : Folder
{
    public const string ICON_RESOURCE_PATH = "speaker-thin.png";

    public const string DISPLAY_NAME = "Multimedia devices";
    public const string DESCRIPTION = "";
    public const string GROUP_NAME = "";

    public AudioMultimediaFolder() : base(DISPLAY_NAME, DESCRIPTION, GROUP_NAME)
    {
        base.HomePage = new AudioSessionsPage(this, AudioSessionsPage.DefaultType.Multimedia);
    }

    public override BitmapImage GetButtonImage(PluginImageSize imageSize)
    {
        return PluginImage.DrawFolderTextImage(true, "Media", imageSize);
    }
}
