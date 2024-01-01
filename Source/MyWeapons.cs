using Verse;
using HarmonyLib;
using System.Reflection;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using System.Windows.Forms;


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

    [DefOf]
    public static class MyLetterDefOf {
        public static LetterDef NeutralEventCopyLetter;
    }


	public class WorldSeedGameComponent : GameComponent
	{
		public WorldSeedGameComponent(Game game) : base()
		{
		}

        public override void FinalizeInit()
        {
            base.FinalizeInit();
			var seed = Find.World.info.seedString;
            Find.LetterStack.ReceiveLetter("WorldSeedLtterTitle".Translate(seed), seed, MyLetterDefOf.NeutralEventCopyLetter);
        }
    }



    public class StandardLetterCopyOnClick : StandardLetter
    {
        /**
         *  Not working because Rimdeed has a prefix patch on OpenLetter, seems to skip this method
         */
        public override void OpenLetter()
        {
            var text = Text.Resolve();
            GUIUtility.systemCopyBuffer = text;
            // Log.Warning("OpenLetter");
            // Log.Warning(text);
            // Clipboard.SetText(text);
            Messages.Message("WorldSeedCopied".Translate(text), MessageTypeDefOf.NeutralEvent);
            // Log.Warning("Copied");
            base.OpenLetter();
        }

        // [HarmonyPatch(typeof(StandardLetterCopyOnClick), "OpenLetter")]
        // public static void Prefix(StandardLetterCopyOnClick __instance)
        // {
        //     var text = __instance.Text.Resolve();
        //     Log.Warning(text);
        //     Clipboard.SetText(text);
        //     Messages.Message("WorldSeedCopied".Translate(text), MessageTypeDefOf.NeutralEvent);
        //     Log.Warning("Copied");
        // }
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