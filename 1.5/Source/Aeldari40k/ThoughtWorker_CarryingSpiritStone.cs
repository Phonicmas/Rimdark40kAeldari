using RimWorld;
using Verse;

namespace Aeldari40k
{
    public class ThoughtWorker_CarryingSpiritStone : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            int thoughtState = 0;
            if (p.genes != null && p.genes.HasGene(Aeldari40kDefOf.BEWH_AeldariCurseOfSlaanesh))
            {
                thoughtState = ((Gene_CurseOfSlaanesh)p.genes.GetGene(Aeldari40kDefOf.BEWH_AeldariCurseOfSlaanesh)).carryingSpiritStone ? 1 : 0;
            }
            return ThoughtState.ActiveAtStage(thoughtState);
        }
    }
}