using RimWorld;
using RimWorld.Planet;
using Verse;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;


namespace MyWeapons
{
    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }
    }

    [StaticConstructorOnStartup]
    public static class LoadingScreen
    {
        static LoadingScreen()
        {
            Log.Warning("My Weapons Loaded");

            var harmony = new Harmony("com.RunningBugs.MyWeapons");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    //[HarmonyPatch(typeof(FloatMenuMakerMap), "ChoicesAtFor")]
    //public class InspectValue
    //{
    //    [HarmonyPrefix]
    //    public static void ChoicesAtFor(Vector3 clickPos, Pawn pawn, bool suppressAutoTakeableGoto = false)
    //    {
    //        TargetingParameters targetingParameters = new TargetingParameters();
    //        targetingParameters.canTargetPawns = true;
    //        targetingParameters.canTargetBuildings = true;
    //        targetingParameters.canTargetItems = true;
    //        targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;

    //        string msg = "";


    //        foreach (Thing item in GenUI.ThingsUnderMouse(clickPos, 1f, targetingParameters))
    //        {
    //            foreach (WorkTypeDef item2 in DefDatabase<WorkTypeDef>.AllDefsListForReading)
    //            {
    //                for (int i = 0; i < item2.workGiversByPriority.Count; ++i)
    //                {
    //                    WorkGiverDef workGiver = item2.workGiversByPriority[i];
    //                    if (workGiver.Worker is WorkGiver_Scanner scanner)
    //                    {
    //                        //Log.Warning("Is WorkGiver_Scanner");
    //                        try
    //                        {
    //                            msg += $"Target Worktype: {scanner.def.label}, {scanner.HasJobOnThing(pawn, item, true)}\n";
    //                        }
    //                        catch (System.Exception e)
    //                        {

    //                        }
    //                    }
    //                    else
    //                    {
    //                        //Log.Warning("Not WorkGiver_Scanner");
    //                    }
    //                }
    //            }
    //        }


    //        Log.Warning(msg);
    //    }
    //}

    [HarmonyPatch(typeof(WorldComponentUtility), "WorldComponentTick")]
    public class AlertUtility
    {
        public class Event
        {
            public int presetGameTicksToAlert;
            public string message;

            public Event(int tickTime, string msg)
            {
                presetGameTicksToAlert = tickTime;
                message = msg;
            }
        }

        private static int defaultInterval = 60;   //  Check on every second in the slow speed
        private static List<Event> events = new List<Event>();

        public static void Add(Event e)
        {
            events.Add(e);
        }

        public static List<Event> GetEvents()
        {
            return events;
        }

        [HarmonyPostfix]
        public static void WorldComponentTickPostfix()
        {
            if (Find.World != null)
            {
                int ticks = Find.TickManager.TicksGame;
                if (ticks % defaultInterval == 0)
                {
                    List<Event> eventsToRemove = new List<Event>();
                    foreach (var e in events)
                    {
                        if (ticks >= e.presetGameTicksToAlert)
                        {
                            // Trigger Alert then remove this element
                            Find.LetterStack.ReceiveLetter("TimerTimeOut".Translate(e.message.Truncate(5)), e.message, LetterDefOf.ThreatBig);
                            eventsToRemove.Add(e);
                        }
                    }
                    foreach (var e in eventsToRemove)
                    {
                        events.Remove(e);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(PlaySettings), "DoPlaySettingsGlobalControls", MethodType.Normal)]
    public class ToggleIconPatcher
    {
        [HarmonyPostfix]
        public static void AddIcon(WidgetRow row, bool worldView)
        {
            if (worldView) return;
            bool flag = Find.WindowStack.IsOpen(typeof(TimedMailWindow));
            row.ToggleableIcon(ref flag, ContentFinder<Texture2D>.Get("UI/timer_mail", true), "WriteMailToFuture".Translate(), SoundDefOf.Mouseover_ButtonToggle, (string)null);
            if (flag != Find.WindowStack.IsOpen(typeof(TimedMailWindow)))
            {
                if (!Find.WindowStack.IsOpen(typeof(TimedMailWindow)))
                {
                    TimedMailWindow.DrawWindow(AlertUtility.GetEvents());
                }
                else
                {
                    Find.WindowStack.TryRemove(typeof(TimedMailWindow), false);
                }
            }
        }
    }

    public static class Log
    {
        const string prefix = "[MyWeapons] ";
        public static void Warning(string msg)
        {
            Verse.Log.Warning(prefix + msg);
        }
    }
}