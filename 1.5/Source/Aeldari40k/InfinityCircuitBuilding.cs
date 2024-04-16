using System.Collections.Generic;
using Verse;


namespace Aeldari40k
{
    public class InfinityCircuitBuilding : Building
    {

        public List<ThingDef> heldStones = new List<ThingDef>();

        public int SoulAmount
        {
            get { return heldStones.Count; }
        }

        public override void TickLong()
        {
            base.TickLong();
            if (SoulAmount >= 1)
            {
                foreach (Pawn pawn in Map.mapPawns.FreeColonists)
                {
                    if (pawn.genes != null && pawn.genes.HasGene(Aeldari40kDefOf.BEWH_AeldariPsyker))
                    {
                        pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(Aeldari40kDefOf.BEWH_InfinityCircuitThought);
                    }
                }
            }
            
        }

    }
}