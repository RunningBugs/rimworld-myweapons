using RimWorld;
using System.Collections.Generic;
using Verse;

namespace MyWeapons
{
    public class CompProperties_BladelinkWeapon_SpecificWeaponTraits : CompProperties_Biocodable
    {
        public List<string> personas;

        public CompProperties_BladelinkWeapon_SpecificWeaponTraits()
        {
            compClass = typeof(CompBladelinkWeapon_SpecificWeaponTraits);
        }
    }
}
