using RimWorld;
using Verse;

namespace Aeldari40k
{
    public class DefModExtension_WeaponTraitExtra : DefModExtension
    {
        public bool isSkillRelated = false;
        public SkillDef skillDef = null;

        public bool isTraitRelated = false;
        public TraitDef traitDef = null;

        public bool isStatRelated = false;
        public StatDef statDef = null;
        public float statThreshold = 0;
    }
}