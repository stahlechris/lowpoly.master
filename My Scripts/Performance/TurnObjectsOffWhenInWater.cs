using UnityEngine;


/*This script is attached to an object with a trigger collider.
 * The player enters the collider and a method is ran.
 * 
 * This script disables objects / components that do not 
 * need to be on.
 * 
 * After X has been achieved, this script 
 * enables the objects / components. 
 * 
 * In this case, X is a HelpFul fish who... 
 * falsifies Activated once interacted with.
 * Thus letting this behavior execute enabling.
 */ 

public class TurnObjectsOffWhenInWater : MonoBehaviour 
{
    //A control mechanism used to only initiate a behavior once.
    bool Activated { get; set; }

    #region Costly Components Here
    public GameObject sheep;
    #endregion

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Entered");
        //no other trigger but the player, no need to check if player entered.
        if(!Activated)
        {
            Activated = true;
            DeactivateComponents();
        }
    }

    //The helpful fish is the only one who calls this.
    public void ActivateComponents()
    {
        sheep.SetActive(true);
    }
    void DeactivateComponents()
    {
        sheep.SetActive(false);
    }
}
