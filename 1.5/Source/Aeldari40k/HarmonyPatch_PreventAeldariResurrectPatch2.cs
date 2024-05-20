using Core40k;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Aeldari40k
{   
    [HarmonyPatch(typeof(ResurrectionUtility), "TryResurrectWithSideEffects")]
    public class PreventAeldariRessurectPatch2
    {
        public static bool Prefix(Pawn pawn)
        {
            if (Prefs.DevMode && DebugSettings.godMode)
            {
                return true;
            }
            if (pawn.genes != null && pawn.genes.HasGene(Aeldari40kDefOf.BEWH_AeldariCurseOfSlaanesh))
            {
                Letter_JumpTo letter = new Letter_JumpTo
                {
                    title = "CannotResurrectTitle".Translate(),
                    Text = "CannotResurrectMessage".Translate(pawn)
                };
                Find.LetterStack.ReceiveLetter(letter);
                return false;
            }
            return true;
        }
    }
}