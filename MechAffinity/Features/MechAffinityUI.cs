using System.Linq;
using MechAffinity.Helpers;
using PhantomBrigade;

namespace MechAffinity.Features;

/// <summary>
///     UI-related functionality for Mech Affinity.
/// </summary>
public static class MechAffinityUI
{
    /// <summary>
    ///     Creates a string with the mech affinity information for the given pilot.
    /// </summary>
    /// <param name="pilot"> The pilot to get the mech affinity information for. </param>
    /// <returns> The string with the mech affinity information. </returns>
    public static string GetBioText(PersistentEntity pilot)
    {
        var mechAffinityList = MechAffinityHelper.GetMechAffinityList(pilot);
        if (mechAffinityList.Count == 0)
            return "\n";

        var stringToAdd = "\n\n";

        stringToAdd += "Mech Affinity:\n";
        foreach (var mechInternalName in mechAffinityList)
        {
            var mech = IDUtility.GetPersistentEntity(mechInternalName);
            if (mech == null)
            {
                MechAffinityHelper.ClearMechAffinity(pilot, mechInternalName);
                continue;
            }

            if (mech.hasUnitIdentification)
                stringToAdd +=
                    $"- {mech.unitIdentification.nameOverride}: {MechAffinityHelper.GetMechAffinity(pilot, mech)}\n";
        }

        foreach (var currentMech in from slot in Contexts.sharedInstance.persistent.squadComposition.slots
                 where slot.pilotNameInternal == pilot.nameInternal.s
                 select IDUtility.GetPersistentEntity(slot.unitNameInternal))
        {
            stringToAdd += "\n\n";
            
            var hasBonuses = false;
            var damageReduction = 1f - MechAffinityBonus.GetDamageReductionMultiplier(currentMech);
            if (damageReduction > 0)
            {
                hasBonuses = true;
            }
            
            if (hasBonuses) {
                stringToAdd += "Current mech bonuses:\n";
                stringToAdd +=
                    $"- Damage reduction: {damageReduction}%\n";
            } else {
                stringToAdd += "No affinity bonuses unlocked for piloted mech.";
            }
            
            break;
        }

        return stringToAdd + "\n\n";
    }
}