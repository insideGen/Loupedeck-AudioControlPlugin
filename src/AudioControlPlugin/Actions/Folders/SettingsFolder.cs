namespace Loupedeck.AudioControlPlugin;

internal class SettingsFolder : Folder
{
    public const string SETTINGS_THIN_RESOURCE_PATH = "settings-thin.png";

    public const string DISPLAY_NAME = "Settings";
    public const string DESCRIPTION = "";
    public const string GROUP_NAME = "";

    public SettingsFolder() : base(DISPLAY_NAME, DESCRIPTION, GROUP_NAME)
    {
        base.HomePage = new SettingsPage(this);
    }

    public override BitmapImage GetButtonImage(PluginImageSize imageSize)
    {
        return PluginImage.DrawFolderIconImage(true, SETTINGS_THIN_RESOURCE_PATH, imageSize);
    }
}
