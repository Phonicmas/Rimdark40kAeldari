using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace Aeldari40k
{
    public class CompMeditationSpawn : ThingComp
    {
        private float progressToNextSpawn;

        private List<Thing> spawnedThings = new List<Thing>();

        private int meditationTicksToday;

        private Pawn meditator;

        private static readonly List<Pair<int, float>> TicksToProgressMultipliers = new List<Pair<int, float>>
    {
        new Pair<int, float>(30000, 1f),
        new Pair<int, float>(60000, 0.5f),
        new Pair<int, float>(120000, 0.25f),
        new Pair<int, float>(240000, 0.15f)
    };

        public CompProperties_MeditationSpawn Props => (CompProperties_MeditationSpawn)props;

        public List<Thing> WraithbonesForReading
        {
            get
            {
                Cleanup();
                return spawnedThings;
            }
        }

        private float ProgressMultiplier
        {
            get
            {
                foreach (Pair<int, float> ticksToProgressMultiplier in TicksToProgressMultipliers)
                {
                    if (meditationTicksToday < ticksToProgressMultiplier.First)
                    {
                        return ticksToProgressMultiplier.Second;
                    }
                }
                return TicksToProgressMultipliers.Last().Second;
            }
        }

        public void AddProgress(Pawn pawn, float progress = 6.666667E-05f, bool ignoreMultiplier = false)
        {
            meditator = pawn;
            if (!ignoreMultiplier)
            {
                progress *= ProgressMultiplier;
            }
            progressToNextSpawn += progress * (1f + parent.GetStatValue(StatDefOf.MeditationPlantGrowthOffset));
            meditationTicksToday++;
            TryGrowSubplants();
        }

        public override void CompTickLong()
        {
            if (GenLocalDate.DayTick(parent.Map) < 2000)
            {
                meditationTicksToday = 0;
            }
        }

        public void Cleanup()
        {
            spawnedThings.RemoveAll((Thing p) => !p.Spawned);
        }

        public override string CompInspectStringExtra()
        {
            string text = "TotalMeditationToday".Translate((meditationTicksToday / 2500).ToString() + "LetterHour".Translate(), ProgressMultiplier.ToStringPercent());
            text += " (" + "ProgressToNextSubplant".Translate(progressToNextSpawn.ToStringPercent()) + ")";
            return text;
        }

        private void TryGrowSubplants()
        {
            while (progressToNextSpawn >= 1f)
            {
                DoGrowWraithbone();
                progressToNextSpawn -= 1f;
            }
        }

        private void DoGrowWraithbone()
        {
            IntVec3 position = parent.Position;
            for (int i = 0; i < 1000; i++)
            {
                IntVec3 intVec = position + GenRadial.RadialPattern[i];
                if (!intVec.InBounds(parent.Map) || !WanderUtility.InSameRoom(position, intVec, parent.Map))
                {
                    continue;
                }
                bool flag = false;
                List<Thing> thingList = intVec.GetThingList(parent.Map);
                foreach (Thing item in thingList)
                {
                    if (item.def == Props.spawnThing)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    continue;
                }
                //Check here if spawn location is valid - that is there are nothing blocking it
                /*if (false) 
                {
                    continue;
                }*/
                for (int num = thingList.Count - 1; num >= 0; num--)
                {
                    if (thingList[num].def.category == ThingCategory.Plant)
                    {
                        thingList[num].Destroy();
                    }
                }
                Thing wraithbone = GenSpawn.Spawn(Props.spawnThing, intVec, parent.Map);

                float spawnAmount = Props.spawnAmount;

                if (Props.statMult != null && meditator != null)
                {
                    spawnAmount *= (Props.multAmount * meditator.GetStatValue(Props.statMult));
                }


                wraithbone.stackCount = (int)spawnAmount;
                if (Props.spawnSound != null)
                {
                    Props.spawnSound.PlayOneShot(new TargetInfo(parent));
                }
                break;
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (Prefs.DevMode && DebugSettings.godMode)
            {
                Command_Action command_Action = new Command_Action();
                command_Action.defaultLabel = "DEV: Add 100% progress";
                command_Action.action = delegate
                {
                    AddProgress(meditator, 1f, ignoreMultiplier: true);
                };
                yield return command_Action;
            }
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref progressToNextSpawn, "progressToNextSpawn", 0f);
            Scribe_Collections.Look(ref spawnedThings, "spawnedThings", LookMode.Reference);
            Scribe_Values.Look(ref meditationTicksToday, "meditationTicksToday", 0);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                spawnedThings.RemoveAll((Thing x) => x == null);
            }
        }
    
    
    }
}