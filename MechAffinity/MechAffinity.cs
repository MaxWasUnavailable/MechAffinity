using System;
using HarmonyLib;
using MechAffinity.Config;
using PhantomBrigade.Mods;
using UnityEngine;

namespace MechAffinity;

/// <summary>
///     Main mod class for MechAffinity.
/// </summary>
public class MechAffinity : ModLink
{
    private bool _isPatched;
    internal MechAffinityConfig? Config { get; private set; }
    public static string Guid { get; } = "MaxWasUnavailable.MechAffinity";
    public static string Name { get; } = "MechAffinity";
    public static Version Version { get; } = new(0, 1, 1);

    /// <summary>
    ///     Singleton instance of the plugin.
    /// </summary>
    public static MechAffinity? Instance { get; private set; }

    public override void OnLoad(Harmony harmonyInstance)
    {
        // Set instance
        Instance = this;

        // Load config
        GetOrLoadConfig();

        Debug.Log($"Config.UninstallMode: {Config?.UninstallMode}");

        // Patch using Harmony
        PatchAll(harmonyInstance);

        // Report plugin loaded
        Debug.Log($"Loaded {Name} v{Version}");
    }

    public MechAffinityConfig GetOrLoadConfig()
    {
        return Config ??= MechAffinityConfig.LoadConfig();
    }

    private void PatchAll(Harmony harmonyInstance)
    {
        if (_isPatched)
        {
            Debug.LogWarning("Already patched!");
            return;
        }

        Debug.Log("Patching...");

        harmonyInstance.PatchAll();

        _isPatched = true;

        Debug.Log("Patched!");
    }
}