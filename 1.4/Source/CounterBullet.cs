using RimWorld;
using Verse;
using System.Collections.Generic;

namespace MyWeapons
{
    public class CounterDef
    {
        public class CounterDamage
        {
            public float amount;
            public float armorPenetration;
            public float chance;
        }
        public DamageDef damageDef;
        public FleshTypeDef fleshType;
        public CounterDamage extraDamage;
    }

    public class CounterBulletProps : DefModExtension
    {
        public List<CounterDef> counters;
    }

    public class CounterBullet : Bullet
    {
        private CounterBulletProps props = null;

        //public CounterBullet()
        //{
        //    Log.Warning("MyWeapons.CounterBullet ctor called!");
        //}

        public float AdjustedArmorPenetration(CounterDef.CounterDamage damage)
        {
            if (damage.armorPenetration < 0f)
            {
                return damage.amount * 0.015f;
            }
            return damage.armorPenetration;
        }

        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            base.Impact(hitThing, blockedByShield);

            props = def.GetModExtension<CounterBulletProps>();

            if (hitThing is Pawn pawn && props != null)
            {
                foreach (var counter in props.counters) {
                    if (counter.fleshType == pawn.RaceProps.FleshType)
                    {
                        Log.Warning($"fleshType same {counter.fleshType} vs {pawn.RaceProps.FleshType}");
                        var extraDamage = counter.extraDamage;
                        if (Rand.Chance(extraDamage.chance))
                        {
                            bool instigatorGuilty = !(launcher is Pawn p) || !p.Drafted;
                            BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(launcher, hitThing, intendedTarget.Thing, equipmentDef, def, targetCoverDef);
                            DamageInfo dinfo = new DamageInfo(counter.damageDef, extraDamage.amount, AdjustedArmorPenetration(extraDamage), ExactRotation.eulerAngles.y, launcher, null, equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, intendedTarget.Thing, instigatorGuilty);
                            hitThing.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_RangedImpact);
                        }
                    }
                    else
                    {
                        Log.Warning($"fleshType different {counter.fleshType} vs {pawn.RaceProps.FleshType}");
                    }
                }
            }
        }
    }
}
