using System.Collections.Generic;
using System.Linq;
using MechAffinity.Models;
using PhantomBrigade;
using PhantomBrigade.Data;
using PhantomBrigade.Overworld;

namespace MechAffinity.Helpers;

/// <summary>
///     Helper class for managing mech affinities.
/// </summary>
public static class MechAffinityHelper
{
    /// <summary>
    ///     Prefix for all memory keys used by the mod's affinity system.
    /// </summary>
    private static string Prefix => $"{MechAffinity.Name}_";
    
    /// <summary>
    ///     Prefix for all mech affinity keys.
    /// </summary>
    private static string AffinityKeyPrefix => $"{Prefix}affinity_";
    
    /// <summary>
    ///     Key for the affinity list in the pilot's custom memory.
    /// </summary>
    private static string AffinityListKey => $"{Prefix}affinity_list";

    /// <summary>
    ///     Key for a pilot's affinity with a specific mech.
    /// </summary>
    /// <param name="mech"> The mech to get the affinity key for. </param>
    /// <returns> The key for a pilot's affinity with the specified mech. </returns>
    private static string AffinityKey(PersistentEntity mech)
    {
        return $"{AffinityKeyPrefix}{mech.nameInternal.s}";
    }

    /// <summary>
    ///     Sets the affinity of a pilot for a mech.
    /// </summary>
    /// <param name="pilot"> The pilot to set the affinity for. </param>
    /// <param name="mech"> The mech to set the affinity for. </param>
    /// <param name="affinity"> The affinity to set. </param>
    /// <returns> The affinity set. </returns>
    public static float SetMechAffinity(PersistentEntity pilot, PersistentEntity mech, float affinity)
    {
        if (affinity > 0.0f)
            AddMechAffinityToList(pilot, mech);

        pilot.SetMemoryFloat(AffinityKey(mech), affinity);
        return affinity;
    }

    /// <summary>
    ///     Gets the affinity of a pilot for a mech.
    /// </summary>
    /// <param name="pilot"> The pilot to get the affinity for. </param>
    /// <param name="mech"> The mech to get the affinity for. </param>
    /// <returns> The affinity of the pilot for the mech. </returns>
    public static float GetMechAffinity(PersistentEntity pilot, PersistentEntity mech)
    {
        return pilot.TryGetMemoryFloat(AffinityKey(mech), out var affinity) ? affinity : 0.0f;
    }

    /// <summary>
    ///     Adds affinity to a pilot for a mech.
    /// </summary>
    /// <param name="pilot"> The pilot to add affinity to. </param>
    /// <param name="mech"> The mech to add affinity for. </param>
    /// <param name="affinityToAdd"> The affinity to add. </param>
    /// <returns> The new affinity of the pilot for the mech. </returns>
    public static float AddMechAffinity(PersistentEntity pilot, PersistentEntity mech, float affinityToAdd)
    {
        AddMechAffinityToList(pilot, mech);
        return SetMechAffinity(pilot, mech, GetMechAffinity(pilot, mech) + affinityToAdd);
    }

    /// <summary>
    ///     Reduces the affinity of a pilot for a mech.
    /// </summary>
    /// <param name="pilot"> The pilot to reduce the affinity for. </param>
    /// <param name="mech"> The mech to reduce the affinity for. </param>
    /// <param name="affinityToRemove"> The affinity to remove. </param>
    /// <returns> The new affinity of the pilot for the mech. </returns>
    public static float ReduceMechAffinity(PersistentEntity pilot, PersistentEntity mech, float affinityToRemove)
    {
        var newAffinity = SetMechAffinity(pilot, mech, GetMechAffinity(pilot, mech) - affinityToRemove);
        if (newAffinity > 0.0f)
            return newAffinity;

        ClearMechAffinity(pilot, mech);
        RemoveMechAffinityFromList(pilot, mech);
        return 0.0f;
    }

