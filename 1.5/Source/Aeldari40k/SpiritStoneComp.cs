using Verse;


namespace Aeldari40k
{
    public class SpiritStoneComp : ThingComp
    {
        public SpiritStoneCompProperties Props => (SpiritStoneCompProperties)props;

        public Pawn pawn = null;

        public override string CompInspectStringExtra()
        {
            if (pawn != null)
            {
                return "ContainsSoulOf".Translate(pawn.NameFullColored);
            }
            return "ContainsNoSoul".Translate();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look(ref pawn, "pawn", saveDestroyedThings: true);
        }

    }
}