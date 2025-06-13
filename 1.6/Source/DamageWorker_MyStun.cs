using RimWorld;
using UnityEngine;
using Verse;
using Logs = MyWeapons.Log;

namespace MyWeapons;

[DefOf]
public class MyDefOfs
{
    public static DamageDef MyStun;
}

class DamageWorker_MyStun : DamageWorker
{
    public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
    {
        if (dinfo.Def == MyDefOfs.MyStun)
        {
            if (!victim.HostileTo(dinfo.Instigator))
            {
                return new DamageWorker.DamageResult();
            }
            var ticks = (int)(dinfo.Amount * 30f);
            if (victim is Pawn pawn)
            {
                pawn.stances?.stunner.StunFor(ticks, dinfo.Instigator);
            }
            else if (victim is Building)
            {
                CompStunnable comp = victim.TryGetComp<CompStunnable>();
                comp?.StunHandler.StunFor(ticks, dinfo.Instigator);
            }
            return new DamageWorker.DamageResult();
        }
        else
        {
            return base.Apply(dinfo, victim);
        }
    }
}