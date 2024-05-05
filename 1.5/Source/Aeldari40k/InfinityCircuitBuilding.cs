using RimWorld;
using System.Linq;
using Verse;


namespace Aeldari40k
{
    public class InfinityCircuitBuilding : Building_Storage
    {
        public int SoulAmount
        {
            get
            {
                //Get storage amount of spirit stones, might be located in this.storageGroup
                int c = this.storageGroup.HeldThings.Count();
                Log.Message("Amount: " + c);
                return c;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            //Scribe_Collections.Look(ref heldStones, "heldStones", LookMode.Deep);
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