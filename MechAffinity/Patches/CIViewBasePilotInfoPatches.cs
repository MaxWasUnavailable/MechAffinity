using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using MechAffinity.Helpers;
using PhantomBrigade;

// ReSharper disable InconsistentNaming

namespace MechAffinity.Patches;

[HarmonyPatch(typeof(CIViewBasePilotInfo))]
internal static class CIViewBasePilotInfoPatches
{
    [HarmonyTranspiler]
    [HarmonyPatch(nameof(CIViewBasePilotInfo.RedrawForPilot))]
    private static IEnumerable<CodeInstruction> RedrawForPilotTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .SearchForward(instruction => instruction.operand is "pilot_auto_combat_encounters")
            .ThrowIfInvalid("Could not find instruction with 'pilot_auto_combat_encounters' operand")
            .SearchBackwards(instruction => instruction.opcode == OpCodes.Ldstr && instruction.operand is "\n")
            .ThrowIfInvalid("Could not find instruction with '\\n' operand")
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_1))
            .ThrowIfNotMatch("Not at \\n insertion point", [new CodeMatch(OpCodes.Ldstr, "\n")])
            .SetInstructionAndAdvance(new CodeInstruction(OpCodes.Call,
                AccessTools.Method(typeof(CIViewBasePilotInfoPatches), nameof(GetMechAffinityBioString))))
            .InstructionEnumeration();
    }

    public static string GetMechAffinityBioString(PersistentEntity pilot)
    {
        var stringToAdd = "\n";

        var mechAffinityList = MechAffinityHelper.GetMechAffinityList(pilot);
        if (mechAffinityList.Count == 0)
            return stringToAdd;

        stringToAdd += "Mech Affinity:\n";
        foreach (var mechInternalName in mechAffinityList)
        {
            var mech = IDUtility.GetPersistentEntity(mechInternalName);
            if (mech == null)
                // Should remove the mech from the list if it doesn't exist?
                continue;
            stringToAdd +=
                $"- {mech.unitIdentification.nameOverride}: {MechAffinityHelper.GetMechAffinity(pilot, mech)}\n";
        }

        return stringToAdd + "\n\n";
    }
}