namespace Loupedeck.AudioControlPlugin.Loupedeck;

internal class AudioControlMuteCommand : ActionEditorCommand, IActionEditorAction
{
    private const string DISPLAY_NAME = "Audio touch action";

    private AudioControlAction Action { get; }

    public AudioControlMuteCommand() : base(DeviceType.LoupedeckCtFamily)
    {
        base.DisplayName = $"{DISPLAY_NAME}";
        base.Description = "";
        base.GroupName = null;
        base.IsWidget = true;

        this.Action = new AudioControlAction(this);
    }

    protected override bool OnLoad() => this.Action.OnLoad();

    protected override bool OnUnload() => this.Action.OnUnload();

    protected override string GetCommandDisplayName(ActionEditorActionParameters actionParameters) => this.Action.GetDisplayName(actionParameters);

    protected override BitmapImage GetCommandImage(ActionEditorActionParameters actionParameters, int imageWidth, int imageHeight) => this.Action.GetImage(actionParameters, imageWidth, imageHeight);

    protected override bool ProcessButtonEvent2(ActionEditorActionParameters actionParameters, DeviceButtonEvent2 buttonEvent) => this.Action.ProcessButtonEvent2(actionParameters, buttonEvent);

    protected override bool ProcessTouchEvent(ActionEditorActionParameters actionParameters, DeviceTouchEvent touchEvent) => this.Action.ProcessTouchEvent(actionParameters, touchEvent);

    public new void ActionImageChanged() => base.ActionImageChanged();

    public new void AdjustmentValueChanged() => base.AdjustmentValueChanged();
}
