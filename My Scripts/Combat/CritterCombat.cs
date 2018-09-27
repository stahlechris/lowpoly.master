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
        private void OnTriggerEnter(Collider other) //THIS NULL REF'S LIKE CRAZY 
        {//but immune to spell damage apparently LOL
            //TODO IF( OTHER IS A WEAPON..OR JUST RETHINK THIS. ITS ALL SLOPPY
            InventoryItemBase item = other.GetComponent<InventoryItemBase>();
            if (item != null && item.ItemType == ItemType.Weapon)
            {
                Animator playersAnimator = other.GetComponentInParent<Animator>();   //THIS ASSUMES THE WEAPON IS ON A PLAYER!! NULL REF
                if (playersAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    StartDeathSequence();
                }
            }
        }

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
            yield return new WaitForSeconds(4.5f);
            itemAppears.Play();
            itemCloud.Play();
            //this assumes each critter only has 1 item to drop & meshRenderer & collider is disabled to start
            GetComponentInChildren<MeshRenderer>().enabled = true;
            GetComponentInChildren<Collider>().enabled = true;

            DestroyAll();
        }
        void ActivateLootItems()
        {
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
