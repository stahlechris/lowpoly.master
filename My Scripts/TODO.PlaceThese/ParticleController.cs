
using UnityEngine;

/* This script should be attached to particle systems.
 * It stops and starts particleFx based on the player's distance away from them.
 */

public class ParticleController : MonoBehaviour 
{
    #region Variables
    [Tooltip("Drag the obj that will be distance-checked here")]
    public Transform target; //Ex:My empty holding my main camera
    //The particles that block the demanding object
    [Tooltip("Drag a ParticleSystem here that blocks a resource-intensive obj")]
    public ParticleSystem myParticles;//Ex:My fog fx
    //Control mechanism to start/stop this behavior
    [Tooltip("A control mechanism to enable/disable this behavior")]
    public bool doBehavior = true;
    //How often (sec) do we check the player's distance?
    [Tooltip("How often the script checks the target's position in seconds")]
    public float timer;//Ex:I check every 3 seconds...
    //How far away do you need to be for this behavior to do its thang thang?
    [Tooltip("How far away target needs to be for this behavior to activate")]
    public float minDistance;/*Ex - I have specifed my target needs to be > 10m away from this position
     * in order to turn off myParticles */
    [Tooltip("1 = Target looking right at this. -1 = Target looking directly away")]
    public float forwardAngle; //I use 0.3f for a wide angle
    //0 is the particle fx is right here. 1 is 1m forward of it, 2 is 2m....
    [Tooltip("Add Meters to your particleFx's forward position you want this behavior to distance check from")]
    public float particleForwardPadding;//Ex. I set mine at 3

    public float maxDistance;

    #region Private Don't-Mess-With-Variables
    private Transform myTransform;
    //Is the resource-intensive object on or off?
    private bool objectOn = true; //Have your object start on to reduce a lag spike
    //Variable to reset the timer
    private float time;
    //Variable to check if target isFacing the resource-intensive object
    private bool isFacingObject;
    //Is target behind the blocking particles?
    private bool behindBlocker;
    //DONT WORRY ABOUT IT, GEEZ
    //internal float cachedAngle;
    #endregion

    #endregion

	void Start () 
    {
        myTransform = transform;
        myParticles = GetComponent<ParticleSystem>();
        time = timer;
	}
	
    void Update()
    {
        if (doBehavior)
        {
            //decrement the time by framerate independent amount
            timer -= Time.deltaTime;
            if (timer < 1)
            {
                //Every timer seconds check the player's distance 
                CheckDistance();
                ResetTimer();
            }
        }
    }
    void ResetTimer()
    {
        timer = time;
    }

    void CheckDistance()
    {
        float relativeDistance = Vector3.Distance(myTransform.localPosition, target.localPosition);

//        Debug.Log(relativeDistance);
        if(relativeDistance > maxDistance)
        {
            if (!objectOn)
            {
                ActivateResourceIntensiveObject(true);
            }

        }
        else
        {
            if (objectOn)
            {
                ActivateResourceIntensiveObject(false);
            }
        }
        //if (relativePoint.y < particleForwardPadding)
        //{
        //    behindBlocker = true;
        //    //Debug.Log("Player is behind or very near particles, no need to check which way he's facing.");
        //    if (!objectOn)
        //    {
        //        ActivateResourceIntensiveObject(true);
        //    }
        //    //If player is close enough, we always want the water to look good af.
        //}
        //else
        //{
        //    //Target is in front of the blocking particles...
        //    behindBlocker = false;
        //    //Now check which way target is facing
        //    Vector3 dirFromAtoB = (myTransform.position - target.position).normalized;
        //    float dot = Vector3.Dot(dirFromAtoB, target.forward);

        //    //Debug.Log(dot + " dot vs forwardAngle  " + forwardAngle + " dot needs to be under forward angle to turn simple");
        //    if (dot < forwardAngle)
        //    { //Target is in front of this fog, facing away from it.
        //        if (objectOn)
        //        {
        //            //Debug.Log(dot + " angle is under " + forwardAngle + " water turning simple");
        //            ActivateResourceIntensiveObject(false);
        //            isFacingObject = false;
        //        }
        //    }
        //    else
        //    {
        //        //Player is in front of the fog, facing it.
        //        isFacingObject = true;
        //    }
        //}

        ////Check again if the player isn't behind the particles
        //if (!behindBlocker)
        //{
        ////Get distance from me (particles) to target(player) one more time
        //float currentDistance = Vector3.Distance(myTransform.position, target.position);
        //if (currentDistance > minDistance)
        //{
        //    if (objectOn)
        //    {
        //        ActivateResourceIntensiveObject(false); //turn water simple
        //    }
        //}
        ////Player is behind the particle fx, if his new current distance is less than our minDistance
        //else if (currentDistance < minDistance)
        //{
        //    //The target is within our minDistance
        //    if (isFacingObject)
        //    {//Only ever activate the object if target is facing it
        //        if (!objectOn)
        //        {
        //            ActivateResourceIntensiveObject(true);

        //            //Also stop playing the blocking particleFX if the player isn't looking at it.
        //            particles.Stop();
        //            // Debug.Log("target within min distance && target isFacing " + "me! Activating object")
        //        }
        //    }
        //    else
        //    {
        //        if (!particles.isPlaying)
        //        {
        //            particles.Play();
        //        }
        //    }
        //}
        //}
    }

    #region Single Resource Intensive Object Method Here
    private void ActivateResourceIntensiveObject(bool active)
    {
        if (!active)
        {
            //False, deactivate the object
            myParticles.Stop();
            objectOn = false;
            //Debug.Log("Rain, splash & sploosh Stop().");
        }
        else
        {
            //True, activate the object
            myParticles.Play();
            objectOn = true;
            //Debug.Log("Rain, splash & sploosh Play().");

        }
    }
    #endregion
}
