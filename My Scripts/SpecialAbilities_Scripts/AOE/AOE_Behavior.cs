using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowPoly.Character
{
    public class AOE_Behavior : SpecialAbility_Behavior
    {
        public override void Use(GameObject target)
        {
            PlaySpecialAbilitySound();
            DealRadialDamage();
            PlayParticleFX();
            PlayAbilityAnimation();
        }

        public void DealRadialDamage()
        {   //create a static sphere cast. Up bc not moving
            float blast_radius = (config as AOE_Configuration).GetRadius();
            //I want this sphere cast as a trigger so other objects's OnTriggerEnter can see this
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position, blast_radius,
                Vector3.up,blast_radius);

            foreach (RaycastHit hit in hits)
            {
                //Debug.Log("Aoe sphere cast hit " + hit.collider.gameObject);
                //potentially get HealthSystem from targets in radius...does making variable a var make it so you dont need try catch?
                var damageable = hit.transform.GetComponent<HealthSystem>();
                bool hitPlayer = hit.transform.GetComponent<PlayerController>();

                if (damageable != null && !hitPlayer)
                {
                    damageable.TakeDamage((config as AOE_Configuration).GetDamageToEachTarget());
                }
                else
                {
                    //Debug.Log("this hit in the spherecast hit array is either the player or null");
                }
            }
        }
    }
}