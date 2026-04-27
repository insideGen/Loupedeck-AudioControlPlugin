namespace Loupedeck.AudioControlPlugin;

using WindowsInterop.CoreAudio;

internal class AudioCaptureDevicesFolder : Folder
{
    public const string ICON_RESOURCE_PATH = "microphone-thin.png";

    public const string DISPLAY_NAME = "Capture devices";
    public const string DESCRIPTION = "";
    public const string GROUP_NAME = "";

    public AudioCaptureDevicesFolder() : base(DISPLAY_NAME, DESCRIPTION, GROUP_NAME)
    {
        base.HomePage = new AudioDevicesPage(this, DataFlow.Capture);
    }

    public override BitmapImage GetButtonImage(PluginImageSize imageSize)
    {
        return PluginImage.DrawFolderIconImage(true, ICON_RESOURCE_PATH, imageSize);
    }
}
