namespace Loupedeck.AudioControlPlugin;

using System.Drawing;
using System.Linq;

using WindowsInterop.CoreAudio;

internal class MuteAllDevicesCommand : PluginDynamicCommand
{
    public MuteAllDevicesCommand() : base()
    {
        base.AddParameter(DataFlow.Capture.ToLower(), "Mute/unmute all capture devices", "Group action");
        base.AddParameter(DataFlow.Render.ToLower(), "Mute/unmute all render devices", "Group action");
        base.IsWidget = true;
    }

    protected override BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
    {
        DataFlow dataFlow = (DataFlow)Enum.Parse(typeof(DataFlow), actionParameter, true);
        string displayName = "Un|Mute all";
        string iconPath = "";
        if (dataFlow == DataFlow.Capture)
        {
            iconPath = CaptureDevice.MUTED_RESOURCE_PATH;
        }
        else if (dataFlow == DataFlow.Render)
        {
            iconPath = RenderDevice.MUTED_RESOURCE_PATH;
        }
        using (Bitmap image = new Bitmap(80, 80))
        using (Graphics graphics = Graphics.FromImage(image))
        using (Font calibri10Font = new Font("Calibri", 10, FontStyle.Regular))
        using (Brush orangeBrush = new SolidBrush(Color.Orange))
        using (Brush whiteBrush = new SolidBrush(Color.White.BlueLightFilter()))
        using (StringFormat format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
        {
            graphics.Clear(Color.Black);
            graphics.DrawString(displayName, calibri10Font, whiteBrush, new RectangleF(2, 14, 76, 24), format);
            using (Bitmap icon = PluginImage.ReadBitmap(iconPath))
            {
                icon.Recolor(Color.White.BlueLightFilter());
                graphics.DrawImage(icon, (image.Width - icon.Width) / 2, 37, icon.Width, icon.Height);
            }
            return PluginImage.ToBitmapImage(image);
        }
    }

    protected override bool ProcessTouchEvent(string actionParameter, DeviceTouchEvent touchEvent)
    {
        DataFlow dataFlow = (DataFlow)Enum.Parse(typeof(DataFlow), actionParameter, true);
        foreach (MMDevice device in AudioControl.MMAudio.Devices.Where(x => x.State == DeviceState.Active && x.DataFlow == dataFlow))
        {
            if (touchEvent.EventType == DeviceTouchEventType.Tap)
            {
                device.AudioEndpointVolume.Mute = true;
            }
            else if (touchEvent.EventType == DeviceTouchEventType.LongPress)
            {
                device.AudioEndpointVolume.Mute = false;
            }
        }
        return true;
    }
}
