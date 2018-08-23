/* I STARTED THIS SCRIPT TO MOVE THE SUN (DIRECTIONAL LIGHT) AROUND THE SCENE.
 * I DIDN'T FINISH IT BECAUSE LIGHT EATS PERFORMANCE SO FAST IT'S NOT WORTH IT.
using UnityEngine;

public class TimeOfDay : MonoBehaviour 
{
    [SerializeField] Light sun;
    [SerializeField] float secondsInDay = 250f;

    [Range(0, 1)] [SerializeField] float currentTime = 0;

    [SerializeField] float timeMultiplier = 1f;
    [SerializeField] float sunInitialIntensity = 0;

    private void Start()
    {
        sunInitialIntensity = sun.intensity;
    }

    private void Update()
    {
        UpdateSun();
        //the current time is added to every deltaTimeFrame
        currentTime += (Time.deltaTime / secondsInDay) * timeMultiplier;

        //if goes past our max value (currentTime), reset our day
        if(currentTime >= 1)
        {
            currentTime = 0;
        }
    }

    private void UpdateSun()
    {
        //change 170 depending on where your horizon is
        sun.transform.localRotation = Quaternion.Euler((currentTime * 360f) - 90, 170, 0);

        float intensityMultiplier = 1f;

        //just before the sunrise....sunset
        if(currentTime <= 0.23f || currentTime >= 0.75f)
        {
            intensityMultiplier = 0;
        }
        else if( currentTime <= 0.25f)
        { //fade in intensity
            intensityMultiplier = Mathf.Clamp01( (currentTime - 0.23f) * (1/0.02f) );
        }
        else if(currentTime >= 0.73f)
        { //fade out intensity
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTime - 0.73f) * (1/0.02f)));
        }
        sun.intensity = sunInitialIntensity * intensityMultiplier;
    }
}
*/