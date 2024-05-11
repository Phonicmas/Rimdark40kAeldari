using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Aeldari40k
{
    [HarmonyPatch(typeof(GenRecipe), "MakeRecipeProducts")]
    public class DontPlaceProductPatch
    {
        public static bool Prefix(ref IEnumerable<Thing> __result, RecipeDef recipeDef)
        {
            if (recipeDef.HasModExtension<DefModExtension_WraithboneCreatedStuff>())
            {
				__result = new List<Thing>();
                return false;
            }
            return true;
        }
    }
}