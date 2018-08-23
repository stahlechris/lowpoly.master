using UnityEngine;
using System;

namespace LowPoly.Character
{
    public class SelfHeal_Behavior : SpecialAbility_Behavior
    {
        HealthSystem playersHealthSystem;

        void Start()
        {
            playersHealthSystem = GetComponent<HealthSystem>();
        }

        public override void Use(GameObject target)
        {
            //var playersHealthSystem = player.GetComponent<HealthSystem>();
            int healthToRestore = (config as SelfHeal_Configuration).GetHealthToRestoreToSelf();

            playersHealthSystem.ReceiveHealing(healthToRestore);
            PlaySpecialAbilitySound();
            PlayParticleFX();
            PlayAbilityAnimation();
        }
    }
}
