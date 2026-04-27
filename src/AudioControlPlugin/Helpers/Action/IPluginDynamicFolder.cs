namespace Loupedeck.AudioControlPlugin;

internal interface IPluginDynamicFolder
{
    void ButtonActionNamesChanged();
    void EncoderActionNamesChanged();
    void CommandImageChanged(string actionParameter);
    void AdjustmentImageChanged(string actionParameter);
    void AdjustmentValueChanged(string actionParameter);
}
