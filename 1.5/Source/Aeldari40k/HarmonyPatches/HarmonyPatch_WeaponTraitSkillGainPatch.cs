using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace Aeldari40k
{   
    [HarmonyPatch(typeof(SkillRecord), "Aptitude", MethodType.Getter)]
    public class WeaponTraitSkillGainPatch
    {
        public static void Postfix(Pawn ___pawn, ref int __result, SkillRecord __instance)
        {
            Thing bondedWeap = ___pawn.equipment?.bondedWeapon;
            if (bondedWeap.HasComp<CompBladelinkWeapon_AeldariSpirit>())
            {
                foreach (WeaponTraitDef_AeldariSpirit trait in bondedWeap.TryGetComp<CompBladelinkWeapon_AeldariSpirit>().TraitsListForReading)
                {
                    foreach (SkillGain skillGain in trait.skillGains)
                    {
                        if (skillGain.skill == __instance.def)
                        {
                            __result += skillGain.amount;
                        }
                    }
                }
            }
        }
    }
}