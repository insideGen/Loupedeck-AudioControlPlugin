namespace Loupedeck.AudioControlPlugin
{
    using System;

    internal class AudioControlVolumeAdjustment : ActionEditorAdjustment, IActionEditorAction
    {
        private const string DISPLAY_NAME = "Audio dial action";

        private AudioControlAction Action { get; }

        public AudioControlVolumeAdjustment() : base(hasReset: true)
        {
            base.DisplayName = $"{DISPLAY_NAME} - Adjustment";
            base.Description = "";
            base.GroupName = null;
            base.IsWidget = true;

            base.ResetCommandDisplayName = $"{DISPLAY_NAME} - Adjustment reset";

            this.Action = new AudioControlAction(this);
        }

        protected override bool OnLoad() => this.Action.OnLoad();

        protected override bool OnUnload() => this.Action.OnUnload();

        protected override string GetCommandDisplayName(ActionEditorActionParameters actionParameters) => this.Action.GetDisplayName(actionParameters);

        protected override BitmapImage GetCommandImage(ActionEditorActionParameters actionParameters, int imageWidth, int imageHeight) => this.Action.GetImage(actionParameters, imageWidth, imageHeight);

        protected override bool ProcessButtonEvent2(ActionEditorActionParameters actionParameters, DeviceButtonEvent2 buttonEvent) => this.Action.ProcessButtonEvent2(actionParameters, buttonEvent);

        protected override bool ProcessTouchEvent(ActionEditorActionParameters actionParameters, DeviceTouchEvent touchEvent) => this.Action.ProcessTouchEvent(actionParameters, touchEvent);

        protected override string GetAdjustmentDisplayName(ActionEditorActionParameters actionParameters) => this.Action.GetDisplayName(actionParameters);

        protected override BitmapImage GetAdjustmentImage(ActionEditorActionParameters actionParameters, int imageWidth, int imageHeight) => this.Action.GetImage(actionParameters, imageWidth, imageHeight);

        protected override bool ApplyAdjustment(ActionEditorActionParameters actionParameters, int diff) => this.Action.ApplyAdjustment(actionParameters, diff);

        protected override bool ProcessEncoderEvent(ActionEditorActionParameters actionParameters, DeviceEncoderEvent encoderEvent) => this.Action.ProcessEncoderEvent(actionParameters, encoderEvent);

        public new void ActionImageChanged() => base.ActionImageChanged();

        public new void AdjustmentValueChanged() => base.AdjustmentValueChanged();
    }
}
