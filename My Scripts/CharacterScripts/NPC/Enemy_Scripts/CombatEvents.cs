
using UnityEngine;

public class CombatEvents : MonoBehaviour 
{
    public delegate void EventHandler<EnemyEventArgs>(GameObject enemyGameObject);
    public event EventHandler<EnemyEventArgs> OnEnemyKilled;

    public void FireAnEvent_OnEnemyKilled(GameObject enemyGameObject)
    {
        if(OnEnemyKilled != null)
        {
            OnEnemyKilled(enemyGameObject);
        }
    }
}
