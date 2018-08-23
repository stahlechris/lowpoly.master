using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*CritterAI gives a random wandering around behavior to objects
 */
namespace LowPoly.Character
{
    public class CritterAI : MonoBehaviour
    {
        Animator m_animator;
        public NavMeshAgent m_Agent;
        int layerMask = -1;
        public GameObject player;
        public float runAwayDistance = 4f;
        public float wanderRadius = 20f;
        public float wanderTimer = 3f;
        private float timer;
        public bool isWanderingAround = true;
        [SerializeField] AudioClip[] animalSounds;
        //AudioClip runAwaySound;
        AudioSource m_audioSource;
        public bool doWander = true;

        private void OnEnable()
        {
            timer = wanderTimer;
        }
        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            m_audioSource = GetComponent<AudioSource>();
            m_Agent = GetComponent<NavMeshAgent>(); 
            m_animator = GetComponent<Animator>();
        }

        private bool IsNavMeshMoving
        {
            get
            {
                return m_Agent.velocity.magnitude > 0.1f;
            }
        }

        void Update()
        {
            if (doWander)
            {
                if (IsNavMeshMoving)
                {
                    m_animator.SetBool("walk", true);
                }
                else
                {
                    m_animator.SetBool("walk", false);
                }
                timer += Time.deltaTime;
                //Calculate our distance from the player every frame
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < runAwayDistance)
                {
                    isWanderingAround = false;
                    Vector3 dirToPlayer = transform.position - player.transform.position;
                    Vector3 newPositionToRunTo = transform.position + dirToPlayer;

                    //when we disable this
                    m_Agent.SetDestination(newPositionToRunTo);
                    m_animator.SetBool("walk", true);
                }
                else
                {
                    isWanderingAround = true;
                }
                if (isWanderingAround && timer >= wanderTimer)
                {
                    //Debug.Log("time.deltaTime timer is greater than or equal to wander timer");
                    WanderAround(transform.position);
                    timer = 0;
                }
            }
        }

        public void WanderAround(Vector3 origin)
        {
            NavMeshHit navMeshHit;
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += origin;
            MakeAnimalNoises();
            NavMesh.SamplePosition(randomDirection, out navMeshHit, wanderRadius, layerMask);

            m_Agent.SetDestination(navMeshHit.position);

        }

        public void MakeAnimalNoises()
        {
            StartCoroutine(MakeNoises());
        }

        IEnumerator MakeNoises()
        {
            float randomSecToWait = Random.Range(5, 10);

            yield return new WaitForSeconds(randomSecToWait);

            m_audioSource.clip = animalSounds[Random.Range(0, animalSounds.Length)];
            m_audioSource.volume = 0.15f;
            m_audioSource.Play();
        }
    }
}

//sheep is rvineyard run away sound