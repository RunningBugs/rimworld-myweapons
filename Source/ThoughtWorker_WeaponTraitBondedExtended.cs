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
            Log.Warning("Triggered: ThoughtWorker_WeaponTraitBondedExtended.CurrentStateInternal");
            if (!baseCurrentStateInternal(p).Active)
            {
                return ThoughtState.Inactive;
            }
            Log.Warning("Get through: ThoughtWorker_WeaponTraitBondedExtended.CurrentStateInternal");
            List<WeaponTraitDef> traitsListForReading;
            CompBladelinkWeapon comp = p.equipment.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
            Log.Warning($"Comp is null? {comp == null}");
            if (comp is CompBladelinkWeapon_SpecificWeaponTraits)
            {
                Log.Warning("Branch: true");
                traitsListForReading = ((CompBladelinkWeapon_SpecificWeaponTraits)comp).TraitsListForReading;
            }
            else
            {
                Log.Warning("Branch: false");
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
