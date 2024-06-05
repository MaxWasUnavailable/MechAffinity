using HarmonyLib;
using MechAffinity.Features;
using PhantomBrigade.Combat.Systems;

namespace MechAffinity.Patches;

/// <summary>
///     Patches the CombatDamageSystem to apply the damage reduction multiplier
/// </summary>
[HarmonyPatch(typeof(CombatDamageSystem))]
internal static class CombatDamageSystemPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(CombatDamageSystem.OnPartDamage))]
    private static void OnPartDamagePrefix(CombatDamageSystem __instance,
        ref PersistentEntity unitPersistentDamaged,
        ref CombatEntity unitCombatDamaged,
        ref EquipmentEntity partDamaged,
        ref CombatEntity damageEvent,
        ref float damage,
        ref string directionKey,
        ref bool normalized,
        ref bool blockLoss)
    {
        damage *= MechAffinityBonus.GetDamageReductionMultiplier(unitPersistentDamaged);
    }
}