using HarmonyLib;
using Verse;


namespace Aeldari40k
{
    public class Aeldari40kModSettings : ModSettings
    {
        public int aeldariWebwayGateRewardCount = 3;
        public float aeldariWebwayGateTravelTimeTicks = 30000;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref aeldariWebwayGateRewardCount, "aeldariWebwayGateRewardCount", 3);
            Scribe_Values.Look(ref aeldariWebwayGateTravelTimeTicks, "aeldariWebwayGateTravelTimeTicks", 30000);
            base.ExposeData();
        }
    }
}