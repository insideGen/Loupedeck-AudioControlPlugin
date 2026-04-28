namespace Loupedeck.AudioControlPlugin.Loupedeck;

internal class AudioRenderSessionsFolder : Folder
{
    public const string ICON_RESOURCE_PATH = "application-thin.png";

    public const string DISPLAY_NAME = "Applications";
    public const string DESCRIPTION = "";
    public const string GROUP_NAME = "";

    public AudioRenderSessionsFolder() : base(DISPLAY_NAME, DESCRIPTION, GROUP_NAME, DeviceType.LoupedeckCtFamily)
    {
        base.HomePage = new AudioSessionsPage(this);
    }

    public override BitmapImage GetButtonImage(PluginImageSize imageSize)
    {
        return PluginImage.DrawFolderIconImage(true, ICON_RESOURCE_PATH, imageSize);
    }
}
