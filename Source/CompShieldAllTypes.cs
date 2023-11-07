using RimWorld;
using Verse;

namespace MyWeapons {
    public class CompShieldAllTypes : CompShield {
        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            DamageInfo tmp = dinfo;
            tmp.Def.isRanged = true;
            base.PostPreApplyDamage(tmp, out absorbed);
        }
    }
}