using RimWorld;
using System.Collections.Generic;
using Verse;


namespace Aeldari40k
{
    public class WeaponTraitDef_AeldariSpirit : WeaponTraitDef
    {

        public List<SkillGain> skillGains = new List<SkillGain>();

        public override IEnumerable<string> ConfigErrors()
        {
            if (!typeof(WeaponTraitWorker).IsAssignableFrom(workerClass))
            {
                yield return $"WeaponTraitDef {defName} has worker class {workerClass}, which is not deriving from {typeof(WeaponTraitWorker).FullName}";
            }
        }

        public bool Overlaps(WeaponTraitDef_AeldariSpirit other)
        {
            if (other == this)
            {
                return true;
            }

            if (exclusionTags.NullOrEmpty() || other.exclusionTags.NullOrEmpty())
            {
                return false;
            }

            return exclusionTags.Any((string x) => other.exclusionTags.Contains(x));
        }

    }
}