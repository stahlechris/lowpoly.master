
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour 
{
    //[SerializeField] int my_MeleeAttackRange = 2; //does an enemy get a weapon system too? provide them an invisible weapon?
    [SerializeField] int my_AggroRadius = 10;
    Transform my_Target;
    Animator my_Animator;
    NavMeshAgent my_NavMeshAgent;
    [SerializeField]Vector3 my_positionBeforeCombat;
    const string WALK_ANIMATION = "walk";
    //bool amChasingTarget = false;
    //WARNING: below variable is being poked every update frame, he no happy.
    float my_distanceToTarget;
    bool amAttacking = false;
    EnemyCombat my_EnemyCombat;
    bool amAtStartingPosition = true;
    Vector3 startingPosition = new Vector3(307, 31, 207.5f);
    public bool behaviorEnabled = true;
    private void Start()
    {
        my_EnemyCombat = GetComponent<EnemyCombat>();
        my_Target = PlayerManager.instance.player.transform;
        my_NavMeshAgent = GetComponent<NavMeshAgent>();
        my_Animator = GetComponent<Animator>();
        my_positionBeforeCombat = transform.position; //will be used to walk back after player has outran enemy
    }
    private void LateUpdate()
    {
        if (behaviorEnabled)
        {
            my_distanceToTarget = Vector3.Distance(my_Target.position, transform.position);
            CheckIfIHaveBeenAgrrod();
            FaceTargetWhileAggrod();
        }
    }
    void CheckIfIHaveBeenAgrrod()
    {
        if (my_distanceToTarget < my_AggroRadius)
        {
            ChaseTarget();
        }
        else
        {
            //if we have gone away from where we started, go back
            if(!amAtStartingPosition)
            {
                my_Animator.SetBool(WALK_ANIMATION, true);
                my_NavMeshAgent.SetDestination(startingPosition);
                amAtStartingPosition = true;
            }
        }
    }
    void GoBackToStartingPosition()
    {
        my_Animator.SetBool(WALK_ANIMATION, true);
        my_NavMeshAgent.SetDestination(my_Target.position);
    }
    void ChaseTarget()
    {
        amAtStartingPosition = false;
        my_Animator.SetBool(WALK_ANIMATION, true);
        my_NavMeshAgent.SetDestination(my_Target.position);
    }
    void FaceTargetWhileAggrod()
    {
        //if we have made it to the target...
        if(my_distanceToTarget <= my_NavMeshAgent.stoppingDistance)
        {
            my_Animator.SetBool(WALK_ANIMATION, false);
            if (!amAttacking)
            {
                amAttacking = true;
                my_EnemyCombat.TestAttackTarget(my_Target);
            }
            //reduce extra computation(?) with bool check
            if (CheckIfFacingTarget(my_Target))
            {
                //Debug.Log("Reorienting to face target...");
                Vector3 direction = (my_Target.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                //speherically interpolate.....LOL wtf does that mean??
            }
        }
    }
    public bool CheckIfFacingTarget(Transform target)
    {
        Vector3 targetDirection = target.transform.position - transform.position;
        Vector3 playerforward = transform.forward;
        float angle = Vector3.Angle(targetDirection, playerforward);
        //Debug.Log("Angle of direction is " + angle + "You need to be under 15 to re-orient");
        return (angle > 15f);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, my_AggroRadius);
    //}
}

