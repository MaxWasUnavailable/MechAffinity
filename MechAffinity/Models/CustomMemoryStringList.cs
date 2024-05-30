using System.Collections.Generic;
using PhantomBrigade.Data;

namespace MechAffinity.Models;

public class CustomMemoryStringList(List<string> stringList) : CustomMemoryValue
{
    public List<string> value = stringList;
}