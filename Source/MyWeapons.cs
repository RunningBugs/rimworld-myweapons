using Verse;
using HarmonyLib;
using System.Reflection;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Revolus.MoreAutosaveSlots;
using System.Linq;
using System.IO;
using aRandomKiwi.ARS;


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



    // public class Tracker : MapComponent
    // {
    //     public Tracker(Map map) : base(map)
    //     {
    //     }

    //     public override void MapComponentTick()
    //     {
    //         base.MapComponentTick();
    //         if (Find.TickManager.TicksGame % 100 == 0)
    //         {
    //             foreach (var thing in Find.CurrentMap.spawnedThings)
    //             {
    //                 if (thing is Pawn p && p.RaceProps.IsMechanoid)
    //                 {
    //                     Log.Warning(p.Name + $"{p.def}");
    //                 }
    //             }
    //         }
    //     }
    // }

    public static class Log
    {
        const string prefix = "[MyWeapons] ";
        public static void Warning(string msg, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            Verse.Log.Warning(prefix + $"[ {fileName}:{lineNumber} {memberName} ]" + msg);
        }
    }


    [HarmonyPatch(typeof(MoreAutosaveSlotsSettings))]
    public static class PatchMoreSaveSlots
    {
        [HarmonyPrefix]
        [HarmonyPatch("NextName")]
        public static void NextName(ref string __result)
        {
            var texts = MoreAutosaveSlotsSettings.AutoSaveNames();
            {
                string info = "";
                foreach (var txt in texts)
                {
                    info += $"{txt} {SaveGameFilesUtility.SavedGameNamedExists(txt)} exists\n";
                }
                {
                    var txt = "食人族-麋鹿新居 (1)";
                    info += $"{txt} {SaveGameFilesUtility.SavedGameNamedExists(txt)} exists\n";
                }
                Log.Warning(info);
            }

            var text = (from name in texts where !SaveGameFilesUtility.SavedGameNamedExists(name) select name).FirstOrDefault();
            var text2 = texts.MinBy((string name) => new FileInfo(GenFilePaths.FilePathForSavedGame(name)).LastWriteTime);
            Log.Warning($"{text}");
            Log.Warning($"{text2}");
        }
    }

    [HarmonyPatch]
    public static class PatchSaveGameFilesUtilitySavedGameNamedExistsOnlyWhenRimSavesExists
    {
        public static readonly string VFOLDERSEP = "#§#";

        private static bool SavedGameNamedExists(string fileName)
        {
            var curFolder = Settings.curFolder;
            var fsep = VFOLDERSEP;

            string prefix = "";

            if (curFolder != "Default")
                prefix = curFolder + fsep;

            fileName = prefix + fileName;

            foreach (string item in GenFilePaths.AllSavedGameFiles.Select((FileInfo f) => Path.GetFileNameWithoutExtension(f.Name)))
            {
                if (item == fileName)
                {
                    return true;
                }
            }
            return false;
        }

        [HarmonyPatch(typeof(SaveGameFilesUtility))]
        [HarmonyPatch("SavedGameNamedExists")]
        [HarmonyPostfix]
        public static void Postfix(ref bool __result, string fileName)
        {
            if (!__result)
            {
                __result = SavedGameNamedExists(fileName);
            }
        }
    }

}