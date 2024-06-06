using RimWorld;
using Verse;
using Verse.AI;

namespace Aeldari40k
{
    public class WorkGiver_BonesingerDoBill : WorkGiver_DoBill
    {
        public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
        {
            if (pawn.genes == null)
            {
                return null;
            }
            if (!pawn.genes.HasActiveGene(Aeldari40kDefOf.BEWH_AeldariPsyker))
            {
                return null;
            }
            if (pawn.GetStatValue(StatDefOf.PsychicSensitivity) <= 0)
            {
                return null;
            }
            return base.JobOnThing(pawn, thing, forced);
        }

    }
}