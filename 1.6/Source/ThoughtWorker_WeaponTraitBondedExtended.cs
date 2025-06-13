using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MyWeapons
{
    public class ThoughtWorker_WeaponTraitBondedExtended : ThoughtWorker_WeaponTraitBonded
    {
        private ThoughtState baseCurrentStateInternal(Pawn p)
        {
            if (p.equipment?.bondedWeapon == null || p.equipment.bondedWeapon.Destroyed)
            {
                return ThoughtState.Inactive;
            }
            CompBladelinkWeapon compBladelinkWeapon = p.equipment.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
            if (compBladelinkWeapon == null)
            {
                return ThoughtState.Inactive;
            }
            return true;
        }

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (!baseCurrentStateInternal(p).Active)
            {
                return ThoughtState.Inactive;
            }

            List<WeaponTraitDef> traitsListForReading;
            CompBladelinkWeapon comp = p.equipment.bondedWeapon.TryGetComp<CompBladelinkWeapon>();

            if (comp is CompBladelinkWeapon_SpecificWeaponTraits)
            {
                traitsListForReading = ((CompBladelinkWeapon_SpecificWeaponTraits)comp).TraitsListForReading;
            }
            else
            {
                traitsListForReading = comp.TraitsListForReading;
            }
            //List<WeaponTraitDef> traitsListForReading = p.equipment.bondedWeapon.TryGetComp<CompBladelinkWeapon>().TraitsListForReading;
            for (int i = 0; i < traitsListForReading.Count; i++)
            {
                if (traitsListForReading[i].bondedThought == def)
                {
                    return true;
                }
            }
            return ThoughtState.Inactive;
        }
    }
}
