using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Noise;


namespace Aeldari40k
{
    public class Building_CraftworldWebwayGate : Building
    {

        private int cooldownTimeTotal = 90000/*0*/;
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
            if (cooldownTimeCharged >= cooldownTimeTotal) //Also check if there is enough power in the power grid it is attached to and then drain it
            {
                CompPowerBattery cpb = this.TryGetComp<CompPowerBattery>();
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
                        //Maybe do job for pawn to go to portal and do some work
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
            Log.Message("Rewards");
            return;
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            //stringBuilder.Append(base.GetInspectString());

            CompPowerBattery cpb = this.TryGetComp<CompPowerBattery>();
            CompPowerTrader cpt = this.TryGetComp<CompPowerTrader>();

            string text = "";

            if (cpt != null)
            {
                if (cpt.PowerNet == null)
                {
                    text += "PowerNotConnected".Translate();
                }
                else
                {
                    string tempText = (cpt.PowerNet.CurrentEnergyGainRate() / 1.66666669E-05f).ToString("F0");
                    string tempText2 = cpt.PowerNet.CurrentStoredEnergy().ToString("F0");
                    text += "PowerConnectedRateStored".Translate(tempText, tempText2);
                }
                text += "\n";
            }

            if (cpb != null)
            {
                text += "PowerBatteryStored".Translate() + ": " + cpb.StoredEnergy.ToString("F0") + " / " + cpb.Props.storedEnergyMax.ToString("F0") + " Wd";
                text += "\n" + "PowerBatteryEfficiency".Translate() + ": " + (cpb.Props.efficiency * 100f).ToString("F0") + "%";
                if (cpb.StoredEnergy > 0f)
                {
                    text += "\n" + "SelfDischarging".Translate() + ": " + 5f.ToString("F0") + " W";
                }
                text += "\n";
            }

            if (!cpt.PowerOn)
            {
                text += "NoPowerToRecharge".Translate();
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
                text += "ReadyIn".Translate(Math.Round(CooldownTimeRemaining / divider, 2), timeDenoter);
            }

            return text;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref cooldownTimeCharged, "cooldownTimeCharged", 0);
        }
    }
}