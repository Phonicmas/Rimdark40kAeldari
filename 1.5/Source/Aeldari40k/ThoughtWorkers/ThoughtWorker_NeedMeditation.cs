using RimWorld;
using System;
using Verse;

namespace Aeldari40k
{
    public class ThoughtWorker_NeedMeditation : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.needs.TryGetNeed(Aeldari40kDefOf.BEWH_MeditationDependency) == null)
            {
                return ThoughtState.Inactive;
            }

            float psyfocusLevel = p.needs.TryGetNeed(Aeldari40kDefOf.BEWH_MeditationDependency).CurLevelPercentage;

            if (psyfocusLevel >= 0.95f)
            {
                return ThoughtState.ActiveAtStage(0);
            }
            else if (psyfocusLevel >= 0.8f)
            {
                return ThoughtState.ActiveAtStage(1);
            }
            else if (psyfocusLevel >= 0.75f)
            {
                return ThoughtState.ActiveAtStage(2);
            }
            else if (psyfocusLevel >= 0.5f)
            {
                return ThoughtState.ActiveAtStage(3);
            }
            else if (psyfocusLevel >= 0.2f)
            {
                return ThoughtState.ActiveAtStage(4);
            }
            else if (psyfocusLevel >= 0.02f)
            {
                return ThoughtState.ActiveAtStage(5);
            }

            return ThoughtState.Inactive;
        }

    }
}