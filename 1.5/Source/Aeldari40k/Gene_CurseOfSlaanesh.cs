using System.Collections.Generic;
using Verse;


namespace Aeldari40k
{
    public class Gene_CurseOfSlaanesh : Gene
    {
        private bool hasDiedBefore = false;

        public bool hadSpiritStone = false;

        public bool carryingSpiritStone = false;

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);
            hasDiedBefore = true;

            Map map = Find.AnyPlayerHomeMap;
            Map map2 = pawn.Map;

            List<Pawn> pawns = new List<Pawn>();

            if (map != null)
            {
                pawns.AddRange(map.mapPawns.FreeColonists);
            }
            if (map2 != null && map2 != map)
            {
                pawns.AddRange(map2.mapPawns.FreeColonists);
            }


            foreach (Pawn colonist in pawns)
            {
                if (!colonist.Dead && colonist.genes != null && colonist.genes.HasActiveGene(Aeldari40kDefOf.BEWH_AeldariCurseOfSlaanesh))
                {
                    if (hadSpiritStone)
                    {
                        colonist.needs.mood.thoughts.memories.TryGainMemory(Aeldari40kDefOf.BEWH_SoulStoneDeathThought);
                    }
                    else
                    {
                        colonist.needs.mood.thoughts.memories.TryGainMemory(Aeldari40kDefOf.BEWH_NoSoulStoneDeathThought);
                    }
                }
            }

        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref hasDiedBefore, "hasDiedBefore", false);
            Scribe_Values.Look(ref hadSpiritStone, "hadSpiritStone", false);
            Scribe_Values.Look(ref carryingSpiritStone, "carryingSpiritStone", false);
        }
    }
}