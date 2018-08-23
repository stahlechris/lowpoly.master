using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //don't really understand this yet. I thought law of demeter says dots r bad
    //with this, each class needs to dot like 3 times into this class ?
    #region Singleton
    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion
    public GameObject player;
}
