using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Aeldari40k
{
    [HarmonyPatch(typeof(StatWorker), "GearHasCompsThatAffectStat")]
    public class GearHasCompsThatAffectStatPatch
    {
        public static void Postfix(ref bool __result, Thing gear, StatDef stat)
        {
            if (__result == true)
            {
                return;
            }
            CompBladelinkWeapon_AeldariSpirit compBladelinkWeapon = gear.TryGetComp<CompBladelinkWeapon_AeldariSpirit>();
            if (compBladelinkWeapon == null)
            {
                __result = false;
                return;
            }
            List<WeaponTraitDef_AeldariSpirit> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
            for (int i = 0; i < traitsListForReading.Count; i++)
            {
                if (traitsListForReading[i].equippedStatOffsets.NullOrEmpty())
                {
                    continue;
                }
                for (int j = 0; j < traitsListForReading[i].equippedStatOffsets.Count; j++)
                {
                    StatModifier statModifier = traitsListForReading[i].equippedStatOffsets[j];
                    if (statModifier.stat == stat && statModifier.value != 0f)
                    {
                        __result = true;
                        return;
                    }
                }
            }
            __result = false;
            return;
        }
    }
}