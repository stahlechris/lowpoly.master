using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPoint : MonoBehaviour 
{
    [SerializeField] SpaceSheep spaceSheep;
    ParticleSystem particle;

    bool HasEntered { get; set; }
    public bool HasJumped { get; set; }
    public bool HasJumpedTwice { get; set; }
    public bool HasJumpedThrice { get; set; }

    bool AlreadyInformedOfJump { get; set; }

    bool TimerOn { get; set; }
    float totalTime = 0;
    Transform myTransform;
    Vector3 myTransformOriginalPosition;

    private void Start()
    {
        myTransform = GetComponent<Transform>();
        myTransformOriginalPosition = myTransform.position;
        particle = GetComponentInChildren<ParticleSystem>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (!HasEntered && other.CompareTag("Player"))
        {
            HasEntered = true;
            spaceSheep.InformPlayerHere();
        }
        else if(HasEntered && TimerOn) //the timer is started
        {
            //player left and re-entered trigger fast enough to be considered a jump
            if(totalTime < 0.6f && !AlreadyInformedOfJump)
            {
                Debug.Log("Player probably jumped");
                AlreadyInformedOfJump = true;
                StopCoroutine(Timer());
                Debug.Log("Informing Sheep that player jumped");
                spaceSheep.InformPlayerJumped();
                //move it down a little to act like you jumped it into the ground
                myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y - 0.1f, myTransform.position.z);
            }
        }
    }

    IEnumerator Timer()
    {
        TimerOn = true;
        float duration = 1;
        totalTime = 0;
        while(totalTime <= duration)
        {
            totalTime += Time.deltaTime;
            yield return null;
        }
        TimerOn = false;
    }

    public void Reset()
    {
        if (HasJumpedThrice)
        {
            Destroy(this.gameObject);
        }
        else //this scripts values reset but not space sheeps - if one resets the other needs to as well.
        {
            spaceSheep.Reset();
            Debug.Log("Reset called to not destroy");
            myTransform.position = myTransformOriginalPosition;
            TimerOn = false;
            HasEntered = false;
            HasJumped = false;
            HasJumpedTwice = false;
            HasJumpedThrice = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        AlreadyInformedOfJump = false;
        Debug.Log("Out of trigger, checking for jump");
        if (HasEntered && !TimerOn)
        {
            //if play enters and leaves trigger in X seconds, he jumped
            StartCoroutine(Timer());
        }
        else
        {
            Debug.Log("reset called bc (HasEntered && ! TimerOn) evaluated to false");
            Reset();
        }
    }
}
