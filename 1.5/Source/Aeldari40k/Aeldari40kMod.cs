using HarmonyLib;
using Verse;


namespace Aeldari40k
{
    public class Aeldari40kMod : Mod
    {
        public static Harmony harmony;

        public Aeldari40kMod(ModContentPack content) : base(content)
        {
            harmony = new Harmony("Aeldari40k.Mod");
            harmony.PatchAll();
        }
    }
}