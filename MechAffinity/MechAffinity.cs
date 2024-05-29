using System;
using HarmonyLib;
using PhantomBrigade.Mods;
using UnityEngine;

namespace MechAffinity;

/// <summary>
///     Main mod class for MechAffinity.
/// </summary>
public class MechAffinity : ModLink
{
    private bool _isPatched;
    public string Guid { get; } = "MaxWasUnavailable.MechAffinity";
    public string Name { get; } = "MechAffinity";
    public Version Version { get; } = new(1, 0, 0);

    /// <summary>
    ///     Singleton instance of the plugin.
    /// </summary>
    public static MechAffinity? Instance { get; private set; }

    public override void OnLoad(Harmony harmonyInstance)
    {
        // Set instance
        Instance = this;

        // Patch using Harmony
        PatchAll(harmonyInstance);

        // Report plugin loaded
        Debug.Log($"Loaded {Name} v{Version}");
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