using UnityEngine;

/// <summary>
///  This script is to be used to help the Occlusion Culling system. 
///  Example: 
///  I have a particleFX that looks like dense fog where my water meets my land.
///  I place this script on an empty holding my particleFX because... 
///  I want my water area culled when my player is X distance away from the water and can't see it.
/// </summary>
public class EnableObjectsBasedOnDistance : MonoBehaviour
{
    #region Variables
    [Tooltip("Drag the obj that will be distance-checked here")]
    public Transform target; //Ex:My empty holding my main camera
    //The particles that block the demanding object
    [Tooltip("Drag a ParticleSystem here that blocks a resource-intensive obj")]
    public ParticleSystem particles;//Ex:My fog fx
    //Control mechanism to start/stop this behavior
    [Tooltip("A control mechanism to enable/disable this behavior")]
    public bool doBehavior = true;
    //Drag demanding CPU/GPU object you wish to control here.
    [Tooltip("Drag a resource-intensive object you wish to cull here")]
    public GameObject demandingObject;//Ex:My WaterProDaytime
    #region public GameObject[] demandingObjects
    /*
    [Tooltip("Drag resource-intensive objects you wish to cull here... + " +
             "(Developer suggests rarely using this; " +
             "Rather child your multiple GO's into an empty parent.")]
    public GameObject[] resourceIntensiveObjects; //Look at tooltip!!*/
    #endregion 
    //How often (sec) do we check the player's distance?
    [Tooltip("How often the script checks the target's position in seconds")]
    public float timer;//Ex:I check every 3 seconds...
    /*Activate this variable if you want to check particle duration rather than emission count
    //How long does it take for your particleFX to cover what you want hidden?
    //[Tooltip("How long it takes your ParticleSystem to cover what you want hidden?")]
    //public float particleDuration; //EX:My fog is dense enough to hide water after 1.5sec
    */
    [Tooltip("How many particles does your system need to emit before it can hide an object behind it?")]
    public float particleCount; //Ex:My fog needs >= 125 particles to hide my water
    //How far away do you need to be for this behavior to do its thang thang?
    [Tooltip("How far away target needs to be for this behavior to activate")]
    public float minDistance;/*Ex - I have specifed my target needs to be > 10m away from this position
     * in order to turn off my resource-intensive object */
    [Tooltip("1 = Target looking right at this. -1 = Target looking directly away")]
    public float forwardAngle; //I use 0.3f
    //0 is the particle fx is right here. 1 is 1m forward of it, 2 is 2m....
    [Tooltip("Add Meters to your particleFx's forward position you want this behavior to distance check from")]
    public float particleForwardPadding;//Ex. I set mine at 3
    #region Internal Variables
    private Transform myTransform;
    //Is the resource-intensive object on or off?
    internal bool objectOn = true;
    //Variable to reset the timer
    internal float time;
    //Variable to check if target isFacing the resource-intensive object
    internal bool isFacingObject;
    //Is target behind the blocking particles?
    internal bool behindBlocker;
    //DONT WORRY ABOUT IT, GEEZ
    internal float cachedAngle;
    #endregion
    #endregion

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
                //Every timer seconds check the player's distance, forward & particle count
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
        /* In my case, negative numbers occur when my target is behind my blocking particleFX.
         * Use Debug.Log(relativePoint) to check your needed x,y,z values ahead and behind 
         */
        //Debug.Log(relativePoint);
        if (relativePoint.y < particleForwardPadding)
        {
            behindBlocker = true;
            //Debug.Log("Player is behind or near particles, no need to check if i should deactivate obj);
        }
        else
        {
            //Target is in front of the blocking particles...
            behindBlocker = false;
            //Now check which way target is facing
            Vector3 dirFromAtoB = (myTransform.position - target.position).normalized;
            float dot = Vector3.Dot(dirFromAtoB, target.forward);
            //Debug.Log(dot + " dot vs forwardAngle  " + forwardAngle);
            if (dot < forwardAngle)
            { //Target is NOT looking at me enough for me to show myself
                if (objectOn)
                {
                    //Debug.Log(dot + " angle is under " + forwardAngle + " turned off objects");
                    ActivateResourceIntensiveObject(false); //Single
                    //ActivateResourceIntensiveObjects(false);//Multiple[]
                    isFacingObject = false;
                }
            }
            else
            {
                isFacingObject = true;
            }
        }

        //TODO: Think about changing the logic to turn off the water if we ar3e inside the zone,
        //but camera is looking directly away from it. - Sept. 13, 2018

        //Don't turn off demanding object if target near or behind particleFX
        if (!behindBlocker)
        {
            //Get distance from me to target
            float currentDistance = Vector3.Distance(myTransform.position, target.position);
            if (currentDistance > minDistance)
            {
                if (particles.particleCount > particleCount)
                {
                    //ParticleSystem has emitted enough particles to hide an object
                    if (objectOn)
                    {
                        ActivateResourceIntensiveObject(false); //Single 
                        //ActivateResourceIntensiveObjects(false);//Multiple[]
                    }
                }
                #region Check ParticleFX Duration Instead Of Emmission Count Here
                /* Activate the below if{} and deactivate the above if{}
                 * when you want to check how long (sec) the particleFX has been playing.
                if (particles.time > particleDuration)
                {
                    //Our particles have been playing long enough to cover up object
                    ActivateResourceIntensiveObject(false);
                }
                */
                #endregion
            }
            else if (currentDistance < minDistance)
            {
                //The target is within our minDistance
                if (isFacingObject)
                {//Only ever activate the object if target is facing it
                    if (!objectOn)
                    {
                        ActivateResourceIntensiveObject(true); //Single

                        //Also stop playing the blocking particleFX if the player isn't looking at it.
                        particles.Stop();

                        //ActivateResourceIntensiveObjects(true); //Multiple[]
                        /*Debug.Log("target within min distance && target isFacing " +
                              "me! Activating object");*/
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
        Debug.Log("activated(" +active+ ") "+"resource intensive object!");

        if (!active)
        {
            //False, deactivate the object
            demandingObject.SetActive(false);
            objectOn = false;
        }
        else
        {
            //True, activate the object
            demandingObject.SetActive(true);
            objectOn = true;
        }
    }
    #endregion

    #region Multiple Resource Intensive Objects Method Here
    //public void ActivateResourceIntensiveObjects(bool active)
    //{
    //    Debug.Log("Activated( " + active + ") " + "resource intensive objects! ");
    //    foreach (GameObject go in resourceIntensiveObjects)
    //    {
    //        if (!active)
    //        {
    //            go.SetActive(false);
    //            objectOn = false;
    //        }
    //        else
    //        {
    //            go.SetActive(true);
    //            objectOn = true;
    //        }
    //    }
    //}
#endregion
}
