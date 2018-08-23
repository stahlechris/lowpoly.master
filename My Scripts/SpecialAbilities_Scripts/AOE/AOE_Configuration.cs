using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowPoly.Character
{
    [CreateAssetMenu(menuName = ("LowPoly/Special Ability/AOE"))] //RIGHT CLICK MENU TO MAKE THIS THING
    public class AOE_Configuration : SpecialAbility_Configuration
    {
        [Header("AOE Specific")] //allows us in the inspector to group things by section
        [SerializeField] float blast_radius = 2.5f;
        [SerializeField] int damageToEachTarget = 5;

        public override SpecialAbility_Behavior GetBehaviorComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<AOE_Behavior>();
        }

        public int GetDamageToEachTarget()
        {
            return damageToEachTarget;
        }

        public float GetRadius()
        {
            return blast_radius;
        }
    }
}