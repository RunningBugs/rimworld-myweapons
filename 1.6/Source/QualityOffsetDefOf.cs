using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

using Logs = MyWeapons.Log;

namespace MyWeapons;

// class CompQualityOffsetManager
// {
//     private static Dictionary<Pawn, int> pawns = new Dictionary<Pawn, int>();

//     public static bool Contains(Pawn p)
//     {
//         return pawns.Keys.Contains(p) && pawns[p] > 0;
//     }

//     public static void Add(Pawn p)
//     {
//         if (pawns.Keys.Contains(p))
//         {
//             pawns[p]++;
//         }
//         else
//         {
//             pawns.Add(p, 1);
//         }
//     }

//     public static void Decrement(Pawn p)
//     {
//         if (pawns.Keys.Contains(p))
//         {
//             pawns[p]--;
//         }
//     }

//     public static void Remove(Pawn p)
//     {
//         if (Contains(p))
//         {
//             pawns.Remove(p);
//         }
//     }
// }

// class CompQualityOffset : ThingComp
// {
//     public CompProperties_QualityOffset Props
//     {
//         get
//         {
//             return (CompProperties_QualityOffset)this.props;
//         }
//     }

//     public override void Notify_Equipped(Pawn pawn)
//     {
//         base.Notify_Equipped(pawn);
//         CompQualityOffsetManager.Add(pawn);
//     }

//     public override void Notify_Unequipped(Pawn pawn)
//     {
//         base.Notify_Unequipped(pawn);
//         CompQualityOffsetManager.Decrement(pawn);
//     }

//     public override void Notify_KilledPawn(Pawn pawn)
//     {
//         base.Notify_KilledPawn(pawn);
//         CompQualityOffsetManager.Remove(pawn);
//     }

//     public override void PostExposeData()
//     {
//         base.PostExposeData();
//         if (parent.ParentHolder is Pawn_ApparelTracker pa && pa.pawn != null)
//         {
//             CompQualityOffsetManager.Add(pa.pawn);
//         }
//     }
// }

[DefOf]
class QualityOffsetDefOf {
    public static StatDef MW_PawnCreatedQualityOffset;
}

[HarmonyPatch(typeof(QualityUtility), "GenerateQualityCreatedByPawn", new System.Type[] { typeof(Pawn), typeof(SkillDef), typeof(bool) })]
class QualityUtilityGenerateQualityCreatedByPawnPatch
{
    private static QualityCategory AddLevels(QualityCategory quality, int levels)
    {
        return (QualityCategory)Mathf.Max(Mathf.Min((int)quality + levels, 6), 0);
    }

    static void Postfix(Pawn pawn, ref QualityCategory __result)
    {
        // if (CompQualityOffsetManager.Contains(pawn))
        {
            int offset = (int)pawn.GetStatValue(QualityOffsetDefOf.MW_PawnCreatedQualityOffset);
            __result = AddLevels(__result, offset);
        }
    }

}

// class CompProperties_QualityOffset : CompProperties
// {
//     public CompProperties_QualityOffset()
//     {
//         this.compClass = typeof(CompQualityOffset);
//     }
//     public int offset = 2;
// }