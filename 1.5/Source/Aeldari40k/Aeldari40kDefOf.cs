using RimWorld;
using Verse;

namespace Aeldari40k
{
    [DefOf]
    public static class Aeldari40kDefOf
    {

        public static GeneDef BEWH_AeldariPsyker;
        public static GeneDef BEWH_AeldariCurseOfSlaanesh;


        public static ThoughtDef BEWH_InfinityCircuitThought;
        public static ThoughtDef BEWH_SoulStoneDeathThought;
        public static ThoughtDef BEWH_NoSoulStoneDeathThought;


        public static ThingDef BEWH_InfinityCircuit;
        public static ThingDef BEWH_SpiritStone;
        public static ThingDef BEWH_Wraithbone;
        public static ThingDef BEWH_WraithboneBenchMeditation;


        public static WebwayRewardDef BEWH_WebwayRewardWraithstone;


        public static RulePackDef BEWH_NamerAeldariFull;


        public static PawnKindDef BEWH_WeaponSpiritAeldariPawn;


        public static NeedDef BEWH_MeditationDependency;


        public static MeditationFocusDef BEWH_Bonesinger;


        public static WeaponTraitDef_AeldariSpirit BEWH_SpiritTraitJoyous;
        public static WeaponTraitDef_AeldariSpirit BEWH_SpiritTraitSanguine;
        public static WeaponTraitDef_AeldariSpirit BEWH_SpiritTraitOptimist;
        public static WeaponTraitDef_AeldariSpirit BEWH_SpiritTraitGreatMemory;

        static Aeldari40kDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(Aeldari40kDefOf));
        }
    }
}