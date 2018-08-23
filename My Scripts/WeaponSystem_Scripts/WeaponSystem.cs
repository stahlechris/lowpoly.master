using UnityEngine;
using System.Collections;
using LowPoly.Character;
using UnityStandardAssets.CrossPlatformInput;

//todo...seperate weaponsystem and combat system...fuck me i knew it
//bc all characters that can fight should use the same fighting script
namespace LowPoly.Weapon
{
    public class WeaponSystem : MonoBehaviour
    {
        #region CONST FINALS
        const string DEFAULT_ATTACK = "DEFAULT_ATTACK";
        const string HAS_TARGET = "HasTarget";
        const string ATTACK = "Attack";
        const string IS_ATTACKING = "IsAttacking";
        #endregion
        [SerializeField] Weapon m_WeaponInUse;
        PlayerController player;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        Animator animator;
        GameObject currentTarget;
        bool hasTarget = false;
        public bool isCurrentlyAttacking = false;
        //float attackCooldown = 1f;
        private void Start()
        {
            player = GetComponentInParent<PlayerController>();
            if (m_WeaponInUse != null)
            {
                OverrideAnimatorController();
            }
        }
        public GameObject GetCurrentTarget()
        {
            return currentTarget;
        }
        public Weapon GetCurrentWeapon()
        {
            return m_WeaponInUse;
        }
        //this gets called from Weapon, when you pickup a weapon
        public void SetCurrentWeapon(Weapon weaponToBeSet)
        {
            m_WeaponInUse = weaponToBeSet;
        }
        private void Update()
        {   //TODO: figure how to implement attackCooldown...animation is fucked when i tried last time
            //attackCooldown -= Time.time;
            if (CrossPlatformInputManager.GetButtonDown(ATTACK))
            {
                if (player.IsArmed && Global.COMBAT_INPUT_ENABLED)
                {
                    OverrideAnimatorController();//player could cast a spell which requires an override.
                    animator.SetTrigger(ATTACK);
                    //attackCooldown = m_WeaponInUse.GetAttackSpeed();
                }
            }
        }


        //todo consider architecture vs performance...
        //(arch - go to the player to get the override controller)
        //perf - might be less cost to only store animator controller
        //i really have know way of knowing whats better
        public void OverrideAnimatorController()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            //search for the "DEFAULT_ATTACK" and replace it with the weapon in use's animation clip
            animatorOverrideController[DEFAULT_ATTACK] = m_WeaponInUse.GetAttackAnimationClip();
            /*this way weapons can have their own attack Animations
            //prepare better for future with this method (tank, berserk, etc...)
            //future I will be better at animators..blend trees whatever those are
            */
        }



        public void TestAttackTargetOnce(GameObject target, int meleeDamage)
        {
            if (!isCurrentlyAttacking)
            {
                isCurrentlyAttacking = true;
                animator.SetBool(HAS_TARGET, true); //puts us into the combat idle animation after attacking 
                currentTarget = target;
                //roll for a crit
                int totalDamageCausedThisHit = CalculateCriticalDamage(meleeDamage);
                OverrideAnimatorController(); //need to call again bc if a spell was cast in between
                if (totalDamageCausedThisHit == 0)
                {
                    Debug.Log("I missed!");
                }

                //wait to apply damage with a slight delay for animation
                StartCoroutine(WaitDealDamageAndCooldown(totalDamageCausedThisHit));
            }
        }

        IEnumerator WaitDealDamageAndCooldown(int totalDamageCausedThisHit)
        {
            float animationDelay = m_WeaponInUse.GetAnimationDelay();
            //a weapon with 0.800 length will need approx 0 sec delay before calling TakeDamage() so it does damage when weapon looks like it hits;
            if (animationDelay > 0)
            {
                yield return new WaitForSeconds(animationDelay);
            }
            currentTarget.GetComponent<HealthSystem>().TakeDamage(totalDamageCausedThisHit);
            Debug.Log("I just did " + totalDamageCausedThisHit + "damage to " + currentTarget);
            isCurrentlyAttacking= false;
            if (currentTarget.GetComponent<HealthSystem>().GetCurrentHP() < 1 || currentTarget == null)
            {
                hasTarget = false;
                Debug.Log("Target killed!");
                animator.SetBool(HAS_TARGET, false);
                StopAllCoroutines();
            }
        }

        private int CalculateCriticalDamage(int meleeDamage)
        {
            int additionalDamage = 1; //always needs to be 1 so the crit doesn't take away damage
            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= m_WeaponInUse.GetCriticalHitChance();
            //roll between 0.000000 - 1
            //if critChance is 0.1, you've got a 10% chance a number picked randomly between 0 and 1... 
            //will be less than or equal to 1
            if (isCriticalHit && DetermineIfBehindTarget(currentTarget))
            {
                Debug.Log("CRITICAL BACKSTAB!");
                float criticalBackstabMultiplier = 0.25f;
                additionalDamage = meleeDamage * Mathf.RoundToInt(criticalBackstabMultiplier + m_WeaponInUse.GetCriticalMultiplier());
                return additionalDamage;
            }
            else if (isCriticalHit)
            {
                Debug.Log("CRITICAL!");
                additionalDamage = meleeDamage * Mathf.RoundToInt(m_WeaponInUse.GetCriticalMultiplier());
                return additionalDamage;
            }

            else
                return meleeDamage;
        }
        public bool DetermineIfCanAttack(GameObject target, float meleeAttackRadius)
        {
            if (DetermineIfInMeleeAttackRange(target, meleeAttackRadius) && !DetermineIfFacingTarget(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DetermineIfInMeleeAttackRange(GameObject target, float meleeAttackRadius)
        {
            float distanceToTarget = Vector3.Distance(target.transform.position, this.transform.position);

            //Debug.Log("You need to be " + (distanceToTarget - meleeAttackRadius) + "meters closer");
            return (distanceToTarget <= meleeAttackRadius);


        }
        public bool DetermineIfFacingTarget(GameObject target)
        {
            Vector3 targetDirection = target.transform.position - this.transform.position;
            Vector3 playerforward = this.transform.forward;
            float angle = Vector3.Angle(targetDirection, playerforward);
            //Debug.Log("Angle of direction is " + angle + "You need to be under 60.0 to attack");
            return (angle > 60f);

        }
        private bool DetermineIfBehindTarget(GameObject target)
        {
            Vector3 toTarget = (target.transform.position - transform.position).normalized;
            bool isBehindEnemy;
            if (Vector3.Dot(toTarget, target.transform.forward) < 0)
            {
                isBehindEnemy = false;
            }
            else
            {
                isBehindEnemy = true; //crit increased, ignore armor
            }
            return isBehindEnemy;
        }
    }
}