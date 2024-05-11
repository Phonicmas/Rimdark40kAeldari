using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Aeldari40k
{
    [HarmonyPatch(typeof(StatDef), "PopulateMutableStats")]
    public class PopulateMutableStatsPatch
    {
        public static void Postfix(ref HashSet<StatDef> ___mutableStats)
        {
            foreach (WeaponTraitDef_AeldariSpirit item6 in DefDatabase<WeaponTraitDef_AeldariSpirit>.AllDefsListForReading)
            {
                if (item6.equippedStatOffsets != null)
                {
                    ___mutableStats.AddRange(item6.equippedStatOffsets.Select((StatModifier mod) => mod.stat));
                }
            }
        }
        
    }
}