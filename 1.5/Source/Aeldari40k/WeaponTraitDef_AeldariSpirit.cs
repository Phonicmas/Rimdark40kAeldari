using RimWorld;
using System.Collections.Generic;
using Verse;


namespace Aeldari40k
{
    public class WeaponTraitDef_AeldariSpirit : WeaponTraitDef
    {
        public override IEnumerable<string> ConfigErrors()
        {
            if (!typeof(WeaponTraitWorker).IsAssignableFrom(workerClass))
            {
                yield return $"WeaponTraitDef {defName} has worker class {workerClass}, which is not deriving from {typeof(WeaponTraitWorker).FullName}";
            }
        }
    }
}