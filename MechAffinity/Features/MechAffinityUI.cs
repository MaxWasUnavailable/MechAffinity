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
        var stringToAdd = "\n\n";

        var mechAffinityList = MechAffinityHelper.GetMechAffinityList(pilot);
        if (mechAffinityList.Count == 0)
            return stringToAdd;

        PersistentEntity? currentMech = null;

        stringToAdd += "Mech Affinity:\n";
        foreach (var mechInternalName in mechAffinityList)
        {
            var mech = IDUtility.GetPersistentEntity(mechInternalName);
            if (mech == null)
            {
                MechAffinityHelper.ClearMechAffinity(pilot, mechInternalName);
                continue;
            }

            stringToAdd +=
                $"- {mech.unitIdentification.nameOverride}: {MechAffinityHelper.GetMechAffinity(pilot, mech)}\n";

            if (mech.entityLinkPilot.persistentID == pilot.id.id)
                currentMech = mech;
        }

        if (currentMech != null)
        {
            stringToAdd += "\n\n";
            stringToAdd += "Current mech bonuses:\n";
            stringToAdd += $"- Damage reduction: {MechAffinityBonus.GetDamageReductionMultiplier(currentMech) * 100}%";
        }

        return stringToAdd + "\n\n";
    }
}