namespace Loupedeck.AudioControlPlugin.Loupedeck;

internal class AudioCommunicationFolder : Folder
{
    public const string ICON_RESOURCE_PATH = "speaker-thin.png";

    public const string DISPLAY_NAME = "Communication devices";
    public const string DESCRIPTION = "";
    public const string GROUP_NAME = "";

    public AudioCommunicationFolder() : base(DISPLAY_NAME, DESCRIPTION, GROUP_NAME, DeviceType.LoupedeckCtFamily)
    {
        base.HomePage = new AudioSessionsPage(this, AudioSessionsPage.DefaultType.Communications);
    }

    public override BitmapImage GetButtonImage(PluginImageSize imageSize)
    {
        return PluginImage.DrawFolderTextImage(true, "Comm.", imageSize);
    }
}
