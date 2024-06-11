using RimWorld;
using Verse;


namespace Aeldari40k
{
    public class CompForceWeapon : ThingComp
    {
        public CompProperties_ForceWeapon Props => (CompProperties_ForceWeapon)props;

        private float cachedDamageValue = -1;
        private float cachedPenValue = -1;

        public override void Notify_UsedWeapon(Pawn pawn)
        {
            cachedDamageValue = pawn.GetStatValue(Props.damageScalingStat) * Props.damageScalingFactor;

            if (Props.scalesPen)
            {
                cachedPenValue = pawn.GetStatValue(Props.penScalingStat) * Props.penScaleFactor;
            }
            //Have a cached variable of the stats the extra weapon damage scales of on, check if they match if not put new value in and then change damage
            //of extra damage on weapon according to said stat and whatever multiplier
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref cachedDamageValue, "cachedDamageValue", -1);
            Scribe_Values.Look(ref cachedPenValue, "cachedPenValue", -1);
        }

    }
}