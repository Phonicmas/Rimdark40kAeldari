using Verse;


namespace Aeldari40k
{
    public class SpiritStoneComp : ThingComp
    {
        public SpiritStoneCompProperties Props => (SpiritStoneCompProperties)props;

        public Pawn SoulOfPawn
        {
            get
            {
                if(ParentHolder is Pawn pawn)
                {
                    return pawn;
                }
                return null;
            }
        }


    }
}