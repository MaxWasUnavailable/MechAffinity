using System.Linq;
using HarmonyLib;
using MechAffinity.Constants;
using MechAffinity.Helpers;
using PhantomBrigade;
using PhantomBrigade.Overworld.Systems;

namespace MechAffinity.Patches;

[HarmonyPatch(typeof(OverworldCombatOutcomeProcessingSystem))]
internal static class OverworldCombatOutcomeProcessingSystemPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(OverworldCombatOutcomeProcessingSystem.Execute))]
    private static void ExecutePostfix()
    {
        foreach (var participantUnit in ScenarioUtility.GetCombatParticipantUnits())
        {
            if (participantUnit.faction.s != FactionConstants.PlayerFaction)
                continue;

            var pilot = IDUtility.GetLinkedPilot(participantUnit);
            if (pilot == null)
                continue;

            if (pilot.isDeceased)
                continue;

            if (participantUnit.isDestroyed)
                continue;

            foreach (var mechInternalName in MechAffinityHelper.GetMechAffinityList(pilot)
                         .Where(mechInternalName => mechInternalName != participantUnit.nameInternal.s))
                MechAffinityHelper.ReduceMechAffinity(pilot, IDUtility.GetPersistentEntity(mechInternalName), 1);

            MechAffinityHelper.AddMechAffinity(pilot, participantUnit, 1);
        }
    }
}