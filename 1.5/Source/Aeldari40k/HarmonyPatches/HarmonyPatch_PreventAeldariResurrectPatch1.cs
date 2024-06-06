using Core40k;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Aeldari40k
{   
    [HarmonyPatch(typeof(ResurrectionUtility), "TryResurrect")]
    public class PreventAeldariRessurectPatch1
    {
        public static bool Prefix(Pawn pawn)
        {
            if (Prefs.DevMode && DebugSettings.godMode)
            {
                return true;
            }
            if (pawn.genes != null && pawn.genes.HasActiveGene(Aeldari40kDefOf.BEWH_AeldariCurseOfSlaanesh))
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