using RimWorld;
using Verse;
using UnityEngine;
using HarmonyLib;

namespace MyWeapons
{
    public class TimedMailWindow : Window
    {

        public override Vector2 InitialSize => new Vector2(600f, 350f);

        private string buffer = "0";
        private string txtBuffer = "";

        // Seems like starting x, y and width height
        protected override void SetInitialSizeAndPosition()
        {
            base.windowRect = new Rect((UI.screenWidth - this.InitialSize.x) / 2, (UI.screenHeight - this.InitialSize.y)/2, ((Window)this).InitialSize.x, ((Window)this).InitialSize.y);
        }

        public override void PreOpen()
        {
            base.PreOpen();
        }

        public override void DoWindowContents(Rect inRect)
        {
            var listView = new Listing_Standard(GameFont.Small);
            listView.Begin(inRect);
            Text.Anchor = TextAnchor.MiddleCenter;
            listView.Label("SetTimer".Translate());
            listView.Gap();

            Text.Anchor = TextAnchor.MiddleLeft;
            int ticks = 0;
            listView.TextFieldNumericLabeled<int>("TicksToAlert".Translate(), ref ticks, ref buffer);
            listView.Gap();

            listView.Label("TicksToAlertExplaination".Translate());
            listView.Gap();

            Text.Anchor = TextAnchor.MiddleLeft;
            txtBuffer = listView.TextEntryLabeled("LetterMessage".Translate(), txtBuffer, 5);
            listView.Gap();

            bool close = listView.ButtonText("Set".Translate());
            if (close)
            {
                int ticksToAlert = Find.TickManager.TicksGame;
                AlertUtility.Add(new AlertUtility.Event(ticksToAlert + ticks, txtBuffer));
                Find.WindowStack.TryRemove(typeof(TimedMailWindow));
            }

            GenUI.ResetLabelAlign();
            listView.End();

        }

        public override void PostClose()
        {
        }

        public static void DrawWindow()
        {
            Find.WindowStack.Add((Window)(object)new TimedMailWindow());
        }
    }

}
