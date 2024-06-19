using System.IO;
using System.Reflection;

namespace MechAffinity.Config;

/// <summary>
///     Configuration for MechAffinity.
/// </summary>
public class MechAffinityConfig
{
    /// <summary>
    ///     Whether the mod should remove its data on load, so saving makes a campaign vanilla-compatible.
    /// </summary>
    public bool UninstallMode = false;

    private static string ConfigPath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
        "..", "Config", "settings.yaml");

    public static MechAffinityConfig LoadConfig()
    {
        var settings = UtilitiesYAML.ReadFromFile<MechAffinityConfig>(ConfigPath, false);
        if (settings == null)
        {
            settings = new MechAffinityConfig();
            SaveConfig(settings);
        }

        return settings;
    }

    public static void SaveConfig(MechAffinityConfig config)
    {
        UtilitiesYAML.SaveToFile(ConfigPath, config);
    }
}