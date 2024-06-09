using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Noise;


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

        private static readonly Texture2D UsePortalIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel");


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
                DefModExtension_WebwayGate dfwg = def.GetModExtension<DefModExtension_WebwayGate>();

                if (cpb != null && dfwg != null && cpb.StoredEnergy >= dfwg.powerToActivate)
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
            List<Thing> choices = new List<Thing>();

            Thing wraithbone = ThingMaker.MakeThing(Aeldari40kDefOf.BEWH_Wraithbone);
            wraithbone.stackCount = 200;
            choices.Add(wraithbone);

            Thing wraithbone2 = ThingMaker.MakeThing(Aeldari40kDefOf.BEWH_Wraithbone);
            wraithbone2.stackCount = 300;
            choices.Add(wraithbone2);

            Thing wraithbone3 = ThingMaker.MakeThing(Aeldari40kDefOf.BEWH_Wraithbone);
            wraithbone3.stackCount = 400;
            choices.Add(wraithbone3);


            Find.WindowStack.Add(new Dialog_ChooseRewards(choices));
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