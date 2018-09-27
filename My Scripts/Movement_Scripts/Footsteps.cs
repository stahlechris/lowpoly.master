using System.Collections;
using UnityEngine;

//TODO: figure out footsteps;
namespace LowPoly.Character
{
    public class Footsteps : MonoBehaviour
    {
        public ThirdPerson_Camera mainCamera;

        //public PlayerController m_playerController;
        //public CustomMovement m_customMovement;
        public GameObject m_Footprints;
        public GameObject m_FootStepDust;
        public AudioSource m_AudioSource;
        public AudioClip[] m_Grass;
        public AudioClip[] m_Water;
        public Rigidbody rb;
        private bool step = true;
        //float audioStepLengthWalk = 0.5f;
        float stepLengthRun = 0.23f;
        float stepLengthWater = 0.4f;
        float PARTICLE_DESTROY_DELAY = 0.35f;
        const string WATER_TAG = "Water";
        //Vector3 spawnOffset = new Vector3()
        bool HasSetCamera { get; set; }
        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody>();
        }

        private void OnCollisionStay(Collision collision)
        {
            if(step && rb.velocity.magnitude > 0.75f) //magnitude means && we moved
            {
                PlayFootstepNoise(collision.collider);
                //MakeFootstepDust(collision.collider);
            }
        }

        IEnumerator WaitForFootstep(float stepLength)
        {
            step = false;
            yield return new WaitForSeconds(stepLength);
            step = true;
        }

        private void PlayFootstepNoise(Collider other)
        {
            m_AudioSource.volume = Random.Range(0.75f, 1f);
            m_AudioSource.pitch = Random.Range(0.75f, 1f);

            if (other.CompareTag(WATER_TAG))
            {
                m_AudioSource.PlayOneShot(m_Water[0]);
                StartCoroutine(WaitForFootstep(stepLengthWater));

                if (!HasSetCamera)
                {//don't let them look underwater...yet!
                    HasSetCamera = true;
                    mainCamera.YMinLimit = 0;
                }
            }
            else
            {

                m_AudioSource.PlayOneShot(m_Grass[0]);
                MakeFootstepDust();
                StartCoroutine(WaitForFootstep(stepLengthRun));

                if (HasSetCamera)
                {//give max allowed rotation back
                    HasSetCamera = false;
                    mainCamera.YMinLimit = -20;
                }
            }

        }


        private void MakeFootstepDust()
        {
            var particlePrefab = m_FootStepDust;
            var particleObject = Instantiate(particlePrefab,transform.position,
                                             particlePrefab.transform.rotation);
            particleObject.GetComponentInChildren<ParticleSystem>().Play();

            StartCoroutine(DestroyParticleWhenFinished(particleObject));

        }

        void MakeFootstepSplash()
        {
            //TODO
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
