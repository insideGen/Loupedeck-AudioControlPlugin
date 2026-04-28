namespace Loupedeck.AudioControlPlugin;

using System.Collections.Generic;
using System.Linq;

internal abstract class Folder : PluginDynamicFolder, IPluginDynamicFolder
{
    private FolderPage? _homePage = null;

    public List<string> ButtonActionNames { get; }

    public Stack<FolderPage> PageStack { get; }

    public FolderPage? HomePage
    {
        get
        {
            return this._homePage;
        }
        set
        {
            this._homePage = value;
            this.PageStack.Clear();
            if (this._homePage is not null)
            {
                this.PageStack.Push(this._homePage);
            }
        }
    }

    public FolderPage? CurrentPage
    {
        get
        {
            if (this.PageStack.Count > 0)
            {
                return this.PageStack.Peek();
            }
            return null;
        }
    }

    protected Folder(string displayName, string? description = null, string? groupName = null, DeviceType supportedDevices = DeviceType.All) : base()
    {
        base.DisplayName = displayName;
        base.Description = description;
        base.GroupName = groupName;
        base.SupportedDevices = supportedDevices;

        this.ButtonActionNames = new List<string>();
        this.PageStack = new Stack<FolderPage>();
    }

    public void NavigateTo(FolderPage nextPage)
    {
        if (nextPage == null || this.CurrentPage == null)
        {
            return;
        }
        this.CurrentPage.Leave();
        this.PageStack.Push(nextPage);
        this.ButtonActionNamesChanged();
        this.EncoderActionNamesChanged();
        this.CurrentPage.Load();
        this.CurrentPage.Enter();
    }

    public void LeavePage()
    {
        if (this.CurrentPage == null)
        {
            return;
        }
        this.CurrentPage.Leave();
        this.CurrentPage.Unload();
        this.PageStack.Pop();
        if (this.PageStack.Count == 0)
        {
            base.Close();
        }
        else
        {
            this.ButtonActionNamesChanged();
            this.EncoderActionNamesChanged();
            this.CurrentPage.Enter();
        }
    }

    public void ParseActionParameter(string actionParameter, out string pageName, out string pageActionParameter)
    {
        int index = actionParameter.IndexOf('+');
        pageName = actionParameter.Substring(0, index);
        pageActionParameter = actionParameter.Substring(index + 1);
    }

    public FolderPage? GetPage(string pageName)
    {
        return this.PageStack.FirstOrDefault(p => p.Name == pageName);
    }

    public override bool Activate()
    {
        this.CurrentPage?.Load();
        this.CurrentPage?.Enter();
        return base.Activate();
    }

    public override bool Deactivate()
    {
        this.CurrentPage?.Leave();
        this.CurrentPage?.Unload();
        this.PageStack.Clear();
        if (this._homePage is not null)
        {
            this.PageStack.Push(this._homePage);
        }
        return base.Deactivate();
    }

    public override BitmapImage GetButtonImage(PluginImageSize imageSize)
    {
        return PluginImage.DrawFolderTextImage(false, base.DisplayName, imageSize);
    }

    public override PluginDynamicFolderNavigation GetNavigationArea(DeviceType _)
    {
        return PluginDynamicFolderNavigation.None;
    }

    public override IEnumerable<string> GetButtonPressActionNames(DeviceType deviceType)
    {
        lock (this.ButtonActionNames)
        {
            this.ButtonActionNames.Clear();
            if (this.CurrentPage != null)
            {
                this.ButtonActionNames.Add($"{this.CurrentPage.Name}+@back");
                this.ButtonActionNames.AddRange(this.CurrentPage.GetButtonPressActionNames(deviceType).Select(s => $"{this.CurrentPage.Name}+{s}"));
            }
            return this.ButtonActionNames.Select(s => base.CreateCommandName(s));
        }
    }

    public override IEnumerable<string> GetEncoderRotateActionNames(DeviceType deviceType)
    {
        List<string> actionNames = new List<string>();
        if (this.CurrentPage != null)
        {
            actionNames.AddRange(this.CurrentPage.GetEncoderRotateActionNames(deviceType).Select(s => $"{this.CurrentPage.Name}+{s}"));
        }
        return actionNames.Select(s => base.CreateAdjustmentName(s));
    }

    public override IEnumerable<string> GetEncoderPressActionNames(DeviceType deviceType)
    {
        List<string> actionNames = new List<string>();
        if (this.CurrentPage != null)
        {
            actionNames.AddRange(this.CurrentPage.GetEncoderPressActionNames(deviceType).Select(s => $"{this.CurrentPage.Name}+{s}"));
        }
        return actionNames.Select(s => base.CreateCommandName(s));
    }

