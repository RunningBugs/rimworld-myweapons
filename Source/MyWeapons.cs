using Verse;
using HarmonyLib;
using System.Reflection;


namespace MyWeapons
{
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

    public static class Log
    {
        const string prefix = "[MyWeapons] ";
        public static void Warning(string msg)
        {
            Verse.Log.Warning(prefix + msg);
        }
    }
}