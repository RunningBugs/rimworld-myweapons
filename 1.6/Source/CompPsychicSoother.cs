using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace MyWeapons;

class CompPsychicSoother : ThingComp
{
    public CompProperties Props => props;

    public override void CompTick()
    {
        var map = parent?.Map;

        if (map == null)
        {
            return;
        }

        if (parent.IsHashIntervalTick(250))
        {
            map.mapPawns.SlavesAndPrisonersOfColonySpawned.Concat(map.mapPawns.FreeColonistsSpawned).ToList().ForEach(pawn =>
            {
                var mentalStateHandler = pawn.mindState.mentalStateHandler;
                var curState = mentalStateHandler.CurState;

                if (curState != null)
                {
                    mentalStateHandler.CurState.RecoverFromState();
                    Messages.Message("PsychicSootherActOn".Translate(pawn.Name.ToStringFull), pawn, MessageTypeDefOf.PositiveEvent);
                }

                if (GenHostility.HostileTo(pawn, Faction.OfPlayer))
                {
                    var lord = map.lordManager.LordOf(pawn);
                    if (lord != null && lord.LordJob is LordJob_PrisonBreak)
                    {
                        var pawns = lord.ownedPawns;
                        lord.RemoveAllPawns();
                        foreach (var p in pawns)
                        {
                            mentalStateHandler.Reset();
                            pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
                            Messages.Message("PsychicSootherActOn".Translate(pawn.Name.ToStringFull), pawn, MessageTypeDefOf.PositiveEvent);
                        }
                    }
                }
                // if (pawn.InAggroMentalState)
                // {
                //     pawn.MentalState.RecoverFromState();
                //     Messages.Message("PsychicSootherActOn".Translate(pawn.Name.ToStringFull), pawn, MessageTypeDefOf.PositiveEvent);
                // }
                // else if (GenHostility.HostileTo(pawn, Faction.OfPlayer))
                // {
                //     var lord = map.lordManager.LordOf(pawn);
                //     if (lord != null && lord.LordJob is LordJob_PrisonBreak)
                //     {
                //         var pawns = lord.ownedPawns;
                //         lord.RemoveAllPawns();
                //         foreach (var p in pawns)
                //         {
                //             pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
                //             var mentalStateHandler = pawn.mindState.mentalStateHandler;
                //             var curState = mentalStateHandler.CurState;
                //             if (curState != null)
                //             {
                //                 mentalStateHandler.CurState.RecoverFromState();
                //             }
                //             mentalStateHandler.Reset();
                //             Messages.Message("PsychicSootherActOn".Translate(pawn.Name.ToStringFull), pawn, MessageTypeDefOf.PositiveEvent);
                //         }
                //     }
                // }
            });
        }
    }
}