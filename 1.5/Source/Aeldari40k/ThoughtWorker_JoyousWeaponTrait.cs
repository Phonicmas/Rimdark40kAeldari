using RimWorld;
using System;
using Verse;

namespace Aeldari40k
{
    public class ThoughtWorker_JoyousWeaponTrait : ThoughtWorker
    {
        protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
        {
            if (!p.RaceProps.Humanlike)
            {
                return false;
            }
            if (!RelationsUtility.PawnsKnowEachOther(p, other))
            {
                return false;
            }
            foreach (var item in other.equipment.AllEquipmentListForReading)
            {
                if (item.HasComp<CompBladelinkWeapon_AeldariSpirit>())
                {
                    if (item.GetComp<CompBladelinkWeapon_AeldariSpirit>().TraitsListForReading.Contains(Aeldari40kDefOf.BEWH_SpiritTraitJoyous))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}