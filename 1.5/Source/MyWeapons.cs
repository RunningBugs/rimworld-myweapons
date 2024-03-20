using Verse;
using HarmonyLib;
using System.Reflection;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;


namespace MyWeapons
{
    [StaticConstructorOnStartup]
    public static class LoadingScreen
    {
        static LoadingScreen()
        {
            Log.Warning("My Weapons Loaded");

            ToggleIconData.setupToggleIcon(typeof(PrayRecipeWindow), ContentFinder<Texture2D>.Get("MyWeapons/WindowIcon", true), "SampleWindowTooltip".Translate(), SoundDefOf.Mouseover_ButtonToggle, delegate
            {
                Find.WindowStack.Add(new PrayRecipeWindow());
            });

            var harmony = new Harmony("com.RunningBugs.MyWeapons");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }


	


    public static class Log
    {
        const string prefix = "[MyWeapons] ";
        public static void Warning(string msg, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            Verse.Log.Warning(prefix + $"[ {fileName}:{lineNumber} {memberName} ]" + msg);
        }
    }
}