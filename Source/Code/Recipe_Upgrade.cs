using System.Collections.Generic;
using Verse;
using RimWorld;
using System.Linq;
using UnityEngine;

namespace AutomataUtility;

public static class Recipe_Upgrade
{
	private static readonly Dictionary<string, string> upgradeMapping = new Dictionary<string, string>
	{
		{ "PN_SyncNormal_Combat",       "PN_SyncGood_Combat" },
		{ "PN_SyncNormal_Domestic",     "PN_SyncGood_Domestic" },
		{ "PN_SyncNormal_Engineer",     "PN_SyncGood_Engineer" },
		{ "PN_SyncGood_Combat",         "PN_SyncExcellent_Combat" },
		{ "PN_SyncGood_Domestic",       "PN_SyncExcellent_Domestic" },
		{ "PN_SyncGood_Engineer",       "PN_SyncExcellent_Engineer" },
		{ "PN_SyncExcellent_Combat",    "PN_SyncMasterwork_Combat" },
		{ "PN_SyncExcellent_Domestic",  "PN_SyncMasterwork_Domestic" },
		{ "PN_SyncExcellent_Engineer",  "PN_SyncMasterwork_Engineer" },
		{ "PN_SyncMasterwork_Combat",   "PN_SyncLegendary_Combat" },
		{ "PN_SyncMasterwork_Domestic", "PN_SyncLegendary_Domestic" },
		{ "PN_SyncMasterwork_Engineer", "PN_SyncLegendary_Engineer" }
	};

	public static bool isAvailableFor(Pawn pawn, string quality)
	{

		Hediff current = pawn.health.hediffSet.hediffs.FirstOrDefault(h => upgradeMapping.ContainsKey(h.def.defName));
		// Log.Warning($"ttt {current.def.defName} {quality} {current.def.defName.StartsWith(quality)}");
		if (current == null) return false;
		return current.def.defName.StartsWith(quality);
	}
	
	public static void upgrade(Pawn pawn)
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
		
		foreach (var skill in pawn.skills.skills)
        {

            skill.Level = Mathf.Clamp(skill.Level + 2, 0, 20);
            skill.Notify_SkillDisablesChanged();
        }
	}
}