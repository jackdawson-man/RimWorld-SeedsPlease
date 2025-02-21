using Verse;
using RimWorld;
using HarmonyLib;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
 
namespace SeedsPleaseLite
{
    [HarmonyPatch (typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PostResolve))]
    static class Patch_DefGenerator
    {
        static void Postfix()
        {
            //Resolve references, which validates the seeds are configured right and also sets their market value
            DefDatabase<ThingDef>.AllDefs.Where(x => x.HasModExtension<Seed>()).ToList().ForEach(y => Mod_SeedsPlease.ResolveReferences(y));            

            var report = new System.Text.StringBuilder();
            if (Mod_SeedsPlease.AddMissingSeeds(report))
            {
                ResourceCounter.ResetDefs();
                Log.Warning("SeedsPlease :: Some Seeds were autogenerated.\nDon't rely on autogenerated seeds, share the generated xml for proper support.\n\n" + report);
            }
            ResourceBank.ThingCategoryDefOf.SeedsCategory.ResolveReferences();
            Mod_SeedsPlease.AddButchery();
        }
    }
}