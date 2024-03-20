using RimWorld;
using Verse;

namespace MyWeapons
{
    public class Verb_Shoot_WithComps : Verb_Shoot
    {
        protected override bool TryCastShot()
        {
            bool num = base.TryCastShot();

            Pawn casterPawn = CasterPawn;
            Thing thing = currentTarget.Thing;
            if (thing == null)
            {
                return num;
            }

            foreach (CompTargetEffect comp in base.EquipmentSource.GetComps<CompTargetEffect>())
            {
                comp.DoEffectOn(casterPawn, thing);
            }
            base.ReloadableCompSource?.UsedOnce();
            return num;
        }
    }
}