    public override BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
    {
        this.ParseActionParameter(actionParameter, out string pageName, out string pageActionParameter);
        if (this.GetPage(pageName) is FolderPage page && page == this.CurrentPage)
        {
            if (pageActionParameter == "@back")
            {
                if (this.PageStack.Count > 1)
                {
                    pageActionParameter = "Back";
                }
                else
                {
                    pageActionParameter = "Exit";
                }
                return PluginImage.DrawTextImage(pageActionParameter, false, imageSize);
            }
            else
            {
                return page.GetCommandImage(pageActionParameter, imageSize);
            }
        }
        return PluginImage.DrawBlackImage(imageSize);
    }

    public override BitmapImage GetAdjustmentImage(string actionParameter, PluginImageSize imageSize)
    {
        this.ParseActionParameter(actionParameter, out string pageName, out string pageActionParameter);
        if (this.GetPage(pageName) is FolderPage page && page == this.CurrentPage)
        {
            return page.GetAdjustmentImage(pageActionParameter, imageSize);
        }
        return PluginImage.DrawBlackImage(imageSize);
    }

    public override void RunCommand(string actionParameter)
    {
        this.ParseActionParameter(actionParameter, out string pageName, out string pageActionParameter);
        if (this.GetPage(pageName) is FolderPage page && page == this.CurrentPage)
        {
            if (pageActionParameter == "@back")
            {
                this.LeavePage();
            }
            else
            {
                page.RunCommand(pageActionParameter);
            }
        }
        base.RunCommand(pageActionParameter);
    }

    public override void ApplyAdjustment(string actionParameter, int diff)
    {
        this.ParseActionParameter(actionParameter, out string pageName, out string pageActionParameter);
        if (this.GetPage(pageName) is FolderPage page && page == this.CurrentPage)
        {
            page.ApplyAdjustment(pageActionParameter, diff);
        }
        base.ApplyAdjustment(pageActionParameter, diff);
    }

    public override bool ProcessButtonEvent2(string actionParameter, DeviceButtonEvent2 buttonEvent)
    {
        this.ParseActionParameter(actionParameter, out string pageName, out string pageActionParameter);
        if (this.GetPage(pageName) is FolderPage page && page == this.CurrentPage)
        {
            if (buttonEvent.EventType == DeviceButtonEventType.Press)
            {
                return page.ProcessButtonEvent2(pageActionParameter, buttonEvent);
            }
        }
        return base.ProcessButtonEvent2(actionParameter, buttonEvent);
    }

    public override bool ProcessEncoderEvent(string actionParameter, DeviceEncoderEvent encoderEvent)
    {
        this.ParseActionParameter(actionParameter, out string pageName, out string pageActionParameter);
        if (this.GetPage(pageName) is FolderPage page && page == this.CurrentPage)
        {
            return page.ProcessEncoderEvent(pageActionParameter, encoderEvent);
        }
        return base.ProcessEncoderEvent(actionParameter, encoderEvent);
    }

    public override bool ProcessTouchEvent(string actionParameter, DeviceTouchEvent touchEvent)
    {
        this.ParseActionParameter(actionParameter, out string pageName, out string pageActionParameter);
        if (this.GetPage(pageName) is FolderPage page && page == this.CurrentPage)
        {
            if (pageActionParameter == "@back")
            {
                if (touchEvent.EventType == DeviceTouchEventType.Tap)
                {
                    this.LeavePage();
                }
            }
            else
            {
                return page.ProcessTouchEvent(pageActionParameter, touchEvent);
            }
        }
        return base.ProcessTouchEvent(actionParameter, touchEvent);
    }

    public new void CommandImageChanged(string actionParameter)
    {
        if (this.CurrentPage != null)
        {
            base.CommandImageChanged($"{this.CurrentPage.Name}+{actionParameter}");
        }
    }

    public void CommandImageChanged(string pageName, string actionParameter)
    {
        base.CommandImageChanged($"{pageName}+{actionParameter}");
    }

    public new void AdjustmentImageChanged(string actionParameter)
    {
        if (this.CurrentPage != null)
        {
            base.AdjustmentImageChanged($"{this.CurrentPage.Name}+{actionParameter}");
        }
    }

    public void AdjustmentImageChanged(string pageName, string actionParameter)
    {
        base.AdjustmentImageChanged($"{pageName}+{actionParameter}");
    }

    public new void ButtonActionNamesChanged() => base.ButtonActionNamesChanged();

    public new void EncoderActionNamesChanged() => base.EncoderActionNamesChanged();

    public new void AdjustmentValueChanged(string actionParameter) => base.AdjustmentValueChanged(actionParameter);
}
