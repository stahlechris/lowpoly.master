using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowPoly.Character
{
    public class PowerAttack_Behavior : SpecialAbility_Behavior
    {
        public override void Use(GameObject target)
        {
            PlaySpecialAbilitySound();
            DealDamage(target);
            PlayParticleFX();
            PlayAbilityAnimation();
        }

        private void DealDamage(GameObject target)
        {
            int damageToDeal = (config as PowerAttack_Configuration).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
            Debug.Log("...for a total of  " + damageToDeal + " to " + target);
        }
    }
}