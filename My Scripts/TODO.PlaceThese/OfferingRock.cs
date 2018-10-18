using UnityEngine;

public class OfferingRock : MonoBehaviour 
{
    [SerializeField]GameObject temp;
    public void ChangeUI()
    {//TODO create and have this class change the UI to reflect the next riddle
        temp.SetActive(false);
    }

}
