using RimWorld;
using System;
using Verse;

namespace Aeldari40k
{
    public class ThoughtWorker_SanguineWeaponTrait : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            CompBladelinkWeapon_AeldariSpirit comp = p.equipment.bondedWeapon.TryGetComp<CompBladelinkWeapon_AeldariSpirit>();
            if (comp == null)
            {
                return false;
            }
            return comp.TraitsListForReading.Contains(Aeldari40kDefOf.BEWH_SpiritTraitSanguine);
        }

    }
}