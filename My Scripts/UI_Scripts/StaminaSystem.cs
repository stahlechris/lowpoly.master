using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LowPoly.Character
{
    public class StaminaSystem : MonoBehaviour
    {
        [SerializeField] CustomMovement m_Character;
        private Vector3 m_Move;
        private bool m_Jump;

        [SerializeField] Image m_ImageStamina;
        [SerializeField] float m_MaxStamina = 100;
        [SerializeField] float m_CurrentStamina = 99;
        private float staminaRegenerationRate = 12f;
        private const int JUMP_COST = 5;
        private const float SPRINT_COST = 1f;
        [SerializeField] float timeItTakesToCatchMyBreath = 2f;
        public bool isCatchingBreath = false;

        public bool isSprinting;

        void Update()
        {
            if (m_CurrentStamina < m_MaxStamina)
            {
                RegenerateStaminaOverTime();
                UpdateStaminaWheel();
            }
        }

        void UpdateStaminaWheel()
        {
            m_ImageStamina.fillAmount = StaminaAsPercentage;
        }

        public float StaminaAsPercentage
        {
            get
            {
                return m_CurrentStamina / (float)m_MaxStamina;
            }
        }

        //todo make this a coroutine
        void RegenerateStaminaOverTime()
        {
            var staminaToAdd = staminaRegenerationRate * Time.deltaTime;
            m_CurrentStamina = Mathf.Clamp(m_CurrentStamina + staminaToAdd, 0, m_MaxStamina); //clamp so it can't go above or below
        }

        void ConsumeStamina(float amountToConsume)
        {
            float updatedStamina = (m_CurrentStamina - amountToConsume);
            m_CurrentStamina = Mathf.Clamp(updatedStamina, 0, m_MaxStamina);
            UpdateStaminaWheel();
            RegenerateStaminaOverTime();
        }


        //todo: refactor code to put return false first everywhere
        public bool AttemptJump()
        {
            if (JUMP_COST <= m_CurrentStamina)
            {
                ConsumeStamina(JUMP_COST);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AttemptSprint()
        {
            if (SPRINT_COST <= m_CurrentStamina)
            {
                isSprinting = true;
                ConsumeStamina(SPRINT_COST);
                return true;
            }
            else
            {
                isSprinting = false;
                m_Character.m_MoveSpeedMultiplier = 0.5f;
                m_Character.m_AnimSpeedMultiplier = 0.5f;

                StartCoroutine(CatchMyBreath());
                return false;
            }
        }

        public IEnumerator CatchMyBreath()
        {
            isSprinting = false;
            isCatchingBreath = true;
            yield return new WaitForSeconds(timeItTakesToCatchMyBreath);
            isCatchingBreath = false;
            m_Character.m_MoveSpeedMultiplier = 1f;
            m_Character.m_AnimSpeedMultiplier = 1f;


            //play panting animation that suspends movement
            //play panting sound
        }
    }
}