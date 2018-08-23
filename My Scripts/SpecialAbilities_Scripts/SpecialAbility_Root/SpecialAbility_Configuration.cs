using UnityEngine;

//PARENT CLASS\\

namespace LowPoly.Character //abstract classes can't be instantiated, we just inherit from them 
                            //take their methods and make them our own-->ability()---inherit-->freezingRain();
{
    public abstract class SpecialAbility_Configuration : ScriptableObject
    {
        [Header("Special Ability General")] //allows us in the inspector to group things by section

        [SerializeField] float m_AbilityManaCost = 10f; //all abilities have a mana cost
        [SerializeField] GameObject m_AbilityParticleFX;  //all abilities have a sound fx
        [SerializeField] AudioClip m_AbilityAudioClip; //all abilities have a sound fx
        [SerializeField] AnimationClip m_AbilityAnimation;

        protected SpecialAbility_Behavior behavior;
        public abstract SpecialAbility_Behavior GetBehaviorComponent(GameObject objectToAttachTo);

        public void AttachSpecialAbilityTo(GameObject gameObjectToAttachTo)
        {
            SpecialAbility_Behavior behaviorComponent = GetBehaviorComponent(gameObjectToAttachTo);
            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
        }

        public void Use(GameObject target)
        {
            behavior.Use(target); //will return a null if no currentTarget 
        }

        public float GetManaCost()
        {
            return m_AbilityManaCost;
        }

        public AudioClip GetAudioClip()
        {
            return m_AbilityAudioClip;
        }

        public GameObject GetParticlePrefab()
        {
            return m_AbilityParticleFX;
        }

        public AnimationClip GetAbilityAnimationClip()
        {
            return m_AbilityAnimation;
        }
    }
}

//END PARENT CLASS\\