using System.Collections.Generic;
using Verse;
using RimWorld;

namespace AutomataUtility;

public class Recipe_Upgrade_to_Good: Recipe_Surgery
{
    public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
    {
        return base.AvailableOnNow(thing, part) && Recipe_Upgrade.isAvailableFor(thing as Pawn, "PN_SyncNormal");
    }

    public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
    {
        Recipe_Upgrade.upgrade(pawn);
    }
}