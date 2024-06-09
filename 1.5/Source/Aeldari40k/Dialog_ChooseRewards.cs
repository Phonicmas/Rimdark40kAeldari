using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;


namespace Aeldari40k
{
    [StaticConstructorOnStartup]
    public class Dialog_ChooseRewards : Window
    {
        private readonly List<Thing> choices;

        public Dialog_ChooseRewards()
        {}

        public Dialog_ChooseRewards(List<Thing> choices)
        {
            this.choices = choices;
        }

        public override Vector2 InitialSize => new Vector2(500f, 300f);

        public override void DoWindowContents(Rect inRect)
        {
            inRect = inRect.ContractedBy(15f, 7f);
            Widgets.Label(inRect.TopPartPixels(60f), "ChooseReward".Translate());
            inRect.y += 60f;
            List<Rect> rects = Split(inRect, choices.Count, new Vector2(80f, 200f));
            for (int i = 0; i < choices.Count; i++)
            {
                Thing thing = choices[i];
                Rect item3 = rects[i];
                Rect rect2 = new Rect(item3.x, item3.y, 80f, 80f);
                Rect rect3 = new Rect(item3.x + 5f, item3.y + 170f, 100f, 30f);
                GUI.DrawTexture(rect2, thing.def.uiIcon);
                TooltipHandler.TipRegion(rect2, new TipSignal($"{thing.def.LabelCap}\n\n{thing.def.description}\n\nAmount: x{thing.stackCount}"));
                if (Widgets.ButtonText(rect3, "SelectReward".Translate()))
                {
                    //Give reward
                    Close();
                    break;
                }
            }
        }

        private static List<Rect> Split(Rect rect, int parts, Vector2 size, bool vertical = false)
        {
            List<Rect> result = new List<Rect>();
            float distance = (vertical ? rect.height : rect.width) / (float)parts;
            Vector2 curLoc = new Vector2(rect.x, rect.y);
            Vector2 offset = (vertical ? new Vector2(0f, distance / 2f - size.y / 2f) : new Vector2(distance / 2f - size.x / 2f, 0f));
            for (float i = 0f; i < (vertical ? rect.height : rect.width); i += distance)
            {
                result.Add(new Rect(curLoc + offset, size));
                if (vertical)
                {
                    curLoc.y += distance;
                }
                else
                {
                    curLoc.x += distance;
                }
            }
            return result;
        }
    }
}