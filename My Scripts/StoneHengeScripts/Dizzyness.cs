using System.Collections;
using UnityEngine;

public class Dizzyness : MonoBehaviour
{
    public int numRotationsRequiredToMakeDizzy = 3;
    bool IsDizzy { get; set; }

    bool hasStartedTimer;
    Collider myCollider;
    string playerName = "Liam";

    public StonehengeManager sm;
    public Quest my_quest;


    //This component needs to be loaded onto EyeGuard at runtime after player has been assigned Quest
    void OnTriggerStay(Collider other)
    {
        if (!hasStartedTimer && other.name == playerName && !IsDizzy && my_quest != null)
        {
            Debug.Log("Started rundwon");
            //StartCoroutine(StartTimer(startRotation));
            StartCoroutine(MakeDizzy());
        }
        else
        {
            Debug.Log("did not start getting dizzy because quest hasnt been assigned yet");
        }
    }

    IEnumerator MakeDizzy()
    {
        float countdown = 10f;
        hasStartedTimer = true;

        float startingAngle = transform.eulerAngles.y;
        float previousY = transform.eulerAngles.y;

        float sumRotation = 0;

        int numFullRotations = 0;

        //you've got 10 seconds to complete 3 rotations
        while (countdown > 0 && !IsDizzy)
        {                           //spun once, twice, thrice relative to start
            //confused = (starting angle + (360 + 360 + 360));
            if (numFullRotations > 2)
            {
                BecomeDizzy();
                break;
            }

            //if we have moved , can't just stand in the same spot 
            if(transform.eulerAngles.y > previousY || transform.eulerAngles.y < previousY)
                sumRotation += Mathf.Abs(transform.eulerAngles.y);
            
            Debug.Log("Added to sum of rotational angles. is now " + sumRotation);
            //mod 360 means full circle, assume 1 second value calculating will be 20 degrees off at most
            if(sumRotation != 0 && sumRotation % 360 <= 20 )
            {
                Debug.Log("rotated in full circle relative to starting point");
                numFullRotations++;
            }


            yield return new WaitForSeconds(1);
            previousY = transform.eulerAngles.y;
            countdown--;

            //yo i copy pasted the below and have no idea what it does.
            //angle_delta = (((currentY - previousY) + 180) - Mathf.Floor(((currentY - previousY) + 180) / 360) * 360) - 180;

        }
        hasStartedTimer = false;
        Debug.Log("reset timer");

    }

    void BecomeDizzy()
    {
        //Update the manager
        sm.HasCircledMe = true;

        Debug.Log("You spun around me 3 times fast! Im confused...");
        //dont let behavior loop execute
        IsDizzy = true;

        //turned off for testing, enable when done testing
        //Terminate();
    }

    void Terminate()
    {
        StopAllCoroutines();
        myCollider.enabled = false;
        Destroy(this);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == playerName)
        {
            hasStartedTimer = false;
            Debug.Log("stopped coroutine bc you left");
            StopAllCoroutines();
            if (IsDizzy)
            {
                //Terminate();
            }
            Debug.Log("Stopped timer!");
        }
    }
}
