namespace Loupedeck.AudioControlPlugin;

internal static class PluginSettings
{
    private static Plugin? _plugin = null;

    private static bool? _blueLightFilterEnabled = null;
    private static bool? _isWindowsIconStyle = null;
    private static bool? _preferDecibels = null;
    private static bool? _peakMeterEnabled = null;
    private static int? _fps = null;
    private static bool? _saveImageOnDisk = null;

    public static bool BlueLightFilterEnabled
    {
        get
        {
            if (!_blueLightFilterEnabled.HasValue)
            {
                if (_plugin is not null && _plugin.TryGetPluginSetting("blueLightFilterEnabled", out string value))
                {
                    _blueLightFilterEnabled = bool.Parse(value);
                }
                else
                {
                    return true;
                }
            }
            return (bool)_blueLightFilterEnabled;
        }
        set
        {
            _blueLightFilterEnabled = value;
            _plugin?.SetPluginSetting("blueLightFilterEnabled", value.ToString());
        }
    }

    public static bool IsWindowsIconStyle
    {
        get
        {
            if (!_isWindowsIconStyle.HasValue)
            {
                if (_plugin is not null && _plugin.TryGetPluginSetting("isWindowsIconStyle", out string value))
                {
                    _isWindowsIconStyle = bool.Parse(value);
                }
                else
                {
                    return true;
                }
            }
            return (bool)_isWindowsIconStyle;
        }
        set
        {
            _isWindowsIconStyle = value;
            _plugin?.SetPluginSetting("isWindowsIconStyle", value.ToString());
        }
    }

    public static bool PreferDecibels
    {
        get
        {
            if (!_preferDecibels.HasValue)
            {
                if (_plugin is not null && _plugin.TryGetPluginSetting("preferDecibels", out string value))
                {
                    _preferDecibels = bool.Parse(value);
                }
                else
                {
                    return false;
                }
            }
            return (bool)_preferDecibels;
        }
        set
        {
            _preferDecibels = value;
            _plugin?.SetPluginSetting("useScalarVolume", value.ToString());
        }
    }

    public static bool PeakMeterEnabled
    {
        get
        {
            if (!_peakMeterEnabled.HasValue)
            {
                if (_plugin is not null && _plugin.TryGetPluginSetting("peakMeterEnabled", out string value))
                {
                    _peakMeterEnabled = bool.Parse(value);
                }
                else
                {
                    return true;
                }
            }
            return (bool)_peakMeterEnabled;
        }
        set
        {
            _peakMeterEnabled = value;
            _plugin?.SetPluginSetting("peakMeterEnabled", value.ToString());
        }
    }

    public static int FPS
    {
        get
        {
            if (!_fps.HasValue)
            {
                if (_plugin is not null && _plugin.TryGetPluginSetting("fps", out string value))
                {
                    _fps = int.Parse(value);
                }
                else
                {
                    return 20;
                }
            }
            return (int)_fps;
        }
        set
        {
            _fps = value;
            _plugin?.SetPluginSetting("fps", value.ToString());
        }
    }

    public static bool SaveImageOnDisk
    {
        get
        {
            if (!_saveImageOnDisk.HasValue)
            {
                if (_plugin is not null && _plugin.TryGetPluginSetting("saveImageOnDisk", out string value))
                {
                    _saveImageOnDisk = bool.Parse(value);
                }
                else
                {
                    return false;
                }
            }
            return (bool)_saveImageOnDisk;
        }
        set
        {
            _saveImageOnDisk = value;
            _plugin?.SetPluginSetting("saveImageOnDisk", value.ToString());
        }
    }

    static PluginSettings()
    {
    }

    public static void Init(Plugin plugin)
    {
        _plugin = plugin;
    }
}
