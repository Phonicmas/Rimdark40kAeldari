using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Aeldari40k
{
    public class RecipeWorker_MakeWraithbone : RecipeWorker
    {
        public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
        {
            int defaultAmount = recipe.GetModExtension<DefModExtension_Wraithbone>().defaultAmount;
            float mult = billDoer.GetStatValue(StatDefOf.PsychicSensitivity);

            int finalAmount = (int)(defaultAmount * mult);


            Thing wraitbone = GenSpawn.Spawn(Aeldari40kDefOf.BEWH_Wraithbone, billDoer.PositionHeld, billDoer.MapHeld);

            wraitbone.stackCount = finalAmount;
        }
    }
}