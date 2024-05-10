using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Aeldari40k
{
    public class RecipeWorker_MakeWraithbone : RecipeWorker
    {
        public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
        {
            DefModExtension_WraithboneCreatedStuff defMod = recipe.GetModExtension<DefModExtension_WraithboneCreatedStuff>();
            int defaultAmount = defMod.defaultAmount;
            float mult = billDoer.GetStatValue(StatDefOf.PsychicSensitivity) * defMod.psyMult;

            int finalAmount = (int)(defaultAmount * mult);


            Thing wraitbone = GenSpawn.Spawn(defMod.createdThing, billDoer.PositionHeld, billDoer.MapHeld);

            wraitbone.stackCount = finalAmount;
        }
    }
}