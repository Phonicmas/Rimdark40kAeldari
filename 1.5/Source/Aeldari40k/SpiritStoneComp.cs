using Verse;


namespace Aeldari40k
{
    public class SpiritStoneComp : ThingComp
    {
        public SpiritStoneCompProperties Props => (SpiritStoneCompProperties)props;

        private Pawn pawn = null;

        public Pawn SoulOfPawn
        {
            get
            {
                if(this.pawn == null && ParentHolder is Pawn pawn)
                {
                    this.pawn = pawn;
                }
                return this.pawn;
            }
        }

        public override void Notify_WearerDied()
        {
            base.Notify_WearerDied();

            Thing spiritStone = ThingMaker.MakeThing(Aeldari40kDefOf.BEWH_SpiritStone);
            spiritStone.Position = parent.Position;
            if (parent != null && !parent.Destroyed)
            {
                parent.Destroy();
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look(ref pawn, "pawn");
        }


    }
}