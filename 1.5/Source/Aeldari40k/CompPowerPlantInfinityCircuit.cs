using LudeonTK;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;


namespace Aeldari40k
{
    [StaticConstructorOnStartup]
    public class CompPowerPlantInfinityCircuit : CompPowerPlant
    {
        private float cachedPowerOutput;

        protected override float DesiredPowerOutput => cachedPowerOutput;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref cachedPowerOutput, "cachedPowerOutput", 0f);
        }

        public override void CompTick()
        {
            base.CompTick();
            if (!PowerOn)
            {
                cachedPowerOutput = 0f;
                return;
            }
            if (parent is InfinityCircuitBuilding b)
            {
                cachedPowerOutput = 0f - Props.PowerConsumption * b.SoulAmount;
                return;
            }
            else
            {
                cachedPowerOutput = 0f;
                return;
            }
        }

    }
}