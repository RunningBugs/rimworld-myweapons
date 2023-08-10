using RimWorld.Planet;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using SRTS;

namespace MyWeapons
{
    //[HarmonyPatch(typeof(Caravan), nameof(Caravan.GetGizmos))]
    //public class CaravanGizmoPatch
    //{
    //    public CaravanGizmoPatch()
    //    {
    //    }

    //    //public static void GetGizmosPrefix(ref Caravan __instance, out Caravan __state)
    //    //{

    //    //}

    //    [HarmonyPostfix]
    //    public static IEnumerable<Gizmo> GetGizmosPostfix(IEnumerable<Gizmo> result, Caravan __instance)
    //    {
    //        var cmd = new Command_Action();
    //        cmd.defaultLabel = "GizmoLabel";
    //        cmd.defaultDesc = "GizmoDesc";
    //        cmd.icon = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true);
    //        cmd.action = delegate {
    //            var acceptLabels = new List<string>()
    //            {
    //                "MechanitorShip",
    //                "TransportShuttle",
    //                "TransportDrone"
    //            };
    //            var acceptedStrings = acceptLabels.Select( x => DefDatabase<ThingDef>.GetNamed(x).label );

    //            bool activate = false;
    //            CompLaunchableSRTS comp = null;
    //            Thing srts = null;
    //            foreach (var thing in __instance.AllThings)
    //            {
    //                if (activate = acceptedStrings.Contains(thing.Label))
    //                {
    //                    srts = thing;
    //                    comp = thing.TryGetComp<CompLaunchableSRTS>();
    //                    if (!comp.AllFuelingPortSourcesInGroupHaveAnyFuel)
    //                    {
    //                        cmd.Disable("CommandLaunchGroupFailNoFuel".Translate());
    //                    }
    //                    break;
    //                }
    //            }

    //            if (!activate)
    //            {
    //                cmd.Disable("NoShuttleAvailableInInventory");
    //            }
    //            else
    //            {
    //                if (srts != null)
    //                {
    //                    //ThingCompUtility.TryGetComp<CompLaunchableSRTS>(srts).WorldStartChoosingDestination(__instance);
    //                }
    //            }

    //        };

    //        foreach (var value in result)
    //        {
    //            yield return value;
    //        }
    //        yield return cmd;
    //    }
    //}

    //[HarmonyPatch(typeof(CompLaunchableSRTS), nameof(CompLaunchableSRTS.CompGetGizmosExtra))]
    //public class PatchCompLaunchableSRTS
    //{
    //    [HarmonyPostfix]
    //    public static IEnumerable<Gizmo> CompGetGizmosExtraPostfix(IEnumerable<Gizmo> result, CompLaunchableSRTS __instance)
    //    {
    //        foreach (var value in result)
    //        {
    //            yield return value;
    //        }
    //        Log.Warning("Triggered!");
    //    }
    //}
}
