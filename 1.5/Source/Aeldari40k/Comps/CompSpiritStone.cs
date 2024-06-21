using Verse;


namespace Aeldari40k
{
    public class CompSpiritStone : ThingComp
    {
        public CompProperties_SpiritStone Props => (CompProperties_SpiritStone)props;

        public Pawn pawn = null;

        public override string CompInspectStringExtra()
        {
            if (pawn != null)
            {
                return "BEWH.ContainsSoulOf".Translate(pawn.NameFullColored);
            }
            return "BEWH.ContainsNoSoul".Translate();
        }

        public override void CompTickLong()
        {
            if (pawn == null)
            {
                parent.Destroy();
            }
            base.CompTickLong();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look(ref pawn, "pawn", saveDestroyedThings: true);
        }

    }
}