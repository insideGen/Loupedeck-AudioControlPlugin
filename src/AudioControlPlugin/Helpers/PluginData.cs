namespace Loupedeck.AudioControlPlugin;

internal static class PluginData
{
    private static Plugin? _plugin = null;
    private static string _directory = string.Empty;

    public static string Directory
    {
        get
        {
            if (_plugin != null && string.IsNullOrEmpty(_directory))
            {
                string pluginDataDirectory = _plugin.GetPluginDataDirectory();
                if (IoHelpers.EnsureDirectoryExists(pluginDataDirectory))
                {
                    _directory = pluginDataDirectory;
                }
                else
                {
                    _directory = string.Empty;
                }
            }
            return _directory;
        }
    }

    static PluginData()
    {
    }

    public static void Init(Plugin plugin)
    {
        _plugin = plugin;
    }
}
