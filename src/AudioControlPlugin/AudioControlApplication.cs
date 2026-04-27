namespace Loupedeck.AudioControlPlugin;

// This class can be used to connect the Loupedeck plugin to an application.
public class AudioControlApplication : ClientApplication
{
    public AudioControlApplication()
    {
    }

    // This method can be used to link the plugin to a Windows application.
    protected override string GetProcessName() => "";

    // This method can be used to link the plugin to a macOS application.
    protected override string GetBundleName() => "";

    // This method can be used to check whether the application is installed or not.
    public override ClientApplicationStatus GetApplicationStatus() => ClientApplicationStatus.Unknown;
}
