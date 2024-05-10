using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Aeldari40k
{
    public class RecipeWorker_MakeSpiritWeapon : RecipeWorker
    {
        public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
        {
            Thing weapon = GenSpawn.Spawn(recipe.GetModExtension<DefModExtension_WraithboneCreatedStuff>().createdThing, billDoer.PositionHeld, billDoer.MapHeld);

            Thing spiritStone = ingredients.Find(x => x.HasComp<SpiritStoneComp>());

            CompBladelinkWeapon_AeldariSpirit weaponComp = weapon.TryGetComp<CompBladelinkWeapon_AeldariSpirit>();
            weaponComp.spirit = spiritStone.TryGetComp<SpiritStoneComp>().pawn;
            weaponComp.InitializeTraits();  
        }
    }
}