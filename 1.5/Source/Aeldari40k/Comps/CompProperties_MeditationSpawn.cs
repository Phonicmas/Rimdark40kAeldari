using RimWorld;
using System.Collections.Generic;
using Verse;


namespace Aeldari40k
{
    public class CompProperties_MeditationSpawn : CompProperties
    {
        public ThingDef spawnThing;

        public SoundDef spawnSound;

        public int spawnAmount;

        public float multAmount = 1;

        public StatDef statMult;
    }
}