using RimWorld;
using Verse;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;

namespace MyWeapons
{
    public class TimedMailWindow : Window
    {

        public override Vector2 InitialSize => new Vector2(600f, 330f);

        private string buffer = "0";
        private string txtBuffer = "";
        private int unit = 0;

        // Seems like starting x, y and width height
        protected override void SetInitialSizeAndPosition()
        {
            base.windowRect = new Rect((UI.screenWidth - this.InitialSize.x) / 2, (UI.screenHeight - this.InitialSize.y)/2, ((Window)this).InitialSize.x, ((Window)this).InitialSize.y);
        }

        public override void PreOpen()
        {
            base.PreOpen();
        }

        private int radioButtonGroupHorizontal(Rect inRect, List<string> labels, int res)
        {
            int count = labels.Count;
            float width = inRect.width / count;
            List<Rect> rects = new List<Rect>();

            for (int i = 0; i < count; ++i)
            {
                Rect rect = new Rect(inRect.x + width * i, inRect.y, width, inRect.height);
                rects.Add(rect);
            }

            int num = res;
            if (res >= 0 && res < count)
            {
                for (int i = 0; i < count; ++i)
                {
                    if (Widgets.RadioButtonLabeled(rects[i].LeftHalf().Rounded(), labels[i], i == res))
                    {
                        num = i;
                    }
                }
            }

            return num;
        }

        private int getMultiplier(int unit)
        {
            switch (unit)
            {
                case 0:
                    return 1;
                case 1:
                    return GenDate.TicksPerHour;
                case 2:
                    return GenDate.TicksPerDay;
                case 3:
                    return GenDate.TicksPerYear;
                default:
                    return 1;
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            var listView = new Listing_Standard(GameFont.Small);
            listView.Begin(inRect);
            Text.Anchor = TextAnchor.MiddleCenter;
            listView.Label("SetTimer".Translate());
            listView.Gap();

            // Unit selection: ticks, hours, days, years
            //  Could create a RadioButtonGroup function
            List<string> labels = new List<string>();
            labels.Add("ticks");
            labels.Add("hours");
            labels.Add("days");
            labels.Add("years");
            Log.Warning($"{unit}");
            unit = radioButtonGroupHorizontal(listView.GetRect(Text.LineHeight), labels, unit);
            Log.Warning($"{unit}");
            listView.Gap();

            int ticks = 0, real_ticks = 0;
            listView.TextFieldNumericLabeled<int>("TicksToAlert".Translate(), ref ticks, ref buffer);
            listView.Gap();

            listView.Label("TicksToAlertExplaination".Translate());
            listView.Gap();

            Text.Anchor = TextAnchor.MiddleLeft;
            txtBuffer = listView.TextEntryLabeled("LetterMessage".Translate(), txtBuffer, 2);
            listView.Gap();

            bool close = listView.ButtonText("Set".Translate());
            if (close)
            {
                int ticksToAlert = Find.TickManager.TicksGame;
                int multiplier = getMultiplier(unit);
                real_ticks = ticks * multiplier;
                AlertUtility.Add(new AlertUtility.Event(ticksToAlert + real_ticks, txtBuffer));
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
