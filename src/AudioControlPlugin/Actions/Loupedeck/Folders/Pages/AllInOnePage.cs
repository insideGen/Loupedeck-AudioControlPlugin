namespace Loupedeck.AudioControlPlugin.Loupedeck;

using System.Collections.Generic;
using System.Drawing;

using WindowsInterop.CoreAudio;

internal class AllInOnePage : FolderPage
{
    public const string MICROPHONE_RESOURCE_PATH = "microphone-unmuted.png";
    public const string SPEAKER_RESOURCE_PATH = "speaker-unmuted-2.png";
    public const string APPLICATION_RESOURCE_PATH = "application.png";
    public const string MULTIMEDIA_RESOURCE_PATH = "all-in-one.png";
    public const string COMMUNICATIONS_RESOURCE_PATH = "all-in-one.png";
    public const string SETTINGS_RESOURCE_PATH = "settings.png";

    public AllInOnePage(Folder parent) : base(parent)
    {
    }

    public override IEnumerable<string> GetButtonPressActionNames(DeviceType deviceType)
    {
        return new string[] { "Capture", "Render", "Session", "Multimedia", "Communications", "Settings" };
    }

    public override BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
    {
        string categoryName = string.Empty;
        string iconPath = string.Empty;
        if (actionParameter == "Capture")
        {
            categoryName = "Capture";
            iconPath = MICROPHONE_RESOURCE_PATH;
        }
        else if (actionParameter == "Render")
        {
            categoryName = "Render";
            iconPath = SPEAKER_RESOURCE_PATH;
        }
        else if (actionParameter == "Session")
        {
            categoryName = "Application";
            iconPath = APPLICATION_RESOURCE_PATH;
        }
        else if (actionParameter == "Multimedia")
        {
            categoryName = "Media";
            iconPath = MULTIMEDIA_RESOURCE_PATH;
        }
        else if (actionParameter == "Communications")
        {
            categoryName = "Comm.";
            iconPath = COMMUNICATIONS_RESOURCE_PATH;
        }
        else if (actionParameter == "Settings")
        {
            categoryName = "Settings";
            iconPath = SETTINGS_RESOURCE_PATH;
        }
        using (Bitmap icon = PluginImage.ReadBitmap(iconPath))
        using (Bitmap image = new Bitmap(80, 80))
        using (Graphics graphics = Graphics.FromImage(image))
        using (Font valueFont = new Font("Calibri", 11, FontStyle.Bold))
        using (Brush whiteBrush = new SolidBrush(Color.White.BlueLightFilter()))
        using (Pen whitePen = new Pen(whiteBrush))
        using (StringFormat format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
        {
            graphics.Clear(Color.Black);

            int margin1 = 12;
            int margin2 = -2;

            icon.Recolor(Color.White.BlueLightFilter());
            graphics.DrawImage(icon, (image.Width - icon.Width) / 2, margin1, icon.Width, icon.Height);

            RectangleF valueRectangle = new RectangleF(0, margin1 + icon.Height + margin2, image.Width, image.Height - (margin1 + icon.Height + margin2));
            graphics.DrawString(categoryName, valueFont, whiteBrush, valueRectangle, format);

            return PluginImage.ToBitmapImage(image);
        }
    }

    public override BitmapImage GetAdjustmentImage(string actionParameter, PluginImageSize imageSize)
    {
        return PluginImage.DrawBlackImage(imageSize);
    }

    public override bool ProcessTouchEvent(string actionParameter, DeviceTouchEvent touchEvent)
    {
        if (touchEvent.EventType == DeviceTouchEventType.Tap)
        {
            if (actionParameter == "Capture")
            {
                this.NavigateTo(new AudioDevicesPage(this.Folder, DataFlow.Capture));
            }
            else if (actionParameter == "Render")
            {
                this.NavigateTo(new AudioDevicesPage(this.Folder, DataFlow.Render));
            }
            else if (actionParameter == "Session")
            {
                this.NavigateTo(new AudioSessionsPage(this.Folder));
            }
            else if (actionParameter == "Multimedia")
            {
                this.NavigateTo(new AudioSessionsPage(this.Folder, AudioSessionsPage.DefaultType.Multimedia));
            }
            else if (actionParameter == "Communications")
            {
                this.NavigateTo(new AudioSessionsPage(this.Folder, AudioSessionsPage.DefaultType.Communications));
            }
            else if (actionParameter == "Settings")
            {
                this.NavigateTo(new SettingsPage(this.Folder));
            }
        }
        return false;
    }
}
