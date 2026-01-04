namespace Loupedeck.AudioControlPlugin
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    using WindowsInterop.Win32;

    internal static class PluginImage
    {
        public static Bitmap ToBitmap(BitmapImage image)
        {
            using (MemoryStream stream = new MemoryStream(image.ToArray()))
            {
                return new Bitmap(stream);
            }
        }

        public static Bitmap ReadBitmap(string bitmapName)
        {
            using (BitmapImage image = PluginResources.ReadImage(bitmapName))
            {
                return ToBitmap(image);
            }
        }

        public static BitmapImage ToBitmapImage(Bitmap image)
        {
#if DEBUG
            if (PluginSettings.SaveImageOnDisk)
            {
                if (IoHelpers.EnsureDirectoryExists(PluginData.Directory))
                {
                    long timestamp = DateTime.Now.GetTotalMilliseconds();
                    string path = Path.Combine(PluginData.Directory, $"{timestamp}.png");
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

        public static void GetImageSize(PluginImageSize imageSize, out int imageWidth, out int imageHeight)
        {
            if (imageSize == PluginImageSize.Width60)
            {
                imageWidth = 50;
                imageHeight = 50;
            }
            else if (imageSize == PluginImageSize.Width90)
            {
                imageWidth = 80;
                imageHeight = 80;
            }
            else
            {
                imageWidth = 80;
                imageHeight = 80;
            }
        }

        public static PluginImageSize GetImageSize(int imageWidth, int imageHeight)
        {
            if (imageWidth == 50 && imageHeight == 50)
            {
                return PluginImageSize.Width60;
            }
            else if (imageWidth == 80 && imageHeight == 80)
            {
                return PluginImageSize.Width90;
            }
            else
            {
                return PluginImageSize.None;
            }
        }

        public static BitmapImage DrawBlackImage(PluginImageSize imageSize)
        {
            PluginImage.GetImageSize(imageSize, out int imageWidth, out int imageHeight);
            using (Bitmap image = new Bitmap(imageWidth, imageHeight))
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.Clear(Color.Black);
                return PluginImage.ToBitmapImage(image);
            }
        }

        public static BitmapImage DrawFolderTextImage(bool pluginName, string text, PluginImageSize imageSize)
        {
            PluginImage.GetImageSize(imageSize, out int imageWidth, out int imageHeight);
            using (Bitmap image = new Bitmap(imageWidth, imageHeight))
            using (Graphics graphics = Graphics.FromImage(image))
            using (Font calibri10Font = new Font("Calibri", 10, FontStyle.Regular))
            using (Font calibri12Font = new Font("Calibri", 12, FontStyle.Regular))
            using (Brush orangeBrush = new SolidBrush(Color.Orange))
            using (Brush whiteABrush = new SolidBrush(Color.FromArgb(192, Color.White.BlueLightFilter())))
            using (Brush whiteBrush = new SolidBrush(Color.White.BlueLightFilter()))
            using (StringFormat format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                graphics.Clear(Color.Black);

                int pluginNameY = 12;
                int textY = 0;

                if (PluginSettings.FolderDecorationEnabled)
                {
                    graphics.FillRectangle(orangeBrush, 8, 6, 64, 2);
                    graphics.FillRectangle(orangeBrush, 8, 72, 64, 2);
                }

                if (pluginName)
                {
                    textY += 15;
                    if (PluginSettings.FolderDecorationEnabled)
                    {
                        pluginNameY += 2;
                        textY -= 2;
                    }
                    graphics.DrawString("Audio", calibri10Font, whiteABrush, new RectangleF(2, pluginNameY, 76, 12), format);
                    graphics.DrawString("Control", calibri10Font, whiteABrush, new RectangleF(2, pluginNameY + 12, 76, 12), format);
                }

                graphics.DrawString(text, calibri12Font, whiteBrush, new RectangleF(0, textY, image.Width, image.Height), format);

                return PluginImage.ToBitmapImage(image);
            }
        }

        public static BitmapImage DrawFolderIconImage(bool pluginName, string iconPath, PluginImageSize imageSize)
        {
            PluginImage.GetImageSize(imageSize, out int imageWidth, out int imageHeight);
            using (Bitmap icon = PluginImage.ReadBitmap(iconPath))
            using (Bitmap image = new Bitmap(imageWidth, imageHeight))
            using (Graphics graphics = Graphics.FromImage(image))
            using (Font calibri10Font = new Font("Calibri", 10, FontStyle.Regular))
            using (Brush orangeBrush = new SolidBrush(Color.Orange))
            using (Brush whiteABrush = new SolidBrush(Color.FromArgb(192, Color.White.BlueLightFilter())))
            using (StringFormat format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                graphics.Clear(Color.Black);

                int pluginNameY = 12;
                int iconY = 0;

                if (PluginSettings.FolderDecorationEnabled)
                {
                    graphics.FillRectangle(orangeBrush, 8, 6, 64, 2);
                    graphics.FillRectangle(orangeBrush, 8, 72, 64, 2);
                }

                if (pluginName)
                {
                    iconY += 15;
                    if (PluginSettings.FolderDecorationEnabled)
                    {
                        pluginNameY += 2;
                        iconY -= 2;
                    }
                    graphics.DrawString("Audio", calibri10Font, whiteABrush, new RectangleF(2, pluginNameY, 76, 12), format);
                    graphics.DrawString("Control", calibri10Font, whiteABrush, new RectangleF(2, pluginNameY + 12, 76, 12), format);
                }

                icon.Recolor(Color.White.BlueLightFilter());
                graphics.DrawImage(icon, (image.Width - icon.Width) / 2, (image.Height - icon.Height) / 2 + iconY, icon.Width, icon.Height);

                return PluginImage.ToBitmapImage(image);
            }
        }

        public static BitmapImage DrawTextImage(string buttonName, bool bold, PluginImageSize imageSize)
        {
            PluginImage.GetImageSize(imageSize, out int imageWidth, out int imageHeight);
            using (Bitmap image = new Bitmap(imageWidth, imageHeight))
            using (Graphics graphics = Graphics.FromImage(image))
            using (Font calibri10Font = new Font("Calibri", 12, bold ? FontStyle.Bold : FontStyle.Regular))
            using (Brush whiteBrush = new SolidBrush(Color.White.BlueLightFilter()))
            using (StringFormat format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                graphics.Clear(Color.Black);
                graphics.DrawString(buttonName, calibri10Font, whiteBrush, new RectangleF(0, 0, image.Width, image.Height), format);
                return PluginImage.ToBitmapImage(image);
            }
        }

        public static BitmapImage DrawIconImage(string iconPath, PluginImageSize imageSize)
        {
            PluginImage.GetImageSize(imageSize, out int imageWidth, out int imageHeight);
            using (Bitmap image = new Bitmap(imageWidth, imageHeight))
            using (Graphics graphics = Graphics.FromImage(image))
            using (Bitmap icon = PluginImage.ReadBitmap(iconPath))
            {
                graphics.Clear(Color.Black);
                icon.Recolor(Color.White.BlueLightFilter());
                graphics.DrawImage(icon, (image.Width - icon.Width) / 2, (image.Height - icon.Height) / 2, icon.Width, icon.Height);
                return PluginImage.ToBitmapImage(image);
            }
        }

        public static Bitmap GetIcon(string iconPath)
        {
            Bitmap iconBitmap = null;
            if (!string.IsNullOrWhiteSpace(iconPath))
            {
                //int iconSize = 32;
                string[] values = iconPath.Split(',');
                string path = values[0];
                if (path.StartsWith('@'))
                {
                    path = path.Substring(1);
                    string color = values.Length > 1 ? values[1] : null;
                    iconBitmap = PluginImage.ReadBitmap(path);
                    if (iconBitmap != null && !string.IsNullOrEmpty(color))
                    {
                        iconBitmap = iconBitmap.Recolor(Color.FromName(color));
                    }
                }
                //else if (path.EndsWithNoCase(".dll"))
                //{
                //    StringBuilder iconPathSB = new StringBuilder(iconPath);
                //    int iconIndex = Shlwapi.PathParseIconLocation(iconPathSB);
                //    if (iconIndex != 0)
                //    {
                //        using (HMODULE hModule = Kernel32.LoadLibraryEx(iconPathSB.ToString(), IntPtr.Zero, Kernel32.LoadLibraryFlags.LOAD_LIBRARY_AS_DATAFILE | Kernel32.LoadLibraryFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE))
                //        {
                //            IntPtr groupResInfo = Kernel32.FindResource(hModule, new IntPtr(Math.Abs(iconIndex)), Kernel32.RT_GROUP_ICON);
                //            IntPtr groupResData = Kernel32.LockResource(Kernel32.LoadResource(hModule, groupResInfo));
                //            int iconId = User32.LookupIconIdFromDirectoryEx(groupResData, true, iconSize, iconSize, User32.LoadImageFlags.LR_DEFAULTCOLOR);

                //            IntPtr iconResInfo = Kernel32.FindResource(hModule, new IntPtr(iconId), Kernel32.RT_ICON);
                //            IntPtr iconResData = Kernel32.LockResource(Kernel32.LoadResource(hModule, iconResInfo));
                //            int iconResSize = Kernel32.SizeofResource(hModule, iconResInfo);
                //            IntPtr iconHandle = User32.CreateIconFromResourceEx(iconResData, iconResSize, true, User32.IconCursorVersion.Default, iconSize, iconSize, User32.LoadImageFlags.LR_DEFAULTCOLOR);

                //            using (Icon icon = Icon.FromHandle(iconHandle))
                //            {
                //                icon.GetType().GetField("ownHandle", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(icon, true);
                //                iconBitmap = icon.ToBitmap().BlueLightFilter();
                //            }

                //            User32.DestroyIcon(iconHandle);
                //        }
                //    }
                //}
                else if (path.EndsWithNoCase(".dll") || path.EndsWithNoCase(".exe"))
                {
                    int index = values.Length > 1 && int.TryParse(values[1], out index) ? index : 0;
                    Shell32.ExtractIconEx(path, index, out IntPtr large, out IntPtr small, 1);
                    iconBitmap = Icon.FromHandle(large).ToBitmap().BlueLightFilter();
                    User32.DestroyIcon(large);
                    User32.DestroyIcon(small);
                }
                //else
                //{
                //    System.Windows.Media.Imaging.BitmapSource imageSource = null;
                //    string systemPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
                //    string windowsPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                //    if (Path.GetDirectoryName(path).StartsWith(systemPath, StringComparison.InvariantCultureIgnoreCase))
                //    {
                //        path = Path.Combine(windowsPath, "sysnative", path.Substring(systemPath.Length + 1));
                //    }
                //    // Microsoft includes garbage CortanaUI app assets (\MicrosoftWindows.Client.CBS_cw5n1h2txyewy\Cortana.UI\Assets\App)
                //    // so replace the appid with one that has better (than nothing) assets.
                //    // Ref: https://github.com/File-New-Project/EarTrumpet/issues/1259
                //    if (path.Equals("MicrosoftWindows.Client.CBS_cw5n1h2txyewy!CortanaUI", StringComparison.InvariantCultureIgnoreCase))
                //    {
                //        path = "MicrosoftWindows.Client.CBS_cw5n1h2txyewy!PackageMetadata";
                //    }

                //    IShellItem2 shellItem;
                //    try
                //    {
                //        Guid appsFolder = Guid.Parse("{1E87508D-89C2-42F0-8A7E-645A0F50CA58}");
                //        shellItem = Shell32.SHCreateItemInKnownFolder(appsFolder, Shell32.KF_FLAG_DONT_VERIFY, path, typeof(IShellItem2).GUID);
                //    }
                //    catch (Exception)
                //    {
                //        shellItem = Shell32.SHCreateItemFromParsingName(path, IntPtr.Zero, typeof(IShellItem2).GUID);
                //    }

                //    ((IShellItemImageFactory)shellItem).GetImage(new SIZE { cx = iconSize, cy = iconSize }, SIIGBF.SIIGBF_RESIZETOFIT, out nint bmp);
                //    try
                //    {
                //        imageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                //    }
                //    finally
                //    {
                //        Gdi32.DeleteObject(bmp);
                //    }

                //    IntPtr ptr = IntPtr.Zero;
                //    try
                //    {
                //        int stride = imageSource.PixelWidth * ((imageSource.Format.BitsPerPixel + 7) / 8);
                //        ptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(imageSource.PixelHeight * stride);
                //        imageSource.CopyPixels(new System.Windows.Int32Rect(0, 0, imageSource.PixelWidth, imageSource.PixelHeight), ptr, imageSource.PixelHeight * stride, stride);
                //        using (Bitmap bm = new Bitmap(imageSource.PixelWidth, imageSource.PixelHeight, stride, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, ptr))
                //        {
                //            iconBitmap = new Bitmap(bm).BlueLightFilter();
                //        }
                //    }
                //    finally
                //    {
                //        if (ptr != IntPtr.Zero)
                //        {
                //            System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);
                //        }
                //    }
                //}
            }
            return iconBitmap;
        }
    }
}
