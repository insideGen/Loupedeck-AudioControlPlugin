namespace Loupedeck.AudioControlPlugin;

using System.Collections.Generic;

internal abstract class FolderPage
{
    public Folder Folder { get; }
    public string Name { get; }

    public List<string> ButtonActionNames
    {
        get
        {
            lock (this.Folder.ButtonActionNames)
            {
                List<string> actionNames = new List<string>();
                foreach (string actionName in this.Folder.ButtonActionNames)
                {
                    this.Folder.ParseActionParameter(actionName, out string pageName, out string actionParam);
                    if (pageName == this.Name && !actionParam.StartsWith('@'))
                    {
                        actionNames.Add(actionParam);
                    }
                }
                return actionNames;
            }
        }
    }

    public FolderPage(Folder parent)
    {
        this.Folder = parent;
        this.Name = this.GetType().Name;
    }

    public void NavigateTo(FolderPage nextPage)
    {
        this.Folder.NavigateTo(nextPage);
    }

    public void LeavePage()
    {
        this.Folder.LeavePage();
    }

    public virtual void Load()
    {
    }

    public virtual void Enter()
    {
    }

    public virtual void Leave()
    {
    }

    public virtual void Unload()
    {
    }

    public virtual IEnumerable<string> GetButtonPressActionNames(DeviceType deviceType)
    {
        return Array.Empty<string>();
    }

    public virtual IEnumerable<string> GetEncoderRotateActionNames(DeviceType deviceType)
    {
        return Array.Empty<string>();
    }

    public virtual IEnumerable<string> GetEncoderPressActionNames(DeviceType deviceType)
    {
        return Array.Empty<string>();
    }

    public virtual BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
    {
        return PluginImage.DrawTextImage(actionParameter, false, imageSize);
    }

    public virtual BitmapImage GetAdjustmentImage(string actionParameter, PluginImageSize imageSize)
    {
        return PluginImage.DrawTextImage(actionParameter, false, imageSize);
    }

    public virtual void ApplyAdjustment(string actionParameter, int diff)
    {
    }

    public virtual bool ProcessButtonEvent2(string actionParameter, DeviceButtonEvent2 buttonEvent)
    {
        return false;
    }

    public virtual bool ProcessEncoderEvent(string actionParameter, DeviceEncoderEvent encoderEvent)
    {
        return false;
    }

    public virtual bool ProcessTouchEvent(string actionParameter, DeviceTouchEvent touchEvent)
    {
        return false;
    }

    public void ButtonActionNamesChanged()
    {
        this.Folder.ButtonActionNamesChanged();
    }

    public void CommandImageChanged(string actionParameter)
    {
        this.Folder.CommandImageChanged(this.Name, actionParameter);
    }

    public void AdjustmentImageChanged(string actionParameter)
    {
        this.Folder.AdjustmentImageChanged(this.Name, actionParameter);
    }
}
