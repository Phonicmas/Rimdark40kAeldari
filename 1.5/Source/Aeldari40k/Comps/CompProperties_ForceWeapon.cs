using RimWorld;
using Verse;


namespace Aeldari40k
{
    public class CompProperties_ForceWeapon : CompProperties
    {
        public StatDef damageScalingStat;
        public float damageScalingFactor = 1;
        public DamageDef extraDamageDef;

        public StatDef penScalingStat;
        public bool scalesPen = false;
        public float penScaleFactor = 1;

        public CompProperties_ForceWeapon()
        {
            compClass = typeof(CompForceWeapon);
        }
    }
}