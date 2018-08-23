using System.Collections;
using UnityEngine;

/*I TURNED OFF PROJECT SETTINGS -> FANTASTIC AND SWITCHED TO BEAUTIFUL 
 * IT GAVE ME BACK ABOUT 90FPS (F=FIGURE OUT WTF THIS IS)
 */
namespace LowPoly.Character
{
    public abstract class SpecialAbility_Behavior : MonoBehaviour
    {
        protected SpecialAbility_Configuration config; //now the kids can see the config...they grow up so fast

        const float PARTICLE_DESTROY_DELAY = 2.5f;

        public abstract void Use(GameObject target); //decided on abstract over virtual bc each is different
        //were not defining how this works here, the kids can have their own way of doing this

        public void SetConfig(SpecialAbility_Configuration config)
        {
            this.config = config;
        }

        protected void PlayParticleFX()
        {
            var particlePrefab = config.GetParticlePrefab();
            var particleObject = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);

            particleObject.transform.parent = transform;
            particleObject.GetComponentInChildren<ParticleSystem>().Play();

            StartCoroutine(DestroyParticleWhenFinished(particleObject));

        }

        protected void PlaySpecialAbilitySound()
        {
            AudioClip specialAbilitySound = config.GetAudioClip(); //TODO get random clip. 1 each ability
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(specialAbilitySound); //TODO. each audio clip conforms to X seconds
        }

        protected void PlayAbilityAnimation()
        {   //todo make this work
            var animator = GetComponent<Animator>();
            var animatorOverrideController = GetComponent<PlayerController>().GetAnimatorOverrideController();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT_ATTACK"] = config.GetAbilityAnimationClip();
            animator.SetTrigger("Attack");
        }

        IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
        {
            while (particlePrefab.GetComponentInChildren<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_DESTROY_DELAY);
            }
            Destroy(particlePrefab);
            yield return new WaitForEndOfFrame();
        }

    }
}