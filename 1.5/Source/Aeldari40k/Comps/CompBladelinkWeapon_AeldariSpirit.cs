using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Aeldari40k
{
    public class CompBladelinkWeapon_AeldariSpirit : CompBladelinkWeapon
    {
        private int lastKillTick = -1;

        private List<WeaponTraitDef_AeldariSpirit> traits = new List<WeaponTraitDef_AeldariSpirit>();

        public new List<WeaponTraitDef_AeldariSpirit> TraitsListForReading => traits;

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
            if (spirit == null)
            {
                return;
            }
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
                    traits.Add(source.RandomElement());
                }
            }
            Rand.PopState();
        }

        public override string CompInspectStringExtra()
        {
            string text = "";
            if (!traits.NullOrEmpty())
            {
                text += "Stat_Thing_PersonaWeaponTrait_Label".Translate() + ": " + traits.Select((WeaponTraitDef_AeldariSpirit x) => x.label).ToCommaList().CapitalizeFirst();
            }

            if (Biocodable)
            {
                if (!text.NullOrEmpty())
                {
                    text += "\n";
                }
                text = ((base.CodedPawn != null) ? (text + "BondedWith".Translate(base.CodedPawnLabel.ApplyTag(TagType.Name)).Resolve()) : ((string)(text + "NotBonded".Translate())));
            }
            text += "\n";
            if (spirit != null)
            {
                text += "InhabitedBy".Translate(spirit.Label);
            }
            else
            {
                text += "EmptySpiritStone".Translate();
            }
            return text;
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {
            foreach (Faction allFaction in Find.FactionManager.AllFactions)
            {
                RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(parent.def, allFaction);
                if (minTitleToUse != null)
                {
                    yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "Stat_Thing_MinimumRoyalTitle_Name".Translate(allFaction.Named("FACTION")).Resolve(), minTitleToUse.GetLabelCapForBothGenders(), "Stat_Thing_Weapon_MinimumRoyalTitle_Desc".Translate(allFaction.Named("FACTION")).Resolve(), 2100);
                }
            }
            if (traits.NullOrEmpty())
            {
                yield break;
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Stat_Thing_PersonaWeaponTrait_Desc".Translate());
            stringBuilder.AppendLine();
            for (int i = 0; i < traits.Count; i++)
            {
                stringBuilder.AppendLine(traits[i].LabelCap + ": " + traits[i].description);
                if (i < traits.Count - 1)
                {
                    stringBuilder.AppendLine();
                }
            }
            yield return new StatDrawEntry(parent.def.IsMeleeWeapon ? StatCategoryDefOf.Weapon_Melee : StatCategoryDefOf.Weapon_Ranged, "Stat_Thing_PersonaWeaponTrait_Label".Translate(), traits.Select((WeaponTraitDef_AeldariSpirit x) => x.label).ToCommaList().CapitalizeFirst(), stringBuilder.ToString(), 1104);
        }

        private List<WeaponTraitDef_AeldariSpirit> ValidTraitsByStone()
        {
            IEnumerable<WeaponTraitDef_AeldariSpirit> allDefs = DefDatabase<WeaponTraitDef_AeldariSpirit>.AllDefs;
            List<WeaponTraitDef_AeldariSpirit> validTraits = new List<WeaponTraitDef_AeldariSpirit>();

            validTraits.AddRange(GetWeaponTraitsFromSkills(allDefs.Where(x => x.HasModExtension<DefModExtension_WeaponTraitExtra>() && x.GetModExtension<DefModExtension_WeaponTraitExtra>().isSkillRelated)));
            validTraits.AddRange(GetWeaponTraitsFromTraits(allDefs.Where(x => x.HasModExtension<DefModExtension_WeaponTraitExtra>() && x.GetModExtension<DefModExtension_WeaponTraitExtra>().isTraitRelated)));
            validTraits.AddRange(GetWeaponTraitsFromStats(allDefs.Where(x => x.HasModExtension<DefModExtension_WeaponTraitExtra>() && x.GetModExtension<DefModExtension_WeaponTraitExtra>().isStatRelated)));


            return validTraits;
        }

        private List<WeaponTraitDef_AeldariSpirit> GetWeaponTraitsFromTraits(IEnumerable<WeaponTraitDef_AeldariSpirit> weaponTraitDefs)
        {
            List<WeaponTraitDef_AeldariSpirit> weaponTraits = new List<WeaponTraitDef_AeldariSpirit>();

            foreach (Trait trait in spirit.story.traits.allTraits)
            {
                weaponTraits.AddRange(weaponTraitDefs.Where(x => x.GetModExtension<DefModExtension_WeaponTraitExtra>().traitDef == trait.def && x.GetModExtension<DefModExtension_WeaponTraitExtra>().degree == trait.Degree));
            }

            return weaponTraits;
        }

        private List<WeaponTraitDef_AeldariSpirit> GetWeaponTraitsFromStats(IEnumerable<WeaponTraitDef_AeldariSpirit> weaponTraitDefs)
        {
            List<WeaponTraitDef_AeldariSpirit> weaponTraits = new List<WeaponTraitDef_AeldariSpirit>();


            foreach (WeaponTraitDef_AeldariSpirit weaponTraitDef in weaponTraitDefs)
            {
                StatDef statDef = weaponTraitDef.GetModExtension<DefModExtension_WeaponTraitExtra>().statDef;
                if (spirit.GetStatValue(statDef) >= weaponTraitDef.GetModExtension<DefModExtension_WeaponTraitExtra>().statThreshold)
                {
                    weaponTraits.Add(weaponTraitDef);
                }
            }

            return weaponTraits;
        }

        private List<WeaponTraitDef_AeldariSpirit> GetWeaponTraitsFromSkills(IEnumerable<WeaponTraitDef_AeldariSpirit> weaponTraitDefs)
        {
            List<SkillRecord> highestSkill = null;
            foreach (SkillRecord skillRecord in spirit.skills.skills)
            {
                if (highestSkill.NullOrEmpty() || skillRecord.Level > highestSkill.First().Level)
                {
                    highestSkill = new List<SkillRecord>
                    {
                        skillRecord
                    };
                    continue;
                }
                else if (skillRecord.Level == highestSkill.First().Level)
                {
                    highestSkill.Add(skillRecord);
                }
            }

            List<WeaponTraitDef_AeldariSpirit> weaponTraits = new List<WeaponTraitDef_AeldariSpirit>();
            foreach (SkillRecord skillRecord in highestSkill)
            {
                weaponTraits.AddRange(weaponTraitDefs.Where(x => x.GetModExtension<DefModExtension_WeaponTraitExtra>().skillDef == skillRecord.def));
            }

            return weaponTraits;
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
            Scribe_Values.Look(ref biocoded, "biocoded", defaultValue: false);
            Scribe_Values.Look(ref codedPawnLabel, "biocodedPawnLabel");
            if (Scribe.mode == LoadSaveMode.Saving && codedPawn != null && codedPawn.Discarded)
            {
                codedPawn = null;
            }
            Scribe_References.Look(ref codedPawn, "codedPawn", saveDestroyedThings: true);
            Scribe_Values.Look(ref lastKillTick, "lastKillTick", -1);
            Scribe_Collections.Look(ref traits, "traits", LookMode.Def); //Not working properly
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