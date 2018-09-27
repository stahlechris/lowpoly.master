using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPC_AI : MonoBehaviour
{
    NavMeshAgent m_NavMeshAgent;
    bool isHome;
    Collider my_collider;
    Rigidbody my_rigidBody;
    JumpUpAndDown jumpBehavior;

    Animator animator;   
    public CheckIfIpsHome checkIfIpsHome;

    void Start()
    {
        jumpBehavior = GetComponent<JumpUpAndDown>();
        my_collider = GetComponent<Collider>();
        my_rigidBody = GetComponent<Rigidbody>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

    }
    public void GoHome()
    {
        jumpBehavior.enabled = false;
        checkIfIpsHome.Activate = true;
        my_collider.enabled = false;
        animator.SetBool("run", true);
        m_NavMeshAgent.SetDestination(new Vector3(312, 31, 187));
        StartCoroutine(EnableKinematicRigidBodyAndCollider());
    }
    public void GoToTent()
    {
        m_NavMeshAgent.SetDestination(new Vector3(305, 30, 180));
    }

    IEnumerator EnableKinematicRigidBodyAndCollider()
    {
        yield return new WaitForSeconds(3);
        my_rigidBody.isKinematic = false; //only non-kinematic rigidbodies can trigger OnTriggerEnter()
        my_collider.enabled = true;
    }
}
 