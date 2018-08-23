using UnityEngine;

/*This class lets us make a "Special Ability" in the inspector by custom menu
 * It inherits from a parent abstract class to show the customizable values;
 */
namespace LowPoly.Character
{
    [CreateAssetMenu(menuName = ("LowPoly/Special Ability/Self Heal"))] //RIGHT CLICK MENU TO MAKE THIS THING
    public class SelfHeal_Configuration : SpecialAbility_Configuration
    {
        [Header("Self Heal Specific")] //allows us in the inspector to group things by section
        [SerializeField] int hpToRestoreToSelf = 15;

        public override SpecialAbility_Behavior GetBehaviorComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<SelfHeal_Behavior>();
        }

        public int GetHealthToRestoreToSelf()
        {
            return hpToRestoreToSelf;
        }
    }
}