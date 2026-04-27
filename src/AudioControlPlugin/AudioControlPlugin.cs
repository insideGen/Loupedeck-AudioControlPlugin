namespace Loupedeck.AudioControlPlugin;

using System.Timers;

// This class contains the plugin-level logic of the Loupedeck plugin.
public class AudioControlPlugin : Plugin
{
    public static Timer RefreshTimer { get; }

    // Gets a value indicating whether this is an API-only plugin.
    public override bool UsesApplicationApiOnly => true;

    // Gets a value indicating whether this is a Universal plugin or an Application plugin.
    public override bool HasNoApplication => true;

    // Initializes a new instance of the plugin class.
    static AudioControlPlugin()
    {
        AudioControlPlugin.RefreshTimer = new Timer();
    }

    public AudioControlPlugin()
    {
        // Initialize the plugin log.
        PluginLog.Init(this.Log);

        // Initialize the plugin resources.
        PluginResources.Init(this.Assembly);

        // Initialize the plugin settings.
        PluginSettings.Init(this);

        // Initialize the plugin data.
        PluginData.Init(this);
    }

    // This method is called when the plugin is loaded.
    public override void Load()
    {
        base.Info.Icon16x16 = PluginResources.ReadImage("Icon16x16.png");
        base.Info.Icon32x32 = PluginResources.ReadImage("Icon32x32.png");
        base.Info.Icon48x48 = PluginResources.ReadImage("Icon48x48.png");
        base.Info.Icon256x256 = PluginResources.ReadImage("Icon256x256.png");

        AudioControlPlugin.RefreshTimer.Interval = 1000.0 / PluginSettings.FPS;
        AudioControlPlugin.RefreshTimer.Enabled = true;
    }

    // This method is called when the plugin is unloaded.
    public override void Unload()
    {
        AudioControlPlugin.RefreshTimer.Enabled = false;
    }
}
