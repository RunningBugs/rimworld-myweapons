using RimWorld;
using Verse;

namespace MyWeapons
{
    [StaticConstructorOnStartup]
    public static class LoadingScreen
    {
        static LoadingScreen()
        {
            Log.Message("My Weapons Initialized");
        }
    }
}