using HarmonyLib;
using UnityEngine;

namespace MechAffinity.Patches;

[HarmonyPatch(typeof(CIViewPauseRoot))]
internal static class TestPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(CIViewPauseRoot.Start))]
    private static void AddTestTextToMainMenuButton(CIViewPauseRoot __instance)
    {
        Debug.Log("Adding test text to main menu button");
        CIViewDialogConfirmation.ins.Open 
        (
            "My mod loaded", 
            "Detailed message and other stuff I want to print", 
            null,
            null
        );
    }
}