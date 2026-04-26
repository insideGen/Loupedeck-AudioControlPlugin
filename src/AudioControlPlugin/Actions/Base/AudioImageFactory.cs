namespace Loupedeck.AudioControlPlugin
{
    using System;
    using System.Collections.Concurrent;
    using System.Drawing;
    using System.Drawing.Imaging;

    internal class AudioImageFactory : IActionImageFactory<AudioImageData>
    {
        public const string CROSS_MUTE_RESOURCE_PATH = "cross-mute.png";

        private readonly object locker;

        private readonly ConcurrentDictionary<string, Bitmap> iconsDictionary;

        private readonly Bitmap crossMuteIcon;
        private readonly Bitmap imageWidth50;
        private readonly Graphics graphicsWidth50;
        private readonly Bitmap imageWidth80;
        private readonly Graphics graphicsWidth80;
        private readonly Font calibri7Font;
        private readonly Font calibri10Font;
        private readonly Pen orangePen;
        private readonly Brush whiteBrush;
        private readonly Brush greyBrush;
        private readonly Brush orangeBrush;
        private readonly Brush redBrush;
        private readonly StringFormat cFormat;

        public AudioImageFactory()
        {
            this.locker = new object();

            this.iconsDictionary = new ConcurrentDictionary<string, Bitmap>();

            this.crossMuteIcon = PluginImage.ReadBitmap(CROSS_MUTE_RESOURCE_PATH).Recolor(Color.Red);
            this.imageWidth50 = new Bitmap(50, 50);
            this.graphicsWidth50 = Graphics.FromImage(this.imageWidth50);
            this.imageWidth80 = new Bitmap(80, 80);
            this.graphicsWidth80 = Graphics.FromImage(this.imageWidth80);
            this.calibri7Font = new Font("Calibri", 7, FontStyle.Regular);
            this.calibri10Font = new Font("Calibri", 10, FontStyle.Regular);
            this.orangePen = new Pen(Color.Orange, 1);
            this.whiteBrush = new SolidBrush(Color.White.BlueLightFilter());
            this.greyBrush = new SolidBrush(Color.FromArgb(120, Color.White.BlueLightFilter()));
            this.orangeBrush = new SolidBrush(Color.Orange);
            this.redBrush = new SolidBrush(Color.FromArgb(255, Color.Red));
            this.cFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        }

        private static int GraduationRound(int max, float ratio)
        {
            float value = max * ratio;
            if (value <= 0)
            {
                return 0;
            }
            else if (value > 0 && value <= 1)
            {
                return 1;
            }
            else if (value >= (max - 1) && value < max)
            {
                return max - 1;
            }
            else if (value >= max)
            {
                return max;
            }
            else
            {
                return (int)Math.Round(value);
            }
        }

        private Bitmap CreateMutedIcon(string iconPath)
        {
            Bitmap icon = null;
            if (!string.IsNullOrEmpty(iconPath))
            {
                icon = PluginImage.GetIcon(iconPath).Scale(32, 32).BlueLightFilter().Desaturate(0.4f);
                if (icon != null)
                {
                    using (Graphics graphics = Graphics.FromImage(icon))
                    {
                        graphics.DrawImage(this.crossMuteIcon, 0, 0, this.crossMuteIcon.Width, this.crossMuteIcon.Height);
                    }
                }
            }
            return icon;
        }

        private Bitmap DrawWidth50(AudioImageData audioData, Image icon)
        {
            this.graphicsWidth50.Clear(audioData.Highlighted ? Color.FromArgb(45, 45, 45) : Color.Black);

            if (icon != null)
            {
                this.graphicsWidth50.DrawImage(icon, (this.imageWidth50.Width - icon.Width) / 2, 5, icon.Width, icon.Height);
            }

            if (audioData.IsActive)
            {
                string displayVolume = string.Empty;
                if (PluginSettings.PreferDecibels && audioData.HasDecibels)
                {
                    displayVolume = Math.Round(audioData.Volume, 0).ToString() + " dB";
                }
                else
                {
                    displayVolume = Math.Round(audioData.VolumeScalar * 100.0f, 0).ToString() + " %";
                }

                this.graphicsWidth50.DrawString(displayVolume, this.calibri10Font, this.whiteBrush, new Rectangle(0, this.imageWidth50.Height - 12, this.imageWidth50.Width, 12), this.cFormat);
            }
            else
            {
                this.graphicsWidth50.DrawString("No audio", this.calibri10Font, this.whiteBrush, new Rectangle(0, this.imageWidth50.Height - 12, this.imageWidth50.Width, 12), this.cFormat);
            }

            return this.imageWidth50;
        }

        private Bitmap DrawWidth80(AudioImageData audioData, Image icon)
        {
            const int slider_x = 66;
            const int slider_y = 22;
            const int slider_height = 50;
            const int peak_width = 2;

            int slider_thickness = (int)this.orangePen.Width;
            int slider_width = 2 * slider_thickness + 2 * peak_width;
            int slider_internalHeight = slider_height - 2 * slider_thickness;

            int volume_width = 2 * peak_width;
            int volume_height = GraduationRound(slider_internalHeight, audioData.VolumeScalar);
            int volume_x = slider_x + slider_thickness;
            int volume_y = slider_y + slider_thickness + slider_internalHeight - volume_height;

            int peakL_width = peak_width;
            int peakL_height = GraduationRound(volume_height, audioData.PeakL);
            int peakL_x = volume_x;
            int peakL_y = volume_y + volume_height - peakL_height;

            int peakR_width = peak_width;
            int peakR_height = GraduationRound(volume_height, audioData.PeakR);
            int peakR_x = volume_x + peak_width;
            int peakR_y = volume_y + volume_height - peakR_height;

            this.graphicsWidth80.Clear(audioData.Highlighted ? Color.FromArgb(45, 45, 45) : Color.Black);

            this.graphicsWidth80.DrawString(audioData.DisplayName, this.calibri10Font, this.whiteBrush, new Rectangle(0, 6, 80, 12), this.cFormat);

            if (icon != null)
            {
                int iconX = (this.imageWidth80.Width - icon.Width) / 2;
                int iconY = 22 + (32 - icon.Height) / 2;
                this.graphicsWidth80.DrawImage(icon, iconX, iconY, icon.Width, icon.Height);
            }

            if (audioData.IsActive)
            {
                this.graphicsWidth80.DrawRectangle(this.orangePen, slider_x, slider_y, slider_width - slider_thickness, slider_height - slider_thickness);
                this.graphicsWidth80.FillRectangle(this.orangeBrush, volume_x, volume_y, volume_width, volume_height);
                this.graphicsWidth80.FillRectangle(this.redBrush, peakL_x, peakL_y, peakL_width, peakL_height);
                this.graphicsWidth80.FillRectangle(this.redBrush, peakR_x, peakR_y, peakR_width, peakR_height);

                string displayVolume = string.Empty;
                if (PluginSettings.PreferDecibels && audioData.HasDecibels)
                {
                    displayVolume = Math.Round(audioData.Volume, 0).ToString() + " dB";
                }
                else
                {
                    displayVolume = Math.Round(audioData.VolumeScalar * 100.0f, 0).ToString() + " %";
                }

                this.graphicsWidth80.DrawString(displayVolume, this.calibri10Font, this.whiteBrush, new Rectangle(18, 60, 42, 12), this.cFormat);
            }
            else
            {
                this.graphicsWidth80.DrawString("No audio", this.calibri10Font, this.whiteBrush, new Rectangle(10, 60, 58, 12), this.cFormat);
            }

            if (audioData.IsCommunicationsDefault)
            {
                this.graphicsWidth80.DrawString("C", this.calibri7Font, this.greyBrush, new RectangleF(6, 53, 12, 12), this.cFormat);
            }
            if (audioData.IsMultimediaDefault)
            {
                this.graphicsWidth80.DrawString("M", this.calibri7Font, this.greyBrush, new RectangleF(6, 62, 12, 12), this.cFormat);
            }

            return this.imageWidth80;
        }

        private Bitmap Draw(AudioImageData audioData, PluginImageSize imageSize)
        {
            Bitmap icon;
            if (audioData.Muted)
            {
                if (string.IsNullOrEmpty(audioData.MutedIconPath))
                {
                    icon = this.iconsDictionary.GetOrAdd($"{audioData.UnmutedIconPath}+muted", (key) => this.CreateMutedIcon(audioData.UnmutedIconPath));
                }
                else
                {
                    icon = this.iconsDictionary.GetOrAdd($"{audioData.MutedIconPath}+muted", (key) => PluginImage.GetIcon(audioData.MutedIconPath).Scale(32, 32));
                }
            }
            else
            {
                icon = this.iconsDictionary.GetOrAdd($"{audioData.UnmutedIconPath}+unmuted", (key) => PluginImage.GetIcon(audioData.UnmutedIconPath).Scale(32, 32));
            }
            if (imageSize == PluginImageSize.Width60)
            {
                return this.DrawWidth50(audioData, icon);
            }
            else
            {
                return this.DrawWidth80(audioData, icon);
            }
        }

        public BitmapImage DrawBitmapImage(AudioImageData audioData, PluginImageSize imageSize)
        {
            lock (this.locker)
            {
                if (audioData.NotFound)
                {
                    return PluginImage.DrawTextImage("Not found", false, imageSize);
                }
                else
                {
                    Bitmap image = this.Draw(audioData, imageSize);
#if DEBUG
                    if (PluginSettings.SaveImageOnDisk)
                    {
                        if (IoHelpers.EnsureDirectoryExists(PluginData.Directory))
                        {
                            long timestamp = DateTime.Now.GetTotalMilliseconds();
                            string path = Path.Combine(PluginData.Directory, $"{audioData.DisplayName}_{timestamp}.png");
                            try
                            {
                                image.Save(path, ImageFormat.Png);
                            }
                            catch
                            {
                            }
                        }
                    }
#endif
                    ImageConverter converter = new ImageConverter();
                    return BitmapImage.FromArray((byte[])converter.ConvertTo(image, typeof(byte[])));
                }
            }
        }
    }
}
