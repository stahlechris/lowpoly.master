using System.Collections;
using UnityEngine;

public class OfferingRock : MonoBehaviour 
{
    [SerializeField]GameObject temp;
    public void ChangeUI()
    {//TODO change the riddle
        temp.SetActive(false);
    }

}