    /// <summary>
    ///     Clears the affinity of a pilot for a mech.
    /// </summary>
    /// <param name="pilot"> The pilot to clear the affinity for. </param>
    /// <param name="mech"> The mech to clear the affinity for. </param>
    public static void ClearMechAffinity(PersistentEntity pilot, PersistentEntity mech)
    {
        pilot.RemoveMemoryFloat(AffinityKey(mech));
        RemoveMechAffinityFromList(pilot, mech);
    }

    /// <summary>
    ///     Gets the list of mech internal names a pilot has affinity with.
    /// </summary>
    /// <param name="pilot"> The pilot to get the mech affinity list for. </param>
    /// <returns> The list of mech internal names the pilot has affinity with. </returns>
    public static List<string> GetMechAffinityList(PersistentEntity pilot)
    {
        if (!pilot.hasCustomMemory)
            return [];

        var affinityList = pilot.customMemory.s.FirstOrDefault(x => x.Key == AffinityListKey).Value;
        if (affinityList is CustomMemoryStringList affinityListValue)
            return affinityListValue.value;

        return [];
    }

    /// <summary>
    ///     Sets the list of mech internal names a pilot has affinity with.
    /// </summary>
    /// <param name="pilot"> The pilot to set the mech affinity list for. </param>
    /// <param name="affinityList"> The list of mech internal names to set. </param>
    public static void SetMechAffinityList(PersistentEntity pilot, List<string> affinityList)
    {
        if (!pilot.hasCustomMemory)
        {
            pilot.AddCustomMemory(new SortedDictionary<string, CustomMemoryValue>
                { { AffinityListKey, new CustomMemoryStringList { value = affinityList } } });
            return;
        }

        var customMemory = pilot.customMemory.s;
        customMemory[AffinityListKey] = new CustomMemoryStringList { value = affinityList };
        pilot.ReplaceCustomMemory(customMemory);
    }

    /// <summary>
    ///     Adds a mech to a pilot's mech affinity list.
    /// </summary>
    /// <param name="pilot"> The pilot to add the mech to. </param>
    /// <param name="mech"> The mech to add to the pilot's mech affinity list. </param>
    public static void AddMechAffinityToList(PersistentEntity pilot, PersistentEntity mech)
    {
        var affinityList = GetMechAffinityList(pilot);
        if (affinityList.Contains(mech.nameInternal.s))
            return;

        affinityList.Add(mech.nameInternal.s);
        SetMechAffinityList(pilot, affinityList);
    }

    /// <summary>
    ///     Removes a mech from a pilot's mech affinity list.
    /// </summary>
    /// <param name="pilot"> The pilot to remove the mech from. </param>
    /// <param name="mech"> The mech to remove from the pilot's mech affinity list. </param>
    public static void RemoveMechAffinityFromList(PersistentEntity pilot, PersistentEntity mech)
    {
        var affinityList = GetMechAffinityList(pilot);
        if (!affinityList.Contains(mech.nameInternal.s))
            return;

        affinityList.Remove(mech.nameInternal.s);
        SetMechAffinityList(pilot, affinityList);
    }

    /// <summary>
    ///     Clears a pilot's mech affinity list.
    /// </summary>
    /// <param name="pilot"> The pilot to clear the mech affinity list for. </param>
    public static void ClearMechAffinityList(PersistentEntity pilot)
    {
        SetMechAffinityList(pilot, []);
    }

    /// <summary>
    ///     Checks if a pilot has affinity with a mech.
    /// </summary>
    /// <param name="pilot"> The pilot to check for mech affinity. </param>
    /// <param name="mech"> The mech to check for affinity with. </param>
    /// <returns> True if the pilot has affinity with the mech, false otherwise. </returns>
    public static bool HasMechAffinity(PersistentEntity pilot, PersistentEntity mech)
    {
        return GetMechAffinityList(pilot).Contains(mech.nameInternal.s);
    }
}