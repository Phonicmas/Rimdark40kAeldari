using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Aeldari40k
{
    [StaticConstructorOnStartup]
    public class InfinityCircuitBuilding : Building, IThingHolderEvents<Thing>, IHaulEnroute, ILoadReferenceable, IStorageGroupMember, IHaulDestination, IStoreSettingsParent, IHaulSource, IThingHolder, ISearchableContents
    {
        public int MaximumSpiritStones => def.building.maxItemsInCell * def.size.Area;

        public IEnumerable<Thing> SoulAmount => innerContainer.InnerListForReading.Where(x => x.TryGetComp<SpiritStoneComp>()?.pawn != null);


        private ThingOwner<Thing> innerContainer;

        private StorageSettings settings;

        private StorageGroup storageGroup;

        public bool StorageTabVisible => true;

        public ThingOwner SearchableContents => innerContainer;

        StorageSettings IStorageGroupMember.StoreSettings => GetStoreSettings();

        StorageSettings IStorageGroupMember.ParentStoreSettings => GetParentStoreSettings();

        StorageSettings IStorageGroupMember.ThingStoreSettings => settings;

        StorageGroup IStorageGroupMember.Group
        {
            get
            {
                return storageGroup;
            }
            set
            {
                storageGroup = value;
            }
        }

        bool IStorageGroupMember.DrawStorageTab => true;

        bool IStorageGroupMember.ShowRenameButton => base.Faction == Faction.OfPlayer;

        bool IStorageGroupMember.DrawConnectionOverlay => base.Spawned;

        string IStorageGroupMember.StorageGroupTag => def.building.storageGroupTag;



        public InfinityCircuitBuilding()
        {
            innerContainer = new ThingOwner<Thing>(this, oneStackOnly: false);
        }


        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (storageGroup != null && map != storageGroup.Map)
            {
                StorageSettings storeSettings = storageGroup.GetStoreSettings();
                storageGroup.RemoveMember(this);
                storageGroup = null;
                settings.CopyFrom(storeSettings);
            }
        }

        public override void PostMake()
        {
            base.PostMake();
            settings = new StorageSettings(this);
            if (def.building.defaultStorageSettings != null)
            {
                settings.CopyFrom(def.building.defaultStorageSettings);
            }
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            if (storageGroup != null)
            {
                storageGroup?.RemoveMember(this);
                storageGroup = null;
            }
            innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near);
            base.DeSpawn(mode);
        }


        public override string GetInspectString()
        {
            string s = base.GetInspectString();
            s += "\n";
            s += "ContainsXSouls".Translate(SoulAmount.Count());
            return s;
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return innerContainer;
        }

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
        }

        public void Notify_SettingsChanged()
        {
            if (base.Spawned)
            {
                base.MapHeld.listerHaulables.Notify_HaulSourceChanged(this);
            }
        }

        public void Notify_ItemAdded(Thing newItem)
        {
            return;
        }

        public void Notify_ItemRemoved(Thing newItem)
        {
            return;
        }

        public bool Accepts(Thing t)
        {
            if (GetStoreSettings().AllowedToAccept(t) && t.TryGetComp<SpiritStoneComp>()?.pawn != null)
            {
                return innerContainer.CanAcceptAnyOf(t);
            }
            return false;
        }

        public int SpaceRemainingFor(ThingDef _)
        {
            return MaximumSpiritStones - SoulAmount.Count();
        }

        public StorageSettings GetStoreSettings()
        {
            if (storageGroup != null)
            {
                return storageGroup.GetStoreSettings();
            }
            return settings;
        }

        public StorageSettings GetParentStoreSettings()
        {
            return def.building.fixedStorageSettings;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
            Scribe_Deep.Look(ref settings, "settings", this);
            Scribe_References.Look(ref storageGroup, "storageGroup");
        }

    }
}