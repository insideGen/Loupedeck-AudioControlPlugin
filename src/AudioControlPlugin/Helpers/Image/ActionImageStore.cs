namespace Loupedeck.AudioControlPlugin;

using System.Collections.Concurrent;
using System.Collections.Generic;

internal class ActionImageStore<T> where T : IActionImageData
{
    private readonly IActionImageFactory<T> actionImageFactory;
    private readonly ConcurrentDictionary<string, T> actionImageData;
    private readonly ConcurrentDictionary<string, IActionImageFactory<T>> actionImageFactories;
    private readonly ConcurrentDictionary<string, BitmapImage> actionWidth60Images;
    private readonly ConcurrentDictionary<string, BitmapImage> actionWidth90Images;

    public ICollection<string> ActionImageIds
    {
        get
        {
            return this.actionImageData.Keys;
        }
    }

    public ActionImageStore(IActionImageFactory<T> actionImageFactory)
    {
        this.actionImageFactory = actionImageFactory;
        this.actionImageData = new ConcurrentDictionary<string, T>();
        this.actionImageFactories = new ConcurrentDictionary<string, IActionImageFactory<T>>();
        this.actionWidth60Images = new ConcurrentDictionary<string, BitmapImage>();
        this.actionWidth90Images = new ConcurrentDictionary<string, BitmapImage>();
    }

    public bool TryGetImage(string id, PluginImageSize imageSize, out BitmapImage bitmapImage)
    {
        ConcurrentDictionary<string, BitmapImage> actionImages;
        if (imageSize == PluginImageSize.Width60)
        {
            actionImages = this.actionWidth60Images;
        }
        else
        {
            actionImages = this.actionWidth90Images;
        }
        bool exist = actionImages.TryGetValue(id, out BitmapImage storedBitmapImage);
        if (exist)
        {
            bitmapImage = BitmapImage.FromArray(storedBitmapImage.ToArray());
            return true;
        }
        else
        {
            bitmapImage = null;
            return false;
        }
    }

    public bool UpdateImage(string id, T newActionData)
    {
        bool exist = this.actionImageData.TryGetValue(id, out T lastActionData);
        if (!exist || !newActionData.Equals(lastActionData))
        {
            IActionImageFactory<T> factory = this.actionImageFactories.GetOrAdd(id, this.actionImageFactory.GetType().CreateInstance() as IActionImageFactory<T>);
            this.actionImageData.AddOrUpdate(id, newActionData, (key, oldValue) => newActionData);

            BitmapImage bitmapWidth60Image = factory.DrawBitmapImage(newActionData, PluginImageSize.Width60);
            this.actionWidth60Images.AddOrUpdate(id, bitmapWidth60Image, (key, oldValue) => bitmapWidth60Image);

            BitmapImage bitmapWidth90Image = factory.DrawBitmapImage(newActionData, PluginImageSize.Width90);
            this.actionWidth90Images.AddOrUpdate(id, bitmapWidth90Image, (key, oldValue) => bitmapWidth90Image);

            return true;
        }
        return false;
    }

    public bool UpdateImage(T newActionData)
    {
        return this.UpdateImage(newActionData.Id, newActionData);
    }
}
