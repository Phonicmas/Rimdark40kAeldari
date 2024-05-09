using RimWorld;
using System.Collections.Generic;
using Verse;


namespace Aeldari40k
{
    public class Need_Meditation : Need
    {
        private const float MinAgeForNeed = 13f;

        protected override bool IsFrozen
        {
            get
            {
                if ((float)pawn.ageTracker.AgeBiologicalYears < MinAgeForNeed)
                {
                    return true;
                }
                return base.IsFrozen;
            }
        }

        public override bool ShowOnNeedList
        {
            get
            {
                if ((float)pawn.ageTracker.AgeBiologicalYears < MinAgeForNeed)
                {
                    return false;
                }
                return base.ShowOnNeedList;
            }
        }

        public Need_Meditation(Pawn newPawn)
            : base(newPawn)
        {
            threshPercents = new List<float> { 0.3f };
        }

        public override void NeedInterval()
        {
            if (!IsFrozen)
            {
                CurLevel = pawn.psychicEntropy.CurrentPsyfocus;
            }
        }
    }
}