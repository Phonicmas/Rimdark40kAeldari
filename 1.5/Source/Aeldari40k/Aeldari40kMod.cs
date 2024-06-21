using HarmonyLib;
using UnityEngine;
using Verse;


namespace Aeldari40k
{
    public class Aeldari40kMod : Mod
    {
        public static Harmony harmony;
        readonly Aeldari40kModSettings settings;


        public Aeldari40kMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<Aeldari40kModSettings>();
            harmony = new Harmony("Aeldari40k.Mod");
            harmony.PatchAll();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("BEWH.SettingWebwayRewardCount".Translate(settings.aeldariWebwayGateRewardCount));
            settings.aeldariWebwayGateRewardCount = (int)listingStandard.Slider(settings.aeldariWebwayGateRewardCount, 1, 5);

            listingStandard.Label("BEWH.SettingWebwayTravelTime".Translate((settings.aeldariWebwayGateTravelTimeTicks / 2500).ToString("0.00")));
            settings.aeldariWebwayGateTravelTimeTicks = listingStandard.Slider(settings.aeldariWebwayGateTravelTimeTicks, 1250f, 120000);
            listingStandard.End();

            Rect resetRect = new Rect(inRect.BottomPartPixels(30f));
            float offset = resetRect.width / 3;
            resetRect.xMin += offset;
            resetRect.xMax -= offset;

            if (Widgets.ButtonText(resetRect, "BEWH.ResetToDefault".Translate()))
            {
                settings.aeldariWebwayGateTravelTimeTicks = 30000;
                settings.aeldariWebwayGateRewardCount = 3;
            }
            

            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "BEWH.ModSettingsNameAeldari".Translate();
        }
    }
}