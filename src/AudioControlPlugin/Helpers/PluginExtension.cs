namespace Loupedeck.AudioControlPlugin
{
    using System;
    using System.Drawing;

    internal static class PluginExtension
    {
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

        public static string ToLower(this Enum enumValue)
        {
            return enumValue.ToString().ToLower();
        }
    }
}
