using System;
using UnityEngine;

public class EnemyEventArgs : EventArgs
{
    public GameObject m_EnemyItem;

    public EnemyEventArgs(GameObject enemyItem)
    {
        m_EnemyItem = enemyItem;
    }
}