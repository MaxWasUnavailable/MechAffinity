using System.Collections.Generic;
using Content.Code.Utility;
using PhantomBrigade.Data;

namespace MechAffinity.Models;

#nullable disable
[TypeHinted]
public class CustomMemoryStringList : CustomMemoryValue
{
    // ReSharper disable once InconsistentNaming
    public List<string> value;
}