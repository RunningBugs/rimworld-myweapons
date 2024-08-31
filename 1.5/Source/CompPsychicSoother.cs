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
                if (pawn.InAggroMentalState)
                {
                    pawn.MentalState.RecoverFromState();
                    Messages.Message("PsychicSootherActOn".Translate(pawn.Name.ToStringFull), pawn, MessageTypeDefOf.PositiveEvent);
                }
                else if (GenHostility.HostileTo(pawn, Faction.OfPlayer))
                {
                    var lord = map.lordManager.LordOf(pawn);
                    if (lord != null && lord.LordJob is LordJob_PrisonBreak)
                    {
                        lord.RemoveAllPawns();
                    }
                    pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
                    Messages.Message("PsychicSootherActOn".Translate(pawn.Name.ToStringFull), pawn, MessageTypeDefOf.PositiveEvent);
                }
            });
        }
    }
}