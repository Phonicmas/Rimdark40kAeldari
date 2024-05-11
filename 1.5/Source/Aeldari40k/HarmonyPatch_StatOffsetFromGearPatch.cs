using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Aeldari40k
{
    [HarmonyPatch(typeof(StatWorker), "StatOffsetFromGear")]
    public class StatOffsetFromGearPatch
    {
        public static void Postfix(ref float __result, Thing gear, StatDef stat)
        {
            float val = 0;
            //float val = gear.def.equippedStatOffsets.GetStatOffsetFromList(stat);
            CompBladelinkWeapon_AeldariSpirit compBladelinkWeapon = gear.TryGetComp<CompBladelinkWeapon_AeldariSpirit>();
            if (compBladelinkWeapon != null)
            {
                List<WeaponTraitDef_AeldariSpirit> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
                for (int i = 0; i < traitsListForReading.Count; i++)
                {
                    val += traitsListForReading[i].equippedStatOffsets.GetStatOffsetFromList(stat);
                }
            }
            if (Math.Abs(val) > float.Epsilon && !stat.parts.NullOrEmpty())
            {
                foreach (StatPart part in stat.parts)
                {
                    part.TransformValue(StatRequest.For(gear), ref val);
                }
                __result += val;
            }
            __result += val;
        }
    }
}