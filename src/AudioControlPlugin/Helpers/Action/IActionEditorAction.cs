namespace Loupedeck.AudioControlPlugin;

internal interface IActionEditorAction
{
    string DisplayName { get; set; }
    string ResetDisplayName { get; set; }
    ActionEditor ActionEditor { get; }
    void ActionImageChanged();
    void AdjustmentValueChanged();
}
