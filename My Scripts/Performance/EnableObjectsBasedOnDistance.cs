using UnityEngine;
using UnityStandardAssets.Water;

/// <summary>
///  This script is to be used to help the Occlusion Culling system. 
///  Example: 
///  I have a particleFX that looks like fog where my water meets my land.
///  I place this script on an empty holding my particleFX because... 
///  I want my water reduced in performance when my player is X distance away.
/// </summary>
/// 
public class EnableObjectsBasedOnDistance : MonoBehaviour
{
    #region Variables
    [Tooltip("Drag the obj that will be distance-checked here")]
    public Transform target; //Ex:My empty holding my main camera
    //The particles that block the demanding object
    [Tooltip("Drag a ParticleSystem here that blocks a resource-intensive obj")]
    public ParticleSystem particles;//Ex:My fog fx
    //Control mechanism to start/stop this behavior
    [Tooltip("A control mechanism to enable/disable this script entirely")]
    public bool doBehavior = true;
    //Drag demanding CPU/GPU object you wish to control here.
    [Tooltip("Drag a resource-intensive object you wish to cull here")]
    public GameObject demandingObject;//Ex:My WaterProDaytime
    Water water;
    public ParticleSystem fogBehindMe;
    //How often (sec) do we check the player's distance from this object?
    [Tooltip("How often the script checks the target's position in seconds")]
    public float timer;//Ex:I check every 3 seconds...
    //How far away do you need to be for this behavior to do its thang thang?
    [Tooltip("How far away target needs to be for this behavior to activate")]
    public float minDistance;/*Ex - I have specifed my target needs to be > 10m away from this position
     * in order to turn off my resource-intensive object */
    [Tooltip("1 = Target looking right at this. -1 = Target looking directly away")]
    public float forwardAngle; //I use 0.3f
    //0 is the particle fx is right here. 1 is 1m forward of it, 2 is 2m....
    [Tooltip("Add Meters to your particleFx's forward position you want this behavior to distance check from")]
    public float particleForwardPadding;//Ex. I set mine at 3

    #region Don't-touch-unless-you're-a-developer-Variables
    private Transform myTransform;
    //Is the resource-intensive object on or off?
    bool objectOn = true; //Have your object start on to reduce a lag spike
    //Variable to reset the timer
    float time;
    //Variable to check if target isFacing the resource-intensive object
    bool isFacingObject;
    //Is target behind the blocking particles?
    bool behindBlocker;
    //DONT WORRY ABOUT IT, GEEZ
    //internal float cachedAngle;
    #endregion Don't-touch-unless-you're-a-developer-Variables

    #endregion All 

    void Start()
    {
        water = demandingObject.GetComponentInChildren<Water>();
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
                //Every timer seconds check the player's distance and forward facing angle from the fog in front of the water.
                CheckDistanceAndParticles();
                ResetTimer();
            }
        }
    }
    internal void ResetTimer()
    {
        timer = time;
    }
    internal void CheckDistanceAndParticles()
    {
        //Check player's distance from the blocking particleFx
        var relativePoint = myTransform.InverseTransformPoint(target.position);
        /* In my case, negative numbers on the y occur when my target is behind my blocking particleFX.
         * Use Debug.Log(relativePoint) to check your needed x,y,z values ahead and behind 
         */
        //Debug.Log(relativePoint);
        if (relativePoint.y < particleForwardPadding)
        {
            behindBlocker = true;
            //Debug.Log("Player is behind or very near particles, no need to check which way he's facing to ensure water turns refractive fast enough! ");
            if (!objectOn)
            {
                ActivateResourceIntensiveObject(true);
            }
            //If player is close enough, we always want the water to look good af.
        }
        else
        {
            //Target is in front of the blocking particles...
            behindBlocker = false;
            //Now check which way target is facing
            Vector3 dirFromAtoB = (myTransform.position - target.position).normalized;
            float dot = Vector3.Dot(dirFromAtoB, target.forward);

            //Debug.Log(dot + " dot vs forwardAngle  " + forwardAngle + " dot needs to be under forward angle to turn simple");
            if (dot < forwardAngle)
            { //Target is in front of this fog, facing away from it.
                if (objectOn) 
                {
                    //Debug.Log(dot + " angle is under " + forwardAngle + " water turning simple");
                    ActivateResourceIntensiveObject(false);
                    isFacingObject = false;
                }
            }
            else
            {
                //Player is in front of the fog, facing it.
                isFacingObject = true;
            }
        }

        //Check again if the player isn't behind the particles
        if (!behindBlocker)
        {
            //Get distance from me (particles) to target(player) one more time
            float currentDistance = Vector3.Distance(myTransform.position, target.position);
            if (currentDistance > minDistance)
            {
                if (objectOn)
                {
                    ActivateResourceIntensiveObject(false); //turn water simple
                }
            }
            //Player is behind the particle fx, if his new current distance is less than our minDistance
            else if (currentDistance < minDistance)
            {
                //The target is within our minDistance
                if (isFacingObject)
                {//Only ever activate the object if target is facing it
                    if (!objectOn)
                    {
                        ActivateResourceIntensiveObject(true);

                        //Also stop playing the blocking particleFX if the player isn't looking at it.
                        particles.Stop();
                       // Debug.Log("target within min distance && target isFacing " + "me! Activating object")
                    }
                }
                else
                {
                    if(!particles.isPlaying)
                    {
                        particles.Play();
                    }
                }
            }
        }
    }

    #region Single Resource Intensive Object Method Here
    internal void ActivateResourceIntensiveObject(bool active)
    {
        if (!active)
        {
            //False, deactivate the object
            water.waterMode = Water.WaterMode.Simple;
            //False, Stop() playing the particleFX behind me too
            fogBehindMe.Stop();
            objectOn = false;
           // Debug.Log("Water turned simple. 1/3 quality. Fog behind me off");
        }
        else
        {
            //True, activate the object
            water.waterMode = Water.WaterMode.Refractive;
            //True, Play() the particleFX behind me
            fogBehindMe.Play();
            objectOn = true;
            //Debug.Log("Water turned refractive. 3/3 quality. Fog behind me on");
        }
    }
    #endregion

}
