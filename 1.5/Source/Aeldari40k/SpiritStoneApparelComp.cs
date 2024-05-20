using Verse;


namespace Aeldari40k
{
    public class SpiritStoneApparelComp : ThingComp
    {
        public SpiritStoneApparelCompProperties Props => (SpiritStoneApparelCompProperties)props;

        public Pawn pawn;

        public override void Notify_WearerDied()
        {
            base.Notify_WearerDied();

            if (pawn.genes == null || !pawn.genes.HasGene(Aeldari40kDefOf.BEWH_AeldariCurseOfSlaanesh))
            {
                return;
            }

            ((Gene_CurseOfSlaanesh)pawn.genes.GetGene(Aeldari40kDefOf.BEWH_AeldariCurseOfSlaanesh)).hadSpiritStone = true;

            Thing spiritStone = GenSpawn.Spawn(Aeldari40kDefOf.BEWH_SpiritStone, parent.PositionHeld, parent.MapHeld);

            spiritStone.TryGetComp<SpiritStoneComp>().pawn = pawn;

            if (parent != null && !parent.Destroyed)
            {
                parent.Destroy();
            }
        }

        public override void Notify_Equipped(Pawn pawn)
        {
            if (this.pawn == null)
            {
                this.pawn = pawn;
            }
            if (pawn.genes != null && pawn.genes.HasGene(Aeldari40kDefOf.BEWH_AeldariCurseOfSlaanesh))
            {
                ((Gene_CurseOfSlaanesh)pawn.genes.GetGene(Aeldari40kDefOf.BEWH_AeldariCurseOfSlaanesh)).carryingSpiritStone = true;
            }
        }

        public override void Notify_Unequipped(Pawn pawn)
        {
            this.pawn = null;
            if (pawn.genes != null && pawn.genes.HasGene(Aeldari40kDefOf.BEWH_AeldariCurseOfSlaanesh))
            {
                ((Gene_CurseOfSlaanesh)pawn.genes.GetGene(Aeldari40kDefOf.BEWH_AeldariCurseOfSlaanesh)).carryingSpiritStone = false;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look(ref pawn, "pawn");
        }

    }
}