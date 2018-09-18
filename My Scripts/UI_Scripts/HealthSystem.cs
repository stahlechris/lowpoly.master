using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * HealthSystem is responsible for adding and subtracting health from characters.
 * It also updates the HealthBar on the UI. 
 * 
 */
namespace LowPoly.Character //only characters have health
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] CharacterStats m_stats;
        //[SerializeField] EnemyCombat enemyCombat;
        [SerializeField] Image m_ImageHealthBar;
        [SerializeField] Text m_TextHealthPercentage;

        [SerializeField] Image m_ImageHealthBarHUD;
        [SerializeField] Text m_TextHealthPercentageHUD;

        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;
        [SerializeField] Animator m_Animator;
        //[SerializeField] AudioSource m_AudioSource;
        [SerializeField] int m_MaxHP = 100;
        [SerializeField] int m_CurrentHP = 1;
        const float DEATH_VANISH_SECONDS = 2f;
        bool alive = true;

        #region CONST STRING VARIABLES
        const string SETUP_HEALTH_SYSTEM = "SetupHealthSystem";
        const string PLAYER_NAME = "Liam";
        const string HEALTH_SYSTEM_PATH = "UI_Socket/EnemyCanvas(Clone)/EnemyHealthBar/GreenForeground";
        const string CRITTER = "Critter";
        const string ATTACKABLE = "Attackable";
        #endregion

        void Awake()
        {
            m_CurrentHP = m_MaxHP;
        }
        void Start()
        {
            m_stats = GetComponent<CharacterStats>();
            m_Animator = GetComponent<Animator>(); //animate dying
            //enemyCombat = GetComponent<EnemyCombat>();
            //m_AudioSource = GetComponent<AudioSource>(); //death & hit noises
            Invoke(SETUP_HEALTH_SYSTEM, 1.5f);
        }
        void LateUpdate()//change this later to Update when/if its a problem
        {//durr im a computer, i have to check if im alive before i do anything
            if(m_CurrentHP < 1 && alive)
            {
                alive = false;
                StartCoroutine(KillCharacter());
            }
        }
        void SetupHealthSystem()
        {
            if (this.transform.CompareTag(CRITTER) || this.transform.CompareTag(ATTACKABLE))
            {
                m_ImageHealthBar = transform.Find(HEALTH_SYSTEM_PATH).GetComponent<Image>();
                m_TextHealthPercentage = GetComponentInChildren<Text>();
                //!Drag in Enemies' health and text for the hud
            }
        }

        void UpdateHealthBar()
        {//todo update HUD ui too
            //{0} gets rid of the leading 0.....need *100 to work
            m_TextHealthPercentage.text = string.Format("{0} %",Mathf.RoundToInt(HealthAsPercentage * 100));
            m_ImageHealthBar.fillAmount = HealthAsPercentage; //slides the little thingy on inspector
            //non Liam's will have their HP above them AND on the HUD
            if(this.name != PLAYER_NAME)
            {
                try
                {
                    m_TextHealthPercentageHUD.text = string.Format("{0} %", Mathf.RoundToInt(HealthAsPercentage * 100));
                    m_ImageHealthBarHUD.fillAmount = HealthAsPercentage; //slides the little thingy on inspector
                }
                catch(NullReferenceException e)
                {
                    Debug.Log(e + "couldn't update HUD probably bc i never appeared there (one shotted");
                }
            }
        }

        public float HealthAsPercentage
        {
            get
            {
                return m_CurrentHP / (float)m_MaxHP; //cast bc / is integer division
            }
        }

        public void ReceiveHealing(int amountToHeal)
        {
            m_CurrentHP = Mathf.RoundToInt(Mathf.Clamp(m_CurrentHP + amountToHeal, 0f, m_MaxHP));
            UpdateHealthBar();
        }

        public void TakeDamage(int damage)
        {
            ApplyStatsToIncomingDamage(damage);
            bool characterDies = (m_CurrentHP - damage <= 0);
            /*clamp the (currentHp - damge) between 0 and maxHp
             * m_CurrentHP = Mathf.Clamp(m_CurrentHP - damage, 0, m_MaxHP);?
            */

            m_CurrentHP -= damage;
            UpdateHealthBar();
            //var clip = damaSounds[UnityEngine.Random.Range(0, damageSounds.length)];
            //if(damageSounds != null) if no sound then the coroutine doesnt work bc it drops out
                //m_AudioSource.PlayOneShot(damageSounds[0]);
            if (characterDies)
            {
                StartCoroutine(KillCharacter());
            }
        }

        int ApplyStatsToIncomingDamage(int damage)
        {//we don't want greater armor than damage to give us health
            damage -= m_stats.armor.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);
                
            return damage;
        }
        public int GetCurrentHP()
        {
            return m_CurrentHP;
        }
        IEnumerator KillCharacter() // a character is a player is an enemy is a...
        {
            var playerComponent = GetComponent<PlayerController>(); //else null
            if (playerComponent && playerComponent.isActiveAndEnabled)
            {
                m_Animator.Play("Die");
                //Debug.Log("dead" + playerComponent);
                //audioSource.clip = deathSounds[Random.Range(0, deathSounds.Length)];
                //audioSource.Play();
                yield return new WaitForSeconds(2f);//audiosource.clip.length instead of 2
                Debug.Log("attempting to load scene");
                SceneManager.LoadScene(0);
            }
            else //it's an enemy
            {
                transform.GetComponent<EnemyCombat>().StartDeathSequence();
            }
        }
    }
}