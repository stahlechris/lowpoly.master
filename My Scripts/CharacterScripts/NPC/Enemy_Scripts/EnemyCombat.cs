using System;
using UnityEngine;
using LowPoly.Character;
using System.Collections;

public class EnemyCombat : MonoBehaviour
{
    //[SerializeField] PlayerController player;
    [SerializeField] GameObject m_loot = null;
    [SerializeField] GameObject UI_SOCKET;
    CritterAI m_CritterAI;
    EnemyAI m_enemyAI;
    public AudioSource m_AudioSource;
    public GameObject m_CombatAuraParticleSystem;
    Animator my_animator;
    GameObject particlePrefab;
    //public bool passiveEnemy = true; //yellow is passive, red is active...like WoW
    //bool inCombat = false;
    bool onTheHUD = false;
    bool suspensfulMusicPlaying = false;
    bool combatAuraPlaying = false;
    //bool facingAttacker = false;
    bool initiatedCombatStartup = false;
    bool initiatedDeadSequence = false;
    bool alive = true;
    void Start()
    {
        my_animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
    }
    //todo: need to figure out how to detect aoe blasts 
    void OnTriggerEnter(Collider other)
    {
        //Start combat sequence
        if (!initiatedCombatStartup)
        {
            try
            {          //todo figure out how to check for aoe blast
                Animator playersAnimator = other.transform.GetComponentInParent<Animator>();
                if ((playersAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack")))
                {
                    Debug.Log("passed checks to start combat sequence in enemy");
                    StartCombatSequence(other);
                    initiatedCombatStartup = true;
                }
            }
            catch (NullReferenceException e)
            {
                Debug.Log(this + " enemy combat script logged: " + e);
            }
        }

    }
    public void StartCombatSequence(Collider other)
    {
        if (!onTheHUD)
        {
            ShowMyselfAsTargetOnHUD();
        }
        if (!suspensfulMusicPlaying)
        {
            PlaySuspensfulMusic();
        }
        if (!combatAuraPlaying)
        {
            PlayCombatAuraFX();
        }
    }
    //this has been deprecated due to implementing "AlwaysFaceTarget()"
    //private void FaceMyAttacker(Transform target)
    //{
    //    Vector3 targetPosition = new Vector3(target.position.x -0.1f,
    //                                             this.transform.position.y,
    //                                             target.position.z);
    //    this.transform.LookAt(targetPosition);
    //    Debug.Log("now facing attacker");
    //}
    void ShowMyselfAsTargetOnHUD()
    {
        GetComponentInChildren<EnemyUI>().SetupMyUI();
        onTheHUD = true;
    }
    void PlaySuspensfulMusic()
    { //TODO: need an AudioManager empty to play all music 
        m_AudioSource.Play();
        suspensfulMusicPlaying = true;
    }
    void PlayCombatAuraFX()
    {
        combatAuraPlaying = true;
        particlePrefab = Instantiate(m_CombatAuraParticleSystem, transform.position, transform.rotation);
        particlePrefab.transform.parent = transform;
        particlePrefab.GetComponentInChildren<ParticleSystem>().Play();
    }


    public void StartDeathSequence()
    {
        alive = false;
        if (!initiatedDeadSequence)
        {
            initiatedDeadSequence = true;
            PlayDeathAnimation();
            DisableAllBehaviors();
            if (onTheHUD)
            {
                RemoveMyselfFromTheHUD();
            }
            ActivateLootItems();
            DisableAndDestroyAllComponents();
            PlayDeathFX();
        }
        Debug.Log("you already started the death sequence");
    }
    void PlayDeathAnimation()
    {
        Animator m_animator = GetComponent<Animator>();
        m_animator.SetTrigger("die");
    }
    void DisableAllBehaviors()
    {
        m_enemyAI = GetComponent<EnemyAI>();
        m_enemyAI.behaviorEnabled = false;
        m_enemyAI.enabled = false;
        StopCoroutine(TestAttackRepeatedly(currentTarget,2));
        if (this.CompareTag("Critter"))
        {
            m_CritterAI = GetComponent<CritterAI>();
            m_CritterAI.StopAllCoroutines();
            m_CritterAI.m_Agent.isStopped = true;
            m_CritterAI.m_Agent.enabled = false;
        }
    }
    void RemoveMyselfFromTheHUD()
    {
        GetComponentInChildren<EnemyUI>().RemoveMyselfFromTheHUD();
        onTheHUD = false;
    }
    void DisableAndDestroyAllComponents()
    {
        try
        {
            particlePrefab.GetComponentInChildren<ParticleSystem>().Stop();
            Destroy(particlePrefab);
        }
        catch(NullReferenceException e)
        {
            Debug.Log(e + "couldn't destroy non existant particle fx probably because you never started your combat sequence");
        }
        UI_SOCKET.SetActive(false);
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());
        transform.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
    }

    void PlayDeathFX()
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

    #region ENEMY ATTACK ROUTINE UNDER CONSTRUCTION

    bool isCurrentlyAttacking = false;
    Transform currentTarget;
    float currentAttackCooldown = 0;
    public void TestAttackTarget(Transform target)
    {
        my_animator.SetTrigger("attack");
        isCurrentlyAttacking = true;
        currentTarget = target;
        StartCoroutine(TestAttackRepeatedly(target, 2f));
    }
    IEnumerator TestAttackRepeatedly(Transform target, float attackCooldown)
    {
        float animationDelay = 0.2f;
        //a weapon with 0.800 length will need approx 0 sec delay before calling TakeDamage() so it does damage when weapon looks like it hits;
        if (animationDelay > 0)
        {
            yield return new WaitForSeconds(animationDelay);
        }
        currentAttackCooldown = attackCooldown;
        bool attackerStillAlive = GetComponent<HealthSystem>().HealthAsPercentage > 0;
        bool targetStillAlive = target.GetComponent<HealthSystem>().HealthAsPercentage > 0;

        while (attackerStillAlive && targetStillAlive && DetermineIfInMeleeAttackRange(target,2f))
        {
            //bool canAttack = DetermineIfCanAttack(target, attackRange);
            if (alive)
            {
                target.GetComponent<HealthSystem>().TakeDamage(25);
                TestAttackRepeatedly(currentTarget, currentAttackCooldown);
            }
            yield return new WaitForSeconds(attackCooldown);
        }
    }
    public bool DetermineIfInMeleeAttackRange(Transform target, float meleeAttackRadius)
    {
        float distanceToTarget = Vector3.Distance(target.position, transform.position);

        //Debug.Log("You need to be " + (distanceToTarget - meleeAttackRadius) + "meters closer");
        return (distanceToTarget <= meleeAttackRadius);
    }

#endregion


}
/*seperate out a lootmanager: 
 * - Roll for a list of objects to drop with rng
 * - Activate returned objects
 * - Activate scripts on returned objects
 */
