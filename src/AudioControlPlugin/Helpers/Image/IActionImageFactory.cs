namespace Loupedeck.AudioControlPlugin;

internal interface IActionImageFactory<T> where T : IActionImageData
{
    BitmapImage DrawBitmapImage(T actionImageData, PluginImageSize imageSize);
}
