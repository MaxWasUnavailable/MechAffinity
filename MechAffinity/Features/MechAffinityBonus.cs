using MechAffinity.Helpers;
using UnityEngine;

namespace MechAffinity.Features;

public static class MechAffinityBonus
{
    public const float DamageReductionRequiredAffinity = 5f;
    public const float DamageReductionMaxAffinity = 20f;
    public const float DamageReductionAffinityMultiplier = 0.01f;
    
    public static float GetDamageReductionMultiplier(PersistentEntity mech)
    {
        var affinity = MechAffinityHelper.GetMechAffinityOfActivePilot(mech);
        affinity = Mathf.Min(affinity, DamageReductionMaxAffinity);
        
        if (affinity < DamageReductionRequiredAffinity)
            return 1f;
        
        return 1f - affinity * DamageReductionAffinityMultiplier;
    }
}