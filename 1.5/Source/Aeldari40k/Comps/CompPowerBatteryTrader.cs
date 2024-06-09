using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace Aeldari40k
{
    public class CompPowerBatteryTrader : CompPowerBattery
    {
        private float storedEnergyTrader;

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref storedEnergyTrader, "storedEnergyTrader", 0f);
            CompProperties_Battery compProperties_Battery = Props;
            if (storedEnergyTrader > compProperties_Battery.storedEnergyMax)
            {
                storedEnergyTrader = compProperties_Battery.storedEnergyMax;
            }
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                AddEnergy(storedEnergyTrader);
            }
            
        }

        public override string CompInspectStringExtra()
        {
            CompProperties_Battery compProperties_Battery = Props;
            string text = "PowerBatteryStored".Translate() + ": " + StoredEnergy.ToString("F0") + " / " + compProperties_Battery.storedEnergyMax.ToString("F0") + " Wd";
            text += "\n" + "PowerBatteryEfficiency".Translate() + ": " + (compProperties_Battery.efficiency * 100f).ToString("F0") + "%";
            return text;
        }

    }
}