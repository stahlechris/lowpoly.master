/* NEW WAY OF DOING COMBAT SYSTEM?
using UnityEngine;
using LowPoly.Weapon;

namespace LowPoly.Character
{
    public class CharacterCombat : MonoBehaviour
    {
        CharacterStats my_Stats;
        HealthSystem my_HealthSystem;
        WeaponSystem my_WeaponSystem;
        private void Start()
        {
            my_Stats = GetComponent<CharacterStats>();
            my_HealthSystem = GetComponent<HealthSystem>();
            my_WeaponSystem = GetComponent<WeaponSystem>();
        }

        public void Attack(WeaponSystem targetWeaponSystem, CharacterStats targetStats, HealthSystem targetHealthSystem)
        {
            //weaponSystem does the damage
            //stats modify the damage
            //health system takes the damage received and updates the UI 
        }
    }
}
*/