using System.Linq;
using HarmonyLib;
using MechAffinity.Helpers;
using PhantomBrigade;
using UnityEngine;

namespace MechAffinity.Patches;

/// <summary>
///     Patches the DataHelperLoading to allow for removing of the MechAffinity save data for mod uninstallation.
/// </summary>
[HarmonyPatch(typeof(DataHelperLoading))]
internal static class DataHelperLoadingPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(DataHelperLoading.LoadingEnd))]
    private static void LoadingEndPostfix()
    {
        // TODO: Need to add a setting to toggle this behavior
        return;
        Debug.Log("Removing MechAffinity save data");
        foreach (var pilot in Contexts.sharedInstance.persistent.GetEntitiesWithEntityLinkPersistentParent(
                     IDUtility.playerBasePersistent.id.id).Where(pilot => pilot.isPilotTag))
            MechAffinityHelper.RemoveModCustomMemory(pilot);
    }
}