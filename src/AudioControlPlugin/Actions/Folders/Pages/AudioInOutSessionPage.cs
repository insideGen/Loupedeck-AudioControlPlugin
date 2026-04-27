namespace Loupedeck.AudioControlPlugin;

using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using WindowsInterop.CoreAudio;

internal class AudioInOutSessionPage : FolderPage
{
    private IAudioControlSession Session { get; }

    public AudioInOutSessionPage(Folder parent, IAudioControlSession session) : base(parent)
    {
        this.Session = session;
    }

    public override void Enter()
    {
        foreach (string actionParameter in this.GetButtonPressActionNames(DeviceType.All))
        {
            base.CommandImageChanged(actionParameter);
        }
    }

    public override IEnumerable<string> GetButtonPressActionNames(DeviceType deviceType)
    {
        return new string[] { "Capture", "Render" };
    }

    public override BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
    {
        string deviceName = string.Empty;
        string iconPath = string.Empty;
        if (actionParameter == "Capture")
        {
            iconPath = AudioCaptureDevicesFolder.ICON_RESOURCE_PATH;
            try
            {
                string deviceId = AudioControl.MMAudio.AudioPolicyConfig.GetPersistedDefaultAudioEndpoint((uint)this.Session.ProcessId, DataFlow.Capture, Role.Multimedia | Role.Console);
                if (string.IsNullOrEmpty(deviceId))
                {
                    deviceName = "Default";
                }
                else if (AudioControl.MMAudio.CaptureDevices.FirstOrDefault(x => x.Id == deviceId) is MMDevice device)
                {
                    deviceName = $"{device.DeviceDescription}";
                }
            }
            catch
            {

            }
        }
        else if (actionParameter == "Render")
        {
            iconPath = AudioRenderDevicesFolder.ICON_RESOURCE_PATH;
            try
            {
                string deviceId = AudioControl.MMAudio.AudioPolicyConfig.GetPersistedDefaultAudioEndpoint((uint)this.Session.ProcessId, DataFlow.Render, Role.Multimedia | Role.Console);
                if (string.IsNullOrEmpty(deviceId))
                {
                    deviceName = "Default";
                }
                else if (AudioControl.MMAudio.RenderDevices.FirstOrDefault(x => x.Id == deviceId) is MMDevice device)
                {
                    deviceName = $"{device.DeviceDescription}";
                }
            }
            catch
            {

            }
        }
        PluginImage.GetImageSize(imageSize, out int width, out int height);
        using (Bitmap icon = PluginImage.ReadBitmap(iconPath))
        using (Bitmap image = new Bitmap(width, height))
        using (Graphics graphics = Graphics.FromImage(image))
        using (Font valueFont = new Font("Calibri", 11, FontStyle.Bold))
        using (Brush whiteBrush = new SolidBrush(Color.White.BlueLightFilter()))
        using (Pen whitePen = new Pen(whiteBrush))
        using (StringFormat format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
        {
            graphics.Clear(Color.Black);

            int margin1 = 10;
            int margin2 = 0;

            icon.Recolor(Color.White.BlueLightFilter());
            graphics.DrawImage(icon, (image.Width - icon.Width) / 2, margin1, icon.Width, icon.Height);

            RectangleF valueRectangle = new RectangleF(0, margin1 + icon.Height + margin2, image.Width, image.Height - (margin1 + icon.Height + margin2));
            graphics.DrawString(deviceName, valueFont, whiteBrush, valueRectangle, format);

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
                this.NavigateTo(new AudioInOutDeviceSelectorPage(base.Folder, this.Session, DataFlow.Capture));
            }
            else if (actionParameter == "Render")
            {
                this.NavigateTo(new AudioInOutDeviceSelectorPage(base.Folder, this.Session, DataFlow.Render));
            }
        }
        return false;
    }
}
