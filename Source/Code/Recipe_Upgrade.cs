using System.Collections.Generic;
using Verse;
using RimWorld;
using System.Linq;

namespace AutomataUtility;

public class Recipe_Upgrade : Recipe_Surgery
{
	private static readonly Dictionary<string, string> upgradeMapping = new Dictionary<string, string>
	{
		{ "PN_SyncExcellent_Combat", "PN_SyncMasterwork_Combat" },
		{ "PN_SyncExcellent_Domestic", "PN_SyncMasterwork_Domestic" },
		{ "PN_SyncExcellent_Engineer", "PN_SyncMasterwork_Engineer" }
	};
	public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
	{

		Hediff existing = pawn.health.hediffSet.hediffs.FirstOrDefault(h => upgradeMapping.ContainsKey(h.def.defName));
		if (existing == null)
        {
			Log.Warning($"Pawn {pawn.Name} has no matching hediff.");
			return;
        }
		pawn.health.RemoveHediff(existing);
		
		if (!upgradeMapping.ContainsKey(existing.def.defName))
        {
			Log.Warning($"No upgrade mapping found for hediff {existing.def.defName}.");
			return;
        }
		
        Hediff upgraded = HediffMaker.MakeHediff(HediffDef.Named(upgradeMapping[existing.def.defName]), pawn);
        pawn.health.AddHediff(upgraded);
	}
}