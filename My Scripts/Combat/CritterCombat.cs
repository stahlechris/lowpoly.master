using System.Collections;
using UnityEngine;

//critters have a seperate dumbed down life system because they are one shottable
//todo consider performance with critters being grouped as a Character

namespace LowPoly.Character
{
    public class CritterCombat : MonoBehaviour
    {
        CritterAI m_CritterAI;
        public bool initiatedDeadSequence = false;
        public bool alive = true;
        //public bool isLooted = false;
        [SerializeField] GameObject m_loot = null;

        void Start()
        {
            m_CritterAI = GetComponent<CritterAI>();
        }

        //critters are one shotted
        private void OnTriggerEnter(Collider other)
        {//but immune to spell damage apparently LOL
            InventoryItemBase item = other.GetComponent<InventoryItemBase>();
            if (item != null && item.ItemType == ItemType.Weapon)
            {
                Animator playersAnimator = other.GetComponentInParent<Animator>();
                if (playersAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    StartDeathSequence();
                    //m_CritterAI = GetComponent<CritterAI>();
                    //m_CritterAI.StopAllCoroutines();
                    //m_CritterAI.m_Agent.isStopped = true;
                    //m_CritterAI.m_Agent.enabled = false;

                    //Animator m_animator = GetComponent<Animator>();
                    //m_animator.SetTrigger("die");

                    //Destroy(GetComponent<Rigidbody>());

                    //Invoke("DropLoot", 1);
                    //isDead = true;
                }
            }
        }

        //void DropLoot()
        //{

        //    Die();
        //    Destroy(GetComponent<CapsuleCollider>());
        //    transform.Find("sheep_mesh").GetComponent<SkinnedMeshRenderer>().enabled = false;
        //}
        //void Die()
        //{
        //    AudioSource[] deathSound = transform.Find("Loot").GetComponentsInChildren<AudioSource>();
        //    ParticleSystem[] fx = GetComponentsInChildren<ParticleSystem>();
        //    fx[0].Play();
        //    deathSound[0].Play();
        //    deathSound[1].Play();
        //    StartCoroutine(LootAppears(deathSound[2], fx[2]));

        //}
        //IEnumerator LootAppears(AudioSource itemAppears, ParticleSystem itemCloud)
        //{
        //    yield return new WaitForSeconds(4.1f);
        //    foreach (GameObject item in loot)
        //    {
        //        item.SetActive(true);
        //    }
        //    itemAppears.Play();
        //    itemCloud.Play();
        //    DestroyAll();
        //}

        //void DestroyAll()
        //{
        //    Destroy(this.gameObject,30); //30 seconds to loot or gone forever, bitch
        //}

        public void StartDeathSequence()
        {
            alive = false;
            if (!initiatedDeadSequence)
            {
                initiatedDeadSequence = true;
                PlayDeathAnimation();
                DisableAllBehaviors();
                ActivateLootItems();
                DisableAndDestroyAllComponents();
                PlayDeathFX();
            }
            Debug.Log("you already started the death sequence");
        }
        private void PlayDeathAnimation()
        {
            Animator m_animator = GetComponent<Animator>();
            m_animator.SetTrigger("die");
        }
        private void DisableAllBehaviors()
        {
            m_CritterAI = GetComponent<CritterAI>();
            m_CritterAI.doWander = false;
            m_CritterAI.StopAllCoroutines();
            m_CritterAI.m_Agent.isStopped = true;
            m_CritterAI.m_Agent.enabled = false;
        }
        private void DisableAndDestroyAllComponents()
        {
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<Collider>());
            transform.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }

        private void PlayDeathFX()
        {
            AudioSource[] deathSound = transform.Find("Loot").GetComponentsInChildren<AudioSource>();
            ParticleSystem[] fx = GetComponentsInChildren<ParticleSystem>();
            fx[0].Play();
            deathSound[0].Play();
            deathSound[1].Play();
            StartCoroutine(LootAppears(deathSound[2], fx[2]));
        }
        IEnumerator LootAppears(AudioSource itemAppears, ParticleSystem itemCloud)
        {
            yield return new WaitForSeconds(4.1f);
            itemAppears.Play();
            itemCloud.Play();
            DestroyAll();
        }
        void ActivateLootItems()
        {
            //we cant find inactivate objects...soooo this is good code, but bad Unity
            if (m_loot != null)
            {   //Transform implements numerable NOT gameobject...this is stupid
                m_loot.SetActive(true);
                foreach (Transform item in m_loot.transform)
                {
                    item.gameObject.SetActive(true);
                    foreach (Behaviour component in item.GetComponentsInChildren<Behaviour>())
                    {
                        component.enabled = true;
                    }
                }
            }
            else
            {
                //didn't have any loot
                //play a womp womp woOoommmp sound fx because you fucking suck
            }
        }
        void DestroyAll()
        {//sooooo you have 30 seconds to loot it or tough luck lol..prob fix this?
            Destroy(this.gameObject, 30);
        }
    }
}
