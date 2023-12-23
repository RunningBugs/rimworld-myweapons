using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace MyWeapons
{
    class PrayRecipeWindow : Window
    {
        QuickSearchWidget searchWidget = new QuickSearchWidget();

        private float y = 0f;

        private const float lineHeight = 30f;
        private const float scrollbarWidth = 16f;
        private static int recipeId = 0;

        private Vector2 scrollPosition = Vector2.zero;
        private Vector2 createdRecipesScrollPosition = Vector2.zero;
        private Dictionary<ThingDef, Rect> selectedDefs = new Dictionary<ThingDef, Rect>();
        static private List<RecipeDef> createdRecipes = new List<RecipeDef>();

        public static List<RecipeDef> CreatedRecipes { get => createdRecipes; set => createdRecipes = value; }


        public static readonly AccessTools.FieldRef<Dictionary<Type, HashSet<ushort>>> takenShortHashes = AccessTools.StaticFieldRefAccess<Dictionary<Type, HashSet<ushort>>>(AccessTools.Field(typeof(ShortHashGiver), "takenHashesPerDeftype"));
        private delegate void GiveShortHash(Def def, Type defType, HashSet<ushort> takenHashes);
        private static readonly GiveShortHash giveShortHash = AccessTools.MethodDelegate<GiveShortHash>(AccessTools.Method(type: typeof(ShortHashGiver), name: "GiveShortHash"));

        private void GapVertical(float gap = lineHeight)
        {
            y += gap;
        }

        private void GetRow(out Rect top, out Rect bot, Rect inRect, float height = lineHeight)
        {
            top = inRect.TopPartPixels(height);
            bot = inRect.BottomPartPixels(inRect.height - height);
            y += height;
        }

        public PrayRecipeWindow() : base()
        {
            draggable = true;
            resizeable = true;
        }
        public override void DoWindowContents(Rect inRect)
        {
            /// Search Widget
            GetRow(out Rect top, out Rect bot, inRect);
            searchWidget.OnGUI(top, delegate
            {
                selectedDefs.Clear();
            });
            GetRow(out top, out bot, bot);
            Widgets.Label(top, searchWidget.filter.Text);

            /// Scroll area (searching results)
            var textLineHeight = Text.LineHeight + 2f;
            GetRow(out top, out bot, bot, 10 * textLineHeight);
            Rect viewRect = top;
            Rect scrollRect = viewRect;
            List<ThingDef> defs = new List<ThingDef>();
            foreach (ThingDef def in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                // if (!searchWidget.filter.Text.Trim().NullOrEmpty() && searchWidget.filter.Matches(def.label))
                if (searchWidget.filter.Active && searchWidget.filter.Matches(def.label))
                {
                    defs.Add(def);
                }
                if (defs.Count > 50)
                {
                    break;
                }
            }
            scrollRect.width -= scrollbarWidth;
            scrollRect.height = defs.Count * textLineHeight;
            Widgets.BeginScrollView(viewRect, ref scrollPosition, scrollRect, true);
            foreach (ThingDef def in defs)
            {
                GetRow(out top, out scrollRect, scrollRect, textLineHeight);
                Widgets.DefLabelWithIcon(top, def);

                if (Widgets.ButtonInvisible(top))
                {
                    if (selectedDefs.ContainsKey(def))
                    {
                        selectedDefs.Remove(def);
                    }
                    else
                    {
                        selectedDefs.Add(def, top);
                    }
                }
            }

            foreach (KeyValuePair<ThingDef, Rect> kvp in selectedDefs)
            {
                Widgets.DrawHighlight(kvp.Value);
            }
            Widgets.EndScrollView();

            /// Gap
            GetRow(out top, out bot, bot);
            /// Bottom buttons
            /// Pray button
            GetRow(out top, out bot, bot);
            var buttonRect = top.LeftPartPixels(100f);
            if (Widgets.ButtonText(buttonRect, "Pray".Translate()))
            {
                if (!selectedDefs.NullOrEmpty())
                {
                    RecipeDef recipe = new RecipeDef();
                    recipe.defName = "Pray_" + recipeId++;
                    recipe.label = "PrayRecipeLabel".Translate(string.Join(", ", selectedDefs.Keys.Select(d => d.label).ToArray()));
                    recipe.jobString = "WorkingOnPrayedRecipe".Translate();
                    recipe.workAmount = 1f;
                    var speedStat = DefDatabase<StatDef>.GetNamed("DrugCookingSpeed");
                    recipe.workSpeedStat = speedStat;
                    recipe.workSkill = SkillDefOf.Crafting;
                    recipe.effectWorking = DefDatabase<EffecterDef>.GetNamed("Cook");
                    recipe.targetCountAdjustment = 5;
                    ThingDef praySpot2 = DefDatabase<ThingDef>.GetNamed("PraySpotTwo");
                    recipe.recipeUsers = new List<ThingDef>
                    {
                        praySpot2
                    };
                    recipe.defaultIngredientFilter = new ThingFilter();
                    recipe.descriptionHyperlinks = new List<DefHyperlink>();
                    foreach (ThingDef def in selectedDefs.Keys)
                    {
                        recipe.products.Add(new ThingDefCountClass(def, def.stackLimit));
                        recipe.descriptionHyperlinks.Add(new DefHyperlink(def));
                    }

                    recipe.ResolveReferences();
                    recipe.PostLoad();
                    giveShortHash(recipe, typeof(RecipeDef), takenShortHashes()[typeof(RecipeDef)]);

                    DefDatabase<RecipeDef>.Add(recipe);
                    CreatedRecipes.Insert(0, recipe);
                }

            }

            /// Close button
            buttonRect = top.RightPartPixels(100f);
            GUI.color = new Color(1f, 0.3f, 0.35f);
            if (Widgets.ButtonText(buttonRect, "Close".Translate()))
            {
                // GUI.color = Color.white;
                ToggleIconPatcher.flag = false;
                Close();
            }
            GUI.color = Color.white;

            /// Scrollview for created recipes
            viewRect = bot;
            scrollRect = viewRect;
            scrollRect.width -= scrollbarWidth;
            scrollRect.height = CreatedRecipes.Count * textLineHeight;
            Widgets.BeginScrollView(viewRect, ref createdRecipesScrollPosition, scrollRect, true);
            List<RecipeDef> toRemove = new List<RecipeDef>();
            foreach (RecipeDef recipe in CreatedRecipes)
            {
                GetRow(out top, out scrollRect, scrollRect, textLineHeight);
                Rect iconRect = top.LeftPartPixels(textLineHeight);
                Rect labelRect = top.RightPartPixels(top.width - textLineHeight);
                iconRect = iconRect.ContractedBy(2f);
                labelRect = labelRect.ContractedBy(2f);
                if (Widgets.ButtonImage(iconRect, TexButton.CloseXSmall))
                {
                    toRemove.Add(recipe);
                }
                Widgets.Label(labelRect, string.Join(", ", recipe.products.ConvertAll(p => p.thingDef.label).ToArray()));
            }
            foreach (RecipeDef recipe in toRemove)
            {
                CreatedRecipes.Remove(recipe);
            }
            Widgets.EndScrollView();
        }
    }


    [HarmonyPatch(typeof(ThingDef), "AllRecipes", MethodType.Getter)]

    class ThingDef_AllRecipes_Patch
    {
        static void Postfix(ThingDef __instance, ref List<RecipeDef> __result)
        {
            if (__instance.defName == "PraySpotTwo")
            {
                __result = PrayRecipeWindow.CreatedRecipes;
            }
        }
    }
}