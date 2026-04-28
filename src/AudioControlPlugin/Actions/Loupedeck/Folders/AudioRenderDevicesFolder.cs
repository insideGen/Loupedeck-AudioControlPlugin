namespace Loupedeck.AudioControlPlugin.Loupedeck;

using WindowsInterop.CoreAudio;

internal class AudioRenderDevicesFolder : Folder
{
    public const string ICON_RESOURCE_PATH = "speaker-thin.png";

    public const string DISPLAY_NAME = "Render devices";
    public const string DESCRIPTION = "";
    public const string GROUP_NAME = "";

    public AudioRenderDevicesFolder() : base(DISPLAY_NAME, DESCRIPTION, GROUP_NAME, DeviceType.LoupedeckCtFamily)
    {
        base.HomePage = new AudioDevicesPage(this, DataFlow.Render);
    }

    public override BitmapImage GetButtonImage(PluginImageSize imageSize)
    {
        return PluginImage.DrawFolderIconImage(true, ICON_RESOURCE_PATH, imageSize);
    }
}
