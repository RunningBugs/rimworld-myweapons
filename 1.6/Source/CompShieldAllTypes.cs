using RimWorld;
using Verse;

namespace MyWeapons {
    public class CompShieldAllTypes : CompShield {
        public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            DamageInfo tmp = dinfo;
            tmp.Def.isRanged = true;
            base.PostPreApplyDamage(ref tmp, out absorbed);
        }
    }
}