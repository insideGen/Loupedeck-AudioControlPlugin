namespace Loupedeck.AudioControlPlugin
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    internal static class PluginExtension
    {
        public enum DeviceTouchEventOrientation
        {
            Horizontal,
            Vertical,
            None
        }

        public static DeviceTouchEventOrientation GetOrientation(this DeviceTouchEvent touchEvent)
        {
            if (Math.Abs(touchEvent.DeltaX) > Math.Abs(touchEvent.DeltaY))
            {
                return DeviceTouchEventOrientation.Horizontal;
            }
            else if (Math.Abs(touchEvent.DeltaY) > Math.Abs(touchEvent.DeltaX))
            {
                return DeviceTouchEventOrientation.Vertical;
            }
            else
            {
                return DeviceTouchEventOrientation.None;
            }
        }

        public static Color WarmWhiteColor { get; } = Color.FromArgb(255, 240, 150);

        public static Color Filter(this Color color, float red, float green, float blue)
        {
            int r = (int)Math.Round(color.R * (1 - red));
            int g = (int)Math.Round(color.G * (1 - green));
            int b = (int)Math.Round(color.B * (1 - blue));
            return Color.FromArgb(color.A, r, g, b);
        }

        public static Color BlueLightFilter(this Color color)
        {
            if (PluginSettings.BlueLightFilterEnabled)
            {
                return color.Filter(0.0f, 0.05f, 0.35f);
            }
            return color;
        }

        public static Color Desaturate(this Color color, float factor)
        {
            factor = Math.Clamp(factor, 0.0f, 1.0f);
            float gray = 0.299f * color.R + 0.587f * color.G + 0.114f * color.B;
            int r = (int)Math.Round(color.R + (gray - color.R) * factor);
            int g = (int)Math.Round(color.G + (gray - color.G) * factor);
            int b = (int)Math.Round(color.B + (gray - color.B) * factor);
            return Color.FromArgb(color.A, r, g, b);
        }

        public static Bitmap ToNonIndexed(this Bitmap bitmap)
        {
            if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                return bitmap;
            }
            Bitmap converted = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(converted))
            {
                g.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
            }
            bitmap.Dispose();
            return converted;
        }

        public static Bitmap BlueLightFilter(this Bitmap bitmap)
        {
            if (PluginSettings.BlueLightFilterEnabled)
            {
                bitmap = bitmap.ToNonIndexed();
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        Color oldColor = bitmap.GetPixel(x, y);
                        Color newColor = oldColor.BlueLightFilter();
                        bitmap.SetPixel(x, y, newColor);
                    }
                }
            }
            return bitmap;
        }

        public static Bitmap Recolor(this Bitmap bitmap, Color newColor)
        {
            bitmap = bitmap.ToNonIndexed();
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color oldColor = bitmap.GetPixel(x, y);
                    int R = (int)Math.Round(oldColor.R + (1.0f - oldColor.R / 255.0f) * newColor.R);
                    int G = (int)Math.Round(oldColor.G + (1.0f - oldColor.G / 255.0f) * newColor.G);
                    int B = (int)Math.Round(oldColor.B + (1.0f - oldColor.B / 255.0f) * newColor.B);
                    bitmap.SetPixel(x, y, Color.FromArgb(oldColor.A, R, G, B));
                }
            }
            return bitmap;
        }

        public static Bitmap Desaturate(this Bitmap bitmap, float factor)
        {
            if (factor <= 0.0f)
            {
                return bitmap;
            }
            bitmap = bitmap.ToNonIndexed();
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color oldColor = bitmap.GetPixel(x, y);
                    Color newColor = oldColor.Desaturate(factor);
                    bitmap.SetPixel(x, y, newColor);
                }
            }
            return bitmap;
        }

        public static Bitmap Scale(this Bitmap bitmap, int maxWidth, int maxHeight)
        {
            if (bitmap == null || (bitmap.Width <= maxWidth && bitmap.Height <= maxHeight))
            {
                return bitmap;
            }
            float scale = Math.Min((float)maxWidth / bitmap.Width, (float)maxHeight / bitmap.Height);
            int newWidth = (int)(bitmap.Width * scale);
            int newHeight = (int)(bitmap.Height * scale);
            Bitmap scaled = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(scaled))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bitmap, 0, 0, newWidth, newHeight);
            }
            bitmap.Dispose();
            return scaled;
        }

        public static string ToLower(this Enum enumValue)
        {
            return enumValue.ToString().ToLower();
        }
    }
}
