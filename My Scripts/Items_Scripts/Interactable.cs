using UnityEngine;

public class Interactable : MonoBehaviour 
{
    //PlayerController gets a QuestGiver component, which inherits from Interactable
    public virtual void Interact()
    {
        Debug.Log("Interacting with base class, Interactable");
    }
    /* If a class calls GetComponenet<Interactable>().Interact...
     * And there are two classes that have overridden this method...
     * Unity will choose the class that is above the other in the inspector
     */
}
