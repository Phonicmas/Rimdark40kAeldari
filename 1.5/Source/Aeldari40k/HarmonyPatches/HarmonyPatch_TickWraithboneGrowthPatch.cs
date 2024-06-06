using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace Aeldari40k
{   
    [HarmonyPatch(typeof(JobDriver_Meditate), "MeditationTick")]
    public class TickWraithboneGrowthPatch
    {
        public static void Postfix(Pawn ___pawn)
        {
            if (ModsConfig.RoyaltyActive && Aeldari40kDefOf.BEWH_Bonesinger.CanPawnUse(___pawn))
            {
                int num = GenRadial.NumCellsInRadius(MeditationUtility.FocusObjectSearchRadius);
                for (int i = 0; i < num; i++)
                {
                    IntVec3 c = ___pawn.Position + GenRadial.RadialPattern[i];
                    if (c.InBounds(___pawn.Map))
                    {
                        List<Thing> thingList = c.GetThingList(___pawn.Map);

                        for (int j = 0; j < thingList.Count; j++)
                        {
                            if (thingList[j].def == Aeldari40kDefOf.BEWH_WraithboneBenchMeditation)
                            {
                                thingList[j].TryGetComp<CompMeditationSpawn>()?.AddProgress(___pawn);
                            }
                        }
                    }
                }
            }
        }
    }
}