namespace Loupedeck.AudioControlPlugin.Loupedeck;

using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

internal class SettingsPage : FolderPage
{
    private enum Command
    {
#if DEBUG
        PluginDataDirectory = 1,
        BlueLightFilterEnabled = 2,
        IsWindowsIconStyle = 3,
        PreferDecibels = 4,
        PeakMeterEnabled = 5,
        FPS = 6,
        SaveImageOnDisk = 7,
        ClearAllSettings = 8
#else
        PluginDataDirectory = -1,
        BlueLightFilterEnabled = 2,
        IsWindowsIconStyle = 3,
        PreferDecibels = 4,
        PeakMeterEnabled = 5,
        FPS = 6,
        SaveImageOnDisk = -7,
        ClearAllSettings = -8
#endif
    }

    private readonly int[] fpsValues = { 10, 20, 30 };

    public SettingsPage(Folder parent) : base(parent)
    {
    }

    public override IEnumerable<string> GetButtonPressActionNames(DeviceType deviceType)
    {
        List<string> actionNames = new List<string>();
        actionNames.AddRange(((IEnumerable<Command>)Enum.GetValues(typeof(Command))).Where(x => x >= 0).Select(x => x.ToString()));
        return actionNames;
    }

    public override BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
    {
        string settingString = string.Empty;
        string valueString = string.Empty;

        if (actionParameter == Command.PluginDataDirectory.ToString())
        {
            valueString = "Open plugin data directory";
        }
        else if (actionParameter == Command.BlueLightFilterEnabled.ToString())
        {
            settingString = "Blue light filter";
            valueString = PluginSettings.BlueLightFilterEnabled ? "Yes" : "No";
        }
        else if (actionParameter == Command.IsWindowsIconStyle.ToString())
        {
            settingString = "Device icon style";
            valueString = PluginSettings.IsWindowsIconStyle ? "Windows" : "Custom";
        }
        else if (actionParameter == Command.PreferDecibels.ToString())
        {
            settingString = "Prefer decibels";
            valueString = PluginSettings.PreferDecibels ? "Yes" : "No";
        }
        else if (actionParameter == Command.PeakMeterEnabled.ToString())
        {
            settingString = "Peak meter";
            valueString = PluginSettings.PeakMeterEnabled ? "Yes" : "No";
        }
        else if (actionParameter == Command.FPS.ToString())
        {
            settingString = "FPS";
            valueString = PluginSettings.FPS.ToString();
        }
        else if (actionParameter == Command.SaveImageOnDisk.ToString())
        {
            settingString = "Save image on disk";
            valueString = PluginSettings.SaveImageOnDisk ? "On" : "Off";
        }
        else if (actionParameter == Command.ClearAllSettings.ToString())
        {
            valueString = "Clear all settings";
        }

        using (Bitmap image = new Bitmap(80, 80))
        using (Graphics graphics = Graphics.FromImage(image))
        using (Font settingFont = new Font("Calibri", 11, FontStyle.Regular))
        using (Font valueFont = new Font("Calibri", 11, FontStyle.Bold))
        using (Brush whiteBrush = new SolidBrush(Color.White.BlueLightFilter()))
        using (Pen whitePen = new Pen(whiteBrush))
        using (StringFormat format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
        {
            graphics.Clear(Color.Black);

            RectangleF imageRectangle = new RectangleF(0, 0, image.Width, image.Height);

            SizeF settingSize = graphics.MeasureString(settingString, settingFont, imageRectangle.Size);
            SizeF valueSize = graphics.MeasureString(valueString, valueFont, imageRectangle.Size);

            PointF settingPoint = new PointF((imageRectangle.Width - settingSize.Width) / 2, (imageRectangle.Height - settingSize.Height - valueSize.Height) / 2 + 1);
            PointF valuePoint = new PointF((imageRectangle.Width - valueSize.Width) / 2, (imageRectangle.Height - settingSize.Height - valueSize.Height) / 2 + settingSize.Height + 1);

            RectangleF settingRectangle = new RectangleF(settingPoint, settingSize);
            RectangleF valueRectangle = new RectangleF(valuePoint, valueSize);

            graphics.DrawString(settingString, settingFont, whiteBrush, settingRectangle, format);
            graphics.DrawString(valueString, valueFont, whiteBrush, valueRectangle, format);

            return PluginImage.ToBitmapImage(image);
        }
    }

    public override BitmapImage GetAdjustmentImage(string actionParameter, PluginImageSize imageSize)
    {
        return PluginImage.DrawBlackImage(imageSize);
    }

    public override bool ProcessTouchEvent(string actionParameter, DeviceTouchEvent touchEvent)
    {
        if (touchEvent.EventType == DeviceTouchEventType.Press)
        {
            if (actionParameter == Command.PluginDataDirectory.ToString())
            {
                if (IoHelpers.EnsureDirectoryExists(PluginData.Directory))
                {
                    Process.Start("explorer.exe", PluginData.Directory);
                }
            }
            else if (actionParameter == Command.BlueLightFilterEnabled.ToString())
            {
                PluginSettings.BlueLightFilterEnabled = !PluginSettings.BlueLightFilterEnabled;
            }
            else if (actionParameter == Command.IsWindowsIconStyle.ToString())
            {
                PluginSettings.IsWindowsIconStyle = !PluginSettings.IsWindowsIconStyle;
            }
            else if (actionParameter == Command.PreferDecibels.ToString())
            {
                PluginSettings.PreferDecibels = !PluginSettings.PreferDecibels;
            }
            else if (actionParameter == Command.PeakMeterEnabled.ToString())
            {
                PluginSettings.PeakMeterEnabled = !PluginSettings.PeakMeterEnabled;
            }
            else if (actionParameter == Command.FPS.ToString())
            {
                int index = Array.IndexOf(this.fpsValues, PluginSettings.FPS);
                int nextIndex = (index + 1) % this.fpsValues.Length;
                PluginSettings.FPS = this.fpsValues[nextIndex];
                AudioControlPlugin.RefreshTimer.Interval = 1000.0 / PluginSettings.FPS;
            }
            else if (actionParameter == Command.SaveImageOnDisk.ToString())
            {
                PluginSettings.SaveImageOnDisk = !PluginSettings.SaveImageOnDisk;
            }
            else if (actionParameter == Command.ClearAllSettings.ToString())
            {
                string[] settings = base.Folder.Plugin.ListPluginSettings();
                foreach (string setting in settings)
                {
                    base.Folder.Plugin.DeletePluginSetting(setting);
                }
            }
            base.CommandImageChanged(actionParameter);
        }
        return false;
    }
}
