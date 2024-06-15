using MechAffinity.Helpers;
using UnityEngine;

namespace MechAffinity.Features;

/// <summary>
///     Mech-affinity functionality related to bonuses that unlock as the pilot gains affinity with the mech.
/// </summary>
public static class MechAffinityBonus
{
    /// <summary>
    ///     The minimum affinity required to unlock the damage reduction bonus.
    /// </summary>
    public const float DamageReductionRequiredAffinity = 5f;

    /// <summary>
    ///     The maximum affinity used for the damage reduction bonus.
    /// </summary>
    public const float DamageReductionMaxAffinity = 15f;

    /// <summary>
    ///     The multiplier used to calculate the damage reduction bonus.
    /// </summary>
    public const float DamageReductionAffinityMultiplier = 0.01f;

    /// <summary>
    ///     Get the damage reduction multiplier for the given mech.
    /// </summary>
    /// <param name="mech"> The mech to get the damage reduction multiplier for. </param>
    /// <returns> The damage reduction multiplier. </returns>
    public static float GetDamageReductionMultiplier(PersistentEntity mech)
    {
        var affinity = MechAffinityHelper.GetMechAffinityOfActivePilot(mech);
        affinity = Mathf.Min(affinity, DamageReductionMaxAffinity);

        if (affinity < DamageReductionRequiredAffinity)
            return 1f;

        return 1f - affinity * DamageReductionAffinityMultiplier;
    }
}