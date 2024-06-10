using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;


namespace Aeldari40k
{
    [StaticConstructorOnStartup]
    public class Building_CraftworldWebwayGate : Building
    {

        readonly private int cooldownTimeTotal = 900000;

        private int cooldownTimeCharged = 0;

        private float CooldownTimeRemaining
        {
            get { return cooldownTimeTotal - cooldownTimeCharged; }
        }

        private static readonly Texture2D UsePortalIcon = ContentFinder<Texture2D>.Get("UI/Genes/AeldariPsyker_icon");


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
                CompPowerBatteryTrader cpb = this.TryGetComp<CompPowerBatteryTrader>();
                CompPowerTrader cpt = this.TryGetComp<CompPowerTrader>();
                DefModExtension_WebwayGate dfwg = def.GetModExtension<DefModExtension_WebwayGate>();

                if (cpb != null && dfwg != null && cpb.StoredEnergy >= dfwg.powerToActivate && cpt.PowerOn)
                {
                    Command_Action command_Action1 = new Command_Action();
                    command_Action1.defaultLabel = "CommandUsePortal".Translate();
                    command_Action1.defaultDesc = "CommandUsePortalDesc".Translate();
                    command_Action1.icon = UsePortalIcon;
                    command_Action1.activateSound = SoundDefOf.Designate_Cancel;
                    command_Action1.action = delegate
                    {
                        //Do job for pawn to go to portal and then disappear for some time
                        GiveRewardOptions();
                        cpb.DrawPower(dfwg.powerToActivate);
                        cooldownTimeCharged = 0;
                    };
                    yield return command_Action1;
                    yield break;
                }                
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (this.TryGetComp<CompPowerTrader>().PowerOn && cooldownTimeCharged < cooldownTimeTotal)
            {
                cooldownTimeCharged++;
            }
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

            CompPowerTrader cpt = this.TryGetComp<CompPowerTrader>();

            if (!cpt.PowerOn)
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

            return stringBuilder.ToString();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref cooldownTimeCharged, "cooldownTimeCharged", 0);
        }
    }
}