using RimWorld;
using Verse;

namespace Aeldari40k
{
    [DefOf]
    public static class Aeldari40kDefOf
    {

        public static GeneDef BEWH_AeldariPsyker;

        public static ThoughtDef BEWH_InfinityCircuitThought;

        static Aeldari40kDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(Aeldari40kDefOf));
        }
    }
}