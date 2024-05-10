using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Aeldari40k
{
    public class CompBladelinkWeapon_AeldariSpirit : CompBladelinkWeapon
    {
        private int lastKillTick = -1;

        private List<WeaponTraitDef_AeldariSpirit> traits = new List<WeaponTraitDef_AeldariSpirit>();

        private static readonly IntRange TraitsRange = new IntRange(1, 2);

        private bool oldBonded;

        private string oldBondedPawnLabel;

        private Pawn oldBondedPawn;

        public Pawn spirit = null;

        public override void PostPostMake()
        {
            return;
        }

        public void InitializeTraits()
        {
            if (traits == null)
            {
                traits = new List<WeaponTraitDef_AeldariSpirit>();
            }
            Rand.PushState(parent.HashOffset());
            int randomInRange = TraitsRange.RandomInRange;
            for (int i = 0; i < randomInRange; i++)
            {
                IEnumerable<WeaponTraitDef_AeldariSpirit> source = ValidTraitsByStone().Where((WeaponTraitDef_AeldariSpirit x) => CanAddTrait(x));
                if (source.Any())
                {
                    traits.Add(source.RandomElementByWeight((WeaponTraitDef_AeldariSpirit x) => x.commonality));
                }
            }
            Rand.PopState();
        }

        public override string CompInspectStringExtra()
        {
            string text = base.CompInspectStringExtra();
            text += "\n";
            text += "InhabitedBy".Translate(spirit.Label);
            return text;
        }


        private List<WeaponTraitDef_AeldariSpirit> ValidTraitsByStone()
        {
            List<WeaponTraitDef_AeldariSpirit> validTraits = new List<WeaponTraitDef_AeldariSpirit>();
            Log.Message("Stone comp: " + spirit);
            return validTraits;
        }

        private bool CanAddTrait(WeaponTraitDef_AeldariSpirit trait)
        {
            if (!traits.NullOrEmpty())
            {
                for (int i = 0; i < traits.Count; i++)
                {
                    if (trait.Overlaps(traits[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref lastKillTick, "lastKillTick", -1);
            Scribe_Collections.Look(ref traits, "traits", LookMode.Def);
            Scribe_References.Look(ref spirit, "spirit", saveDestroyedThings: true);
            if (Scribe.mode != LoadSaveMode.Saving)
            {
                Scribe_Values.Look(ref oldBonded, "bonded", defaultValue: false);
                Scribe_Values.Look(ref oldBondedPawnLabel, "bondedPawnLabel");
                Scribe_References.Look(ref oldBondedPawn, "bondedPawn", saveDestroyedThings: true);
            }
            if (Scribe.mode != LoadSaveMode.PostLoadInit)
            {
                return;
            }
            if (oldBonded)
            {
                CodeFor(oldBondedPawn);
            }
            if (traits == null)
            {
                traits = new List<WeaponTraitDef_AeldariSpirit>();
            }
            if (oldBondedPawn != null)
            {
                if (string.IsNullOrEmpty(oldBondedPawnLabel) || !oldBonded)
                {
                    codedPawnLabel = oldBondedPawn.Name.ToStringFull;
                    biocoded = true;
                }
                if (oldBondedPawn.equipment.bondedWeapon == null)
                {
                    oldBondedPawn.equipment.bondedWeapon = parent;
                }
                else if (oldBondedPawn.equipment.bondedWeapon != parent)
                {
                    UnCode();
                }
            }
        }
    }
}