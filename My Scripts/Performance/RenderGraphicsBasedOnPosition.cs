using UnityEngine;
using System.Collections;

/* This class always keeps an eye on the player's forward angle.
 * It is an add-on class to help with Occlusion Culling.
 * Keep references to CPU/GPU intensive GameObjects so we can shut them off.
 */
public class RenderGraphicsBasedOnPosition : MonoBehaviour
{
    //The player's CameraArm is the target
    public Transform target; //object A
    //Control mechanism to start/stop this behavior
    public bool doBehavior = true;
    //Drag GameObjects you wish to occlude here.
    public GameObject[] occludees;
    //Define the angle at which to activate objects
    public float fieldOfView; //from -1 to 1 Where 1 is target looking right at me, .9 mostly at me...-1 away
    //Are the objects already on?
    internal bool objectsOn = true;
    //Has 1 collider entered the trigger
    internal bool hasEntered;

    void Update()
    {
        if (doBehavior)
        {
            Vector3 dirFromAtoB = (transform.position - target.position).normalized;
            float dot = Vector3.Dot(dirFromAtoB, target.forward);
            if (dot < fieldOfView) //Player's camera is NOT looking at me enough
            {
                if (objectsOn)
                {
                    Debug.Log(dot + " angle is under " + fieldOfView + " turned off objects");
                    ActivateGameObjects(false);
                }
            }
            else //Player's camera is mostly looking at me
            {
                if (!objectsOn)
                {
                    Debug.Log(dot + " angle is above " + fieldOfView + " turned on objects");
                    ActivateGameObjects(true);
                }
            }
        }
    }
    void ActivateGameObjects(bool active)
    {
        if (!active)
        {
            foreach (GameObject go in occludees)
            {
                go.SetActive(false);
            }
            objectsOn = false;
        }
        else
        {
            foreach (GameObject go in occludees)
            {
                go.SetActive(true);
            }
            objectsOn = true;
        }
    }
}
