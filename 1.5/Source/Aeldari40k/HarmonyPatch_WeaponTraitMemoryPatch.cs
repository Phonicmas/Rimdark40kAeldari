using HarmonyLib;
using RimWorld;
using Verse;

namespace Aeldari40k
{   
    [HarmonyPatch(typeof(SkillRecord), "Learn")]
    public class WeaponTraitMemoryPatch
    {
        public static void Prefix(ref float xp, Pawn ___pawn)
        {
            Thing bondedWeap = ___pawn.equipment.bondedWeapon;
            if (bondedWeap.HasComp<CompBladelinkWeapon_AeldariSpirit>())
            {
                if (bondedWeap.TryGetComp<CompBladelinkWeapon_AeldariSpirit>().TraitsListForReading.Contains(Aeldari40kDefOf.BEWH_SpiritTraitGreatMemory))
                {
                    xp *= 0.75f;
                }
            }
        }
    }
}