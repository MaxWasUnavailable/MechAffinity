using System.Collections.Generic;
using System.Linq;
using MechAffinity.Models;
using PhantomBrigade.Data;
using PhantomBrigade.Overworld;

namespace MechAffinity.Helpers;

public static class MechAffinityHelper
{
    private static string Prefix => $"{MechAffinity.Name}_";
    private static string AffinityKeyPrefix => $"{Prefix}affinity_";
    private static string AffinityListKey => $"{Prefix}affinity_list";

    private static string AffinityKey(this PersistentEntity mech)
    {
        return $"{AffinityKeyPrefix}{mech.nameInternal.s}";
    }

    public static float SetMechAffinity(this PersistentEntity pilot, PersistentEntity mech, float affinity)
    {
        if (affinity > 0.0f)
            AddMechAffinityToList(pilot, mech);

        pilot.SetMemoryFloat(AffinityKey(mech), affinity);
        return affinity;
    }

    public static float GetMechAffinity(this PersistentEntity pilot, PersistentEntity mech)
    {
        return pilot.TryGetMemoryFloat(AffinityKey(mech), out var affinity) ? affinity : 0.0f;
    }

    public static float AddMechAffinity(this PersistentEntity pilot, PersistentEntity mech, float affinityToAdd)
    {
        AddMechAffinityToList(pilot, mech);
        return SetMechAffinity(pilot, mech, GetMechAffinity(pilot, mech) + affinityToAdd);
    }

    public static float ReduceMechAffinity(this PersistentEntity pilot, PersistentEntity mech, float affinityToRemove)
    {
        var newAffinity = SetMechAffinity(pilot, mech, GetMechAffinity(pilot, mech) - affinityToRemove);
        if (newAffinity > 0.0f)
            return newAffinity;

        ClearMechAffinity(pilot, mech);
        RemoveMechAffinityFromList(pilot, mech);
        return 0.0f;
    }

    public static void ClearMechAffinity(this PersistentEntity pilot, PersistentEntity mech)
    {
        pilot.RemoveMemoryFloat(AffinityKey(mech));
        RemoveMechAffinityFromList(pilot, mech);
    }

    public static List<string> GetMechAffinityList(this PersistentEntity pilot)
    {
        if (!pilot.hasCustomMemory)
            return [];

        var affinityList = pilot.customMemory.s.FirstOrDefault(x => x.Key == AffinityListKey).Value;
        if (affinityList is CustomMemoryStringList affinityListValue)
            return affinityListValue.value;

        return [];
    }

    public static void SetMechAffinityList(this PersistentEntity pilot, List<string> affinityList)
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

    public static void AddMechAffinityToList(this PersistentEntity pilot, PersistentEntity mech)
    {
        var affinityList = GetMechAffinityList(pilot);
        if (affinityList.Contains(mech.nameInternal.s))
            return;

        affinityList.Add(mech.nameInternal.s);
        SetMechAffinityList(pilot, affinityList);
    }

    public static void RemoveMechAffinityFromList(this PersistentEntity pilot, PersistentEntity mech)
    {
        var affinityList = GetMechAffinityList(pilot);
        if (!affinityList.Contains(mech.nameInternal.s))
            return;

        affinityList.Remove(mech.nameInternal.s);
        SetMechAffinityList(pilot, affinityList);
    }

    public static void ClearMechAffinityList(this PersistentEntity pilot)
    {
        SetMechAffinityList(pilot, []);
    }

    public static bool HasMechAffinity(this PersistentEntity pilot, PersistentEntity mech)
    {
        return GetMechAffinityList(pilot).Contains(mech.nameInternal.s);
    }
}