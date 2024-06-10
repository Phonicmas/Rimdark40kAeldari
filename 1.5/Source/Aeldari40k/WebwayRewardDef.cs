using RimWorld;
using System.Collections.Generic;
using Verse;


namespace Aeldari40k
{
    public class WebwayRewardDef : Def
    {
        public ThingDef rewardThing = null;
        public int rewardThingCount = 1;

        public bool giveQuality = false;
        public QualityCategory rewardCategoryMinimum = QualityCategory.Normal;

        public PawnKindDef rewardPawn = null;

        public bool showAmountInTooltip = true;
    }
}