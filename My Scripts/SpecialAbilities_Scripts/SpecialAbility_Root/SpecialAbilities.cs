using UnityEngine;
using UnityEngine.UI;

/*SpecialAbilties is responsible for attaching special abilities at runtime on the player.
 * SpecialAbilities is responsible for modifying player's mana.
 * SpecialAbilities is responsible for updating the UI manabar.
 * SpecialAbilities is an interface into casting special abilities
 * 
 */
namespace LowPoly.Character
{
    public class SpecialAbilities : MonoBehaviour
    {
        [SerializeField] SpecialAbility_Configuration[] m_SpecialAbilities;
        [SerializeField] Image m_ImageManaBar;
        [SerializeField] Text m_TextManaPercentage;
        [SerializeField] AudioSource audioSourceForAbilities;
        [SerializeField] AudioClip m_OutOfEnergySound;
        [SerializeField] AudioClip[] specialAbilitySounds;
        [SerializeField] float m_MaxMana = 50;
        [SerializeField] float m_CurrentMana = 1f;
        [SerializeField] float regenAmountPerSec = 1f;

        void Start()
        {
            m_CurrentMana = m_MaxMana;
            AttachInitialAbilities();
            UpdateManaBar();
            audioSourceForAbilities = GetComponent<AudioSource>();

        }

        private void Update()
        {
            if (m_CurrentMana < m_MaxMana)
            {
                RegenerateManaOverTime();
                UpdateManaBar();
            }
        }

        private void AttachInitialAbilities()
        {
            for (int i = 0; i < m_SpecialAbilities.Length; i++)
            {
                m_SpecialAbilities[i].AttachSpecialAbilityTo(gameObject);
            }
        }
        public int GetNumberOfSpecialAbilities()
        {
            return m_SpecialAbilities.Length;
        }

        //consider MathF call every update...and consider moving similiar health/mana logic into the HUD
        void UpdateManaBar()
        {
            //float xValue = -(ManaAsPercentage() / 2f - 0.5f);
            m_TextManaPercentage.text = string.Format("{0} %", Mathf.RoundToInt(ManaAsPercentage * 100));
            m_ImageManaBar.fillAmount = ManaAsPercentage;
        }

        public float ManaAsPercentage
        {
            get
            {
                return m_CurrentMana / (float)m_MaxMana;
            }

        }

        public void RegenerateManaOverTime()
        {
            var manaToAdd = regenAmountPerSec * Time.deltaTime;
            m_CurrentMana = Mathf.Clamp(m_CurrentMana + manaToAdd, 0, m_MaxMana); //clamp so it can't go above or below
        }

        public void ConsumeMana(float amountToConsume)
        {
            float updatedMana = (m_CurrentMana - amountToConsume);
            m_CurrentMana = Mathf.Clamp(updatedMana, 0, m_MaxMana); //clamp so it can't go above or below
            UpdateManaBar();
            RegenerateManaOverTime();
        }


        //on player aoe spells and self casts use this
        public void AttemptSpecialAbility(int i, GameObject target)
        {
            var manaCost = m_SpecialAbilities[i].GetManaCost();

            if (manaCost <= m_CurrentMana && target != null)
            {
                ConsumeMana(manaCost);

                m_SpecialAbilities[i].Use(target);
            }
            else
            {
                if (!audioSourceForAbilities.isPlaying)
                {
                    audioSourceForAbilities.PlayOneShot(m_OutOfEnergySound);
                }
            }
        }

        /*
        //spells against a target enemy use this
        public void AttemptSpecialAbility(int specialAbilityIndex, EnemyAI target)
        {
            var manaCost = m_SpecialAbilities[specialAbilityIndex].GetManaCost();

            if (manaCost <= currentMana && target != null)
            {
                ConsumeMana(manaCost);
                //what target to deal dmg to, AOE dmg + our current weapon damage applied
                //var abilityParams = new AbilityUseParams(target, weaponInUse.GetMeleeDamage());

                //specialAbilities[specialAbilityIndex].Use(target);
                //animator.SetBool("SmashActivated", true);
                //TODO Figure a way for the special ability to take priority over the autoAttack,
                //right now they're happening
            }
        }
        */
    }
}
