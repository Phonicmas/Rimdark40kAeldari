### WebwayRewardDefs:

rewardThing (ThingDef): The thingDef to spawn when reward is chosen
rewardThingCount (int): Amount of said thingDef to spawn when reward is chosen

giveQuality (bool): Whether or not to give the spawned thingDef a Minimum level of quality (Will most likely error if used on thingDefs without the quality comp)
rewardCategoryMinimum (QualityCategory): Minimum level of quality if above setting is true

rewardPawn (PawnKindDef): The pawnKindDef to spawn if the reward is chosen (Note that if both this and rewardThing is on a WebwayRewardDefs, the rewardPawn will take priority and you will only get the pawn)
showAmountInTooltip: Whether or not to show the amount given of given reward when mouse is hovering the reward


### Force weapons

To make a force weapon you need to do a few things.

First you need a DamageDef to have this DefModExtension: "Aeldari40k.DefModExtension_ScalingDamage" - see "BEWH_ForceCutScaling"
Second you need the ThingDef of the weapon to have this Comp: "Aeldari40k.CompProperties_ForceWeapon" - see "BEWH_WitchBlade"
Third you need to add the DamageDef to the ThingDef as extraMeleeDamages under tools for the attack you wish to have to extra scaling damage (It initial damage and armor pen shouldn't matter as its replaced upon equipping the weapon). - see "BEWH_WitchBlade"