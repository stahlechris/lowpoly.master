using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowPoly.Character
{
    [CreateAssetMenu(menuName = ("LowPoly/Special Ability/Power Attack"))] //RIGHT CLICK MENU TO MAKE THIS THING
    public class PowerAttack_Configuration : SpecialAbility_Configuration
    {
        [Header("Special Ability Smash")] //allows us in the inspector to group things by section
        [SerializeField] int extraDamage = 5;

        public override SpecialAbility_Behavior GetBehaviorComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<PowerAttack_Behavior>();
        }

        public int GetExtraDamage()
        {
            Debug.Log("This ability applied " + extraDamage + " extra damage");
            return extraDamage;
        }
    }
}