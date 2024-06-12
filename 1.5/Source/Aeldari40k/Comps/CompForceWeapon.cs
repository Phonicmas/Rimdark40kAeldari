using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;


namespace Aeldari40k
{
    public class CompForceWeapon : ThingComp
    {
        public CompProperties_ForceWeapon Props => (CompProperties_ForceWeapon)props;

        private float statValue = 1;

        private float cachedDamageValue = 0;

        private float cachedPenValue = 0;


        public override void Notify_Equipped(Pawn pawn)
        {
            base.Notify_Equipped(pawn);
            CalculateExtraDamage(pawn);
        }

        public override void Notify_UsedWeapon(Pawn pawn)
        {
            if (pawn.GetStatValue(Props.scalingStat) != statValue)
            {
                CalculateExtraDamage(pawn);
            }
        }

        private void CalculateExtraDamage(Pawn pawn)
        {
            if (pawn == null && pawn.GetStatValue(Props.scalingStat) <= 0)
            {
                return;
            }

            statValue = pawn.GetStatValue(Props.scalingStat);

            cachedDamageValue = statValue * Props.damageScalingFactor;

            if (Props.scalesPen)
            {
                cachedPenValue = pawn.GetStatValue(Props.scalingStat) * Props.penScaleFactor;
            }

            if (parent.TryGetComp<CompEquippable>().Tools != null)
            {
                foreach (Tool tool in parent.TryGetComp<CompEquippable>().Tools)
                {
                    if (!tool.extraMeleeDamages.NullOrEmpty())
                    {
                        IEnumerable<ExtraDamage> tools = tool.extraMeleeDamages.Where(x => x.def.HasModExtension<DefModExtension_ScalingDamage>());
                        if (!tools.EnumerableNullOrEmpty())
                        {
                            foreach (ExtraDamage extraDamage in tools)
                            {
                                extraDamage.amount = cachedDamageValue;
                                extraDamage.armorPenetration = cachedPenValue;
                            }
                        }
                    }
                }
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref statValue, "statValue", 1);
            Scribe_Values.Look(ref cachedDamageValue, "cachedDamageValue", 0);
            Scribe_Values.Look(ref cachedPenValue, "cachedPenValue", 0);
        }

    }
}