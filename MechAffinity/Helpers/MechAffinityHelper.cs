using System.Collections.Generic;
using System.Linq;
using MechAffinity.Models;
using PhantomBrigade.Data;
using PhantomBrigade.Overworld;

namespace MechAffinity.Helpers;

public class MechAffinityHelper
{
    private static string Prefix => $"{MechAffinity.Name}_";
    private static string AffinityKeyPrefix => $"{Prefix}affinity_";
    private static string AffinityListKey => $"{Prefix}affinity_list";

    private static string AffinityKey(PersistentEntity mech)
    {
        return $"{AffinityKeyPrefix}{mech.nameInternal.s}";
    }

    public static float SetMechAffinity(PersistentEntity pilot, PersistentEntity mech, float affinity)
    {
        if (affinity > 0.0f)
            AddMechAffinityToList(pilot, mech);

        pilot.SetMemoryFloat(AffinityKey(mech), affinity);
        return affinity;
    }

    public static float GetMechAffinity(PersistentEntity pilot, PersistentEntity mech)
    {
        return pilot.TryGetMemoryFloat(AffinityKey(mech), out var affinity) ? affinity : 0.0f;
    }

    public static float AddMechAffinity(PersistentEntity pilot, PersistentEntity mech, float affinityToAdd)
    {
        AddMechAffinityToList(pilot, mech);
        return SetMechAffinity(pilot, mech, GetMechAffinity(pilot, mech) + affinityToAdd);
    }

    public static float RemoveMechAffinity(PersistentEntity pilot, PersistentEntity mech, float affinityToRemove)
    {
        var newAffinity = SetMechAffinity(pilot, mech, GetMechAffinity(pilot, mech) - affinityToRemove);
        if (newAffinity > 0.0f)
            return newAffinity;

        ClearMechAffinity(pilot, mech);
        RemoveMechAffinityFromList(pilot, mech);
        return 0.0f;
    }

    public static void ClearMechAffinity(PersistentEntity pilot, PersistentEntity mech)
    {
        pilot.RemoveMemoryFloat(AffinityKey(mech));
        RemoveMechAffinityFromList(pilot, mech);
    }

    public static List<string> GetMechAffinityList(PersistentEntity pilot)
    {
        if (!pilot.hasCustomMemory)
            return [];

        var affinityList = pilot.customMemory.s.FirstOrDefault(x => x.Key == AffinityListKey).Value;
        if (affinityList is CustomMemoryStringList affinityListValue)
            return affinityListValue.value;

        return [];
    }

    public static void SetMechAffinityList(PersistentEntity pilot, List<string> affinityList)
    {
        if (!pilot.hasCustomMemory)
        {
            pilot.AddCustomMemory(new SortedDictionary<string, CustomMemoryValue>
                { { AffinityListKey, new CustomMemoryStringList(affinityList) } });
            return;
        }

        var customMemory = pilot.customMemory.s;
        customMemory.Add(AffinityListKey, new CustomMemoryStringList(affinityList));
        pilot.ReplaceCustomMemory(customMemory);
    }

    public static void AddMechAffinityToList(PersistentEntity pilot, PersistentEntity mech)
    {
        var affinityList = GetMechAffinityList(pilot);
        if (affinityList.Contains(mech.nameInternal.s))
            return;

        affinityList.Add(mech.nameInternal.s);
        SetMechAffinityList(pilot, affinityList);
    }

    public static void RemoveMechAffinityFromList(PersistentEntity pilot, PersistentEntity mech)
    {
        var affinityList = GetMechAffinityList(pilot);
        if (!affinityList.Contains(mech.nameInternal.s))
            return;

        affinityList.Remove(mech.nameInternal.s);
        SetMechAffinityList(pilot, affinityList);
    }

    public static void ClearMechAffinityList(PersistentEntity pilot)
    {
        SetMechAffinityList(pilot, []);
    }

    public static bool HasMechAffinity(PersistentEntity pilot, PersistentEntity mech)
    {
        return GetMechAffinityList(pilot).Contains(mech.nameInternal.s);
    }
}