using UnityEngine;
using UnityEngine.AI;
public class NPC_AI : MonoBehaviour
{
    NavMeshAgent m_NavMeshAgent;
    bool isHome;
    void Start()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }
    public void GoHome()
    {
        m_NavMeshAgent.SetDestination(new Vector3(312, 31, 187));
    }
    public void GoToTent()
    {
        m_NavMeshAgent.SetDestination(new Vector3(305, 30, 180));
        //just use a coroutine instead of a collider to check if he made it
        //it takes like 2 sec for him to get there
    }

}
 