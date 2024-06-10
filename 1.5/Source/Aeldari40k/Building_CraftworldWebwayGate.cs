using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Aeldari40k
{
    [StaticConstructorOnStartup]
    public class Building_CraftworldWebwayGate : Building_Enterable
    {

        private readonly int cooldownTimeTotal = 900000;

        private int cooldownTimeCharged = 0;

        private readonly int ticksTimeTotal = 30000;

        private int ticksRemaining = 0;

        private float CooldownTimeRemaining
        {
            get { return cooldownTimeTotal - cooldownTimeCharged; }
        }

        private static readonly Texture2D UsePortalIcon = ContentFinder<Texture2D>.Get("UI/Genes/AeldariPsyker_icon");

        [Unsaved(false)]
        private CompPowerTrader cachedPowerComp;

        private CompPowerTrader PowerTraderComp
        {
            get
            {
                if (cachedPowerComp == null)
                {
                    cachedPowerComp = this.TryGetComp<CompPowerTrader>();
                }
                return cachedPowerComp;
            }
        }


        [Unsaved(false)]
        private CompPowerBatteryTrader cachedBatteryComp;

        private CompPowerBatteryTrader PowerBatteryComp
        {
            get
            {
                if (cachedBatteryComp == null)
                {
                    cachedBatteryComp = this.TryGetComp<CompPowerBatteryTrader>();
                }
                return cachedBatteryComp;
            }
        }


        [Unsaved(false)]
        private DefModExtension_WebwayGate cachedWebwayGateDefMod;

        private DefModExtension_WebwayGate WebwayGateDefMod
        {
            get
            {
                if (cachedWebwayGateDefMod == null)
                {
                    cachedWebwayGateDefMod = def.GetModExtension<DefModExtension_WebwayGate>();
                }
                return cachedWebwayGateDefMod;
            }
        }


        [Unsaved(false)]
        private Effecter progressBar;

        private Pawn ContainedPawn => innerContainer.FirstOrDefault() as Pawn;


        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            if (progressBar != null)
            {
                progressBar.Cleanup();
                progressBar = null;
            }
            base.DeSpawn(mode);
        }


        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
            {
                yield return floatMenuOption;
            }
            if (!selPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly))
            {
                yield return new FloatMenuOption("CannotEnterBuilding".Translate(this) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
                yield break;
            }
            AcceptanceReport acceptanceReport = CanAcceptPawn(selPawn);
            if (acceptanceReport.Accepted)
            {
                yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("EnterBuilding".Translate(this), delegate
                {
                    SelectPawn(selPawn);
                }), selPawn, this);
            }
            else if (base.SelectedPawn == selPawn && !selPawn.IsPrisonerOfColony)
            {
                yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("EnterBuilding".Translate(this), delegate
                {
                    selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.EnterBuilding, this), JobTag.Misc);
                }), selPawn, this);
            }
            else if (!acceptanceReport.Reason.NullOrEmpty())
            {
                yield return new FloatMenuOption("CannotEnterBuilding".Translate(this) + ": " + acceptanceReport.Reason.CapitalizeFirst(), null);
            }
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }
            if (DebugSettings.ShowDevGizmos)
            {
                Command_Action command_Action2 = new Command_Action();
                command_Action2.defaultLabel = "DEV: Finish Cooldown";
                command_Action2.action = delegate
                {
                    cooldownTimeCharged = cooldownTimeTotal;
                };
                yield return command_Action2;
            }
            if (cooldownTimeCharged >= cooldownTimeTotal)
            {
                if (PowerBatteryComp != null && WebwayGateDefMod != null && PowerBatteryComp.StoredEnergy >= WebwayGateDefMod.powerToActivate && PowerTraderComp.PowerOn)
                {
                    Command_Action command_Action1 = new Command_Action();
                    command_Action1.defaultLabel = "CommandUsePortal".Translate();
                    command_Action1.defaultDesc = "CommandUsePortalDesc".Translate();
                    command_Action1.icon = UsePortalIcon;
                    command_Action1.activateSound = SoundDefOf.Designate_Cancel;
                    command_Action1.action = delegate
                    {
                        List<FloatMenuOption> list = new List<FloatMenuOption>();
                        foreach (Pawn item in Map.mapPawns.AllPawnsSpawned)
                        {
                            Pawn pawn = item;
                            if (pawn.genes != null)
                            {
                                AcceptanceReport acceptanceReport = CanAcceptPawn(pawn);
                                string text = pawn.LabelShortCap;
                                if (!acceptanceReport.Accepted)
                                {
                                    if (!acceptanceReport.Reason.NullOrEmpty())
                                    {
                                        list.Add(new FloatMenuOption(text + ": " + acceptanceReport.Reason, null, pawn, Color.white));
                                    }
                                }
                                else
                                {
                                    list.Add(new FloatMenuOption(text, delegate
                                    {
                                        SelectPawn(pawn);
                                    }, pawn, Color.white));
                                }
                            }
                        }
                        if (!list.Any())
                        {
                            list.Add(new FloatMenuOption("NoValidPawns".Translate(), null));
                        }
                        Find.WindowStack.Add(new FloatMenu(list));
                    };
                    if (!PowerTraderComp.PowerOn)
                    {
                        command_Action1.Disable("NoPower".Translate().CapitalizeFirst());
                    }
                    yield return command_Action1;
                    yield break;
                }                
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (base.Working)
            {
                if (ContainedPawn == null)
                {
                    Cancel();
                    return;
                }
                if (PowerTraderComp.PowerOn)
                {
                    TickEffects();
                    ticksRemaining--;
                    if (ticksRemaining <= 0)
                    {
                        Finish();
                    }
                    
                }
                return;
            }
            else
            {
                if (selectedPawn != null && selectedPawn.Dead)
                {
                    Cancel();
                }
                if (progressBar != null)
                {
                    progressBar.Cleanup();
                    progressBar = null;
                }
            }

            if (this.TryGetComp<CompPowerTrader>().PowerOn && cooldownTimeCharged < cooldownTimeTotal)
            {
                cooldownTimeCharged++;
            }
        }

        private void Finish()
        {
            startTick = -1;
            selectedPawn = null;
            IntVec3 intVec = (def.hasInteractionCell ? InteractionCell : base.Position);
            innerContainer.TryDropAll(intVec, base.Map, ThingPlaceMode.Near);
            GiveRewardOptions();
        }

        private void Cancel()
        {
            startTick = -1;
            selectedPawn = null;
            innerContainer.TryDropAll(def.hasInteractionCell ? InteractionCell : base.Position, base.Map, ThingPlaceMode.Near);
        }

        private void TickEffects()
        {
            if (progressBar == null)
            {
                progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
            }
            progressBar.EffectTick(new TargetInfo(InteractionCell, base.Map), TargetInfo.Invalid);
            MoteProgressBar mote = ((SubEffecter_ProgressBar)progressBar.children[0]).mote;
            if (mote != null)
            {
                mote.progress = 1f - Mathf.Clamp01((float)ticksRemaining / 30000f);
                mote.offsetZ = ((base.Rotation == Rot4.North) ? 0.5f : (-0.5f));
            }
        }


        public override Vector3 PawnDrawOffset { get { return Vector2.zero; } }

        public override AcceptanceReport CanAcceptPawn(Pawn p)
        {
            if (!p.IsColonist && !p.IsSlaveOfColony && !p.IsPrisonerOfColony && (!p.IsColonyMutant || !p.IsGhoul))
            {
                return false;
            }
            if (selectedPawn != null && selectedPawn != p)
            {
                return false;
            }
            if (!p.RaceProps.Humanlike || p.IsQuestLodger())
            {
                return false;
            }
            if (!PowerTraderComp.PowerOn)   
            {
                return "NoPower".Translate().CapitalizeFirst();
            }
            if (innerContainer.Count > 0)
            {
                return "Occupied".Translate();
            }
            if (p.genes == null || !p.genes.GenesListForReading.Any((Gene x) => x.def.passOnDirectly))
            {
                return "PawnHasNoGenes".Translate(p.Named("PAWN"));
            }
            if (!p.genes.HasActiveGene(Aeldari40kDefOf.BEWH_AeldariPsyker))
            {
                return "PawnNotAeldariPsyker".Translate();
            }
            return true;

        }

        protected override void SelectPawn(Pawn pawn)
        {
            selectedPawn = pawn;
            if (!pawn.IsPrisonerOfColony && !pawn.Downed)
            {
                pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.EnterBuilding, this), JobTag.Misc);
            }
        }

        public override void TryAcceptPawn(Pawn p)
        {
            if ((bool)CanAcceptPawn(p))
            {
                selectedPawn = p;
                bool num = p.DeSpawnOrDeselect();
                if (innerContainer.TryAddOrTransfer(p))
                {
                    startTick = Find.TickManager.TicksGame;
                    ticksRemaining = ticksTimeTotal;
                    cooldownTimeCharged = 0;
                    PowerBatteryComp.DrawPower(WebwayGateDefMod.powerToActivate);
                }
                if (num)
                {
                    Find.Selector.Select(p, playSound: false, forceDesignatorDeselect: false);
                }
            }
        }

        public override void DrawExtraSelectionOverlays()
        {
            return;
        }



        private void GiveRewardOptions()
        {
            int rewardCount = 3;

            System.Random rand = new System.Random();

            List<WebwayRewardDef> possibleChoices = new List<WebwayRewardDef>();
            possibleChoices.AddRange((List<WebwayRewardDef>)DefDatabase<WebwayRewardDef>.AllDefs);
            List<Thing> choices = new List<Thing>();

            for (int i = 0; i < rewardCount; i++)
            {
                int chosen = rand.Next(0, possibleChoices.Count);
                WebwayRewardDef reward = possibleChoices[chosen];

                if (reward.rewardPawn != null)
                {
                    Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(reward.rewardPawn, Faction.OfPlayer));
                    choices.Add(pawn);
                }
                else
                {
                    Thing thing = ThingMaker.MakeThing(reward.rewardThing);
                    thing.stackCount = reward.rewardThingCount;
                    if (reward.giveQuality)
                    {
                        List<QualityCategory> possibleQuality = new List<QualityCategory>();

                        foreach (QualityCategory quality in Enum.GetValues(typeof(QualityCategory)))
                        {
                            if (quality >= reward.rewardCategoryMinimum)
                            {
                                possibleQuality.Add(quality);
                            }
                        }

                        thing.TryGetComp<CompQuality>()?.SetQuality(possibleQuality.RandomElement(), null);
                    }
                    choices.Add(thing);
                }
                possibleChoices.RemoveAt(chosen);
            }

            Find.WindowStack.Add(new Dialog_ChooseRewards(choices, this, Map));
            return;
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.GetInspectString());
            stringBuilder.Append("\n");

            if (!PowerTraderComp.PowerOn)
            {
                stringBuilder.Append("NoPowerToRecharge".Translate());
            }
            else if (CooldownTimeRemaining <= 0)
            {
                stringBuilder.Append("ReadyNow".Translate());
            }
            else
            {
                float divider = 60000f;
                string timeDenoter = "LetterDay".Translate();

                if (CooldownTimeRemaining < 60000)
                {
                    divider = 2500f;
                    timeDenoter = "LetterHour".Translate();
                }
                stringBuilder.Append("ReadyIn".Translate(Math.Round(CooldownTimeRemaining / divider, 2), timeDenoter));
            }

            if (Working)
            {
                stringBuilder.Append("\n");
                stringBuilder.Append("TravelTimeRemaining".Translate(ContainedPawn, Math.Round(ticksRemaining / 2500f, 2) + "LetterHour".Translate()));
            }

            return stringBuilder.ToString();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref cooldownTimeCharged, "cooldownTimeCharged", 0);
            Scribe_Values.Look(ref ticksRemaining, "ticksRemaining", 0);
        }
    }
}