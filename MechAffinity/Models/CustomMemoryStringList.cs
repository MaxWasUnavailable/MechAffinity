using System.Collections.Generic;
using Content.Code.Utility;
using PhantomBrigade.Data;

namespace MechAffinity.Models;

#nullable disable

/// <summary>
///     A custom memory value that stores a list of strings.
/// </summary>
[TypeHinted]
public class CustomMemoryStringList : CustomMemoryValue
{
    // ReSharper disable once InconsistentNaming
    public List<string> value;
}