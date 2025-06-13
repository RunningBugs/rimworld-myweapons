using System.Collections.Generic;
using MyWeapons;
using RimWorld;
using UnityEngine;
using Verse;

using Logs = MyWeapons.Log;

public class CompProperties_Pray : CompProperties
{
    public CompProperties_Pray()
    {
        compClass = typeof(CompPray);
    }
}

public class CompPray : ThingComp
{
    public CompProperties_Pray Props => (CompProperties_Pray)props;

    public List<Pair<StatDef, float>> statOffsets;

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        yield return new Command_Action
        {
            defaultLabel = "Pray".Translate(),
            defaultDesc = "PrayDesc".Translate(),
            icon = ContentFinder<Texture2D>.Get("UI/Commands/Pray"),
            action = delegate
            {
                Logs.Message("Praying...");
            }
        };
    }
}