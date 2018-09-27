using UnityEngine;
using System.Collections;

public class CarlsCage : MonoBehaviour 
{
    string DialogueID = "Carl-0"; //do this behavior when a certain conversation happens
    public Transform deerOne;//stage left
    public Transform deerTwo; //stage right
    Quaternion deerOneCachedRotation;
    Quaternion deerTwoCachedRotation;
    public bool rotating = false;

    private float rotSpeed;

    private void OnEnable()
    {
        DialogueEvents.OnDialogueStart += Handle_OnDialogueStart;
        DialogueEvents.OnDialogueEnd += Handle_OnDialogueEnd;
    }

    void Handle_OnDialogueStart(Dialogue dialogueItem)
    {
        if(dialogueItem.dialogueID == DialogueID)
        {
            StartCoroutine(SlowlyRotateTowardsTarget());
            StartCoroutine(SlowlyRotateTowardsTargetTwo());
        }
    }

    void Handle_OnDialogueEnd(Dialogue dialogueItem)
    {
        if (dialogueItem.dialogueID == DialogueID)
        {
            StartCoroutine(Wait());
            //reset back to original positions
            deerOne.rotation = deerOneCachedRotation;
            deerTwo.rotation = deerTwoCachedRotation;
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
    }
    IEnumerator SlowlyRotateTowardsTarget()
    {
        //wait a sec....
        yield return new WaitForSeconds(4);


        float counter = 0;
        float duration = 2;
        float max = 0;
        float min = -50;

        float internalAngle = 0;
        float speed = 0.35f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            internalAngle -= speed * Time.deltaTime;
            internalAngle = Mathf.Clamp(internalAngle, min, max);

            deerOne.rotation = Quaternion.Euler(deerOne.rotation.eulerAngles.x,
                                                deerOne.rotation.eulerAngles.y, internalAngle);
            yield return null;
        }

    }
    IEnumerator SlowlyRotateTowardsTargetTwo()
    {
        //wait a sec....
        yield return new WaitForSeconds(4);


        float counter = 0;
        float duration = 2;
        float max = 20;
        float min = -50;

        float internalAngle = 0;
        float speed = 0.15f; //hacking this variable to get what I want...

        while (counter < duration)
        {
            counter += Time.deltaTime;
            internalAngle -= speed * Time.deltaTime;
            internalAngle = Mathf.Clamp(internalAngle, min, max);

            deerTwo.rotation = Quaternion.Euler(deerTwo.rotation.eulerAngles.x,
                                                deerTwo.rotation.eulerAngles.y, -internalAngle);
            //Debug.Log(internalAngle);
            yield return null;
        }
    }


    void OnDisable()
    {
        DialogueEvents.OnDialogueStart -= Handle_OnDialogueStart;
        DialogueEvents.OnDialogueEnd -= Handle_OnDialogueEnd;
    }
}
