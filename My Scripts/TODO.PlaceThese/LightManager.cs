using System.Collections;
using UnityEngine;

public class LightManager : MonoBehaviour 
{
    Transform myTransform;
    //Variable to reset the timer
    float time;
    public float timer;
    public bool doBehavior = true;
    public Light[] tomeLights;
    public Transform target;
    [Tooltip("How far away target needs to be for this behavior to activate")]
    public float minDistance;//Ex - I have specifed my target needs to be > 10m away from this position
    //Are the resource-intensive objects on or off?
    internal bool objectOn = false;

    void Start()
    {
        myTransform = transform;
        time = timer;
    }
    internal void Update()
    {
        if (doBehavior)
        {
            //decrement the time by framerate independent amount
            timer -= Time.deltaTime;
            if (timer < 1)
            {
                //Every timer seconds check the player's distance.
                CheckDistance();
                ResetTimer();
            }
        }
    }
    internal void ResetTimer()
    {
        timer = time;
    }

    //TODO...make this like the water manager script (take account angle of direction)
    void CheckDistance()
    {
        //Get distance from me to target
        float currentDistance = Vector3.Distance(myTransform.position, target.position);
        //Debug.Log(currentDistance);

        //if target reached the point in which we turn the lights on...
        if (currentDistance <= minDistance)
        {
            //if the lights are not on
            if (!objectOn)
            {
                //turn them on
                StartCoroutine(ActivateResourceIntensiveObjects(true));
            }
        }
        //target hasnt reached point in which to turn the lights on
        else
        {
            //if the lights are on 
            if(objectOn)
            {
                //turn them off
                StartCoroutine(ActivateResourceIntensiveObjects(false));
            }
        }
    }

    IEnumerator ActivateResourceIntensiveObjects(bool active)
    {
        Debug.Log("Light mngr Activated( " + active + ") " + "resource intensive objects! ");
        foreach (Light tomeLight in tomeLights)
        {
            if (!active)
            {
                tomeLight.enabled = false;
                objectOn = false;
            }
            else
            {
                tomeLight.enabled = true;
                objectOn = true;
            }
            //turn lights on slowly to avoid lag spike
            yield return new WaitForSeconds(0.25f);
        }
    }
}
