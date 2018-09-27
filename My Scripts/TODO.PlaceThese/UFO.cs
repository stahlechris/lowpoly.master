using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour 
{
    const string DIALOGUE_ID = "Ed-0";
    [SerializeField] Light abductionLight;
    [SerializeField] Light ringLight;
    [SerializeField] GameObject abductionCylinder;
    [SerializeField] Animator abductionTarget;
    ParticleSystem blastOff;
    Rigidbody myRigidbody;
    Levitate myLevitate;

    private void Start()
    {
        blastOff = GetComponentInChildren<ParticleSystem>();
        myRigidbody = GetComponent<Rigidbody>();
        myLevitate = GetComponent<Levitate>();
        DialogueEvents.OnDialogueStart += Handle_OnDialogueStart;
    }

    void Handle_OnDialogueStart(Dialogue dialogueItem)
    {
        if(dialogueItem.dialogueID == DIALOGUE_ID)
        {
            StartCoroutine(AbductTarget());
        }
    }


    IEnumerator AbductTarget()
    {
        yield return new WaitForSeconds(5);
        //activate ring light
        ringLight.enabled = true;
        yield return new WaitForSeconds(0.5f);
        //activate cylinder
        abductionCylinder.SetActive(true);
        //activate abduction light
        abductionLight.enabled = true;
        //trigger deer "Abduct" animation clip
        abductionTarget.SetTrigger("Abduct");
    }

    public void Inform()
    {
        //If here, deer has said "Im in the ship"

        StartCoroutine(PackUpAndFlyAway());

    }

    IEnumerator PackUpAndFlyAway()
    {
        //turn off cylinder
        abductionCylinder.SetActive(false);
        //turn off abduction light
        abductionLight.enabled = false;
        //turn off ring light
        ringLight.enabled = false;
        yield return new WaitForSeconds(0.5f);
        //start levitating violently to look like taking off.
        myLevitate.frequency = 45f;
        //activate blastoff particle fx
        blastOff.Play();
        yield return new WaitForSeconds(3);
        //stop moving entirely
        Destroy(myLevitate);
        //apply force to rigidbody to make me fly away
        yield return new WaitForSeconds(0.25f);
        myRigidbody.AddRelativeForce(new Vector3(0,200,0),ForceMode.Impulse);
        //turn on trail renderer for streaks in sky
        Destroy(this.gameObject, 1);
    }


    private void OnDisable()
    {
        DialogueEvents.OnDialogueStart -= Handle_OnDialogueStart;
    }
}
