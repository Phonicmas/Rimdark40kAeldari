﻿using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;


namespace Aeldari40k
{
    public class ITab_ContentsSpiritStone : ITab_ContentsBase
    {
        public override IList<Thing> container => InfinityCircuit.GetDirectlyHeldThings();

        public override bool IsVisible
        {
            get
            {
                if (base.SelThing != null)
                {
                    return base.IsVisible;
                }
                return false;
            }
        }

        public InfinityCircuitBuilding InfinityCircuit => base.SelThing as InfinityCircuitBuilding;

        public override bool VisibleInBlueprintMode => false;

        public ITab_ContentsSpiritStone()
        {
            labelKey = "TabCasketContents";
            containedItemsKey = "TabCasketContents";
        }
        
    }
}