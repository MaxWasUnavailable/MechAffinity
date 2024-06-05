using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using MechAffinity.Features;

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
                AccessTools.Method(typeof(MechAffinityUI), nameof(MechAffinityUI.GetBioText))))
            .InstructionEnumeration();
    }
}