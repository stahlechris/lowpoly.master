using UnityEngine;
using LowPoly.Character;

public class FireplaceListener : MonoBehaviour 
{
    public CritterAI critterAI;

    string shortID = "";
    public Light firelight;
    public ParticleSystem fire;
    public ParticleSystem smoke;

    // Use this for initialization
    private void OnEnable()
    {
        DialogueEvents.OnDialogueEnd += Handle_OnDialogueEnd;
        DialogueEvents.OnDialogueStart += Handle_OnDialogueStart;
	}


    private void Handle_OnDialogueStart(Dialogue dialogueItem)
    {
        shortID = "";
        shortID = dialogueItem.dialogueID.Substring(0, 2);
        //Debug.Log(shortID);
        if (shortID == "Ca")
        {
            critterAI.doWander = false;

            firelight.enabled = false;
            fire.Stop();
            smoke.Stop();
        }
    }

    private void Handle_OnDialogueEnd(Dialogue dialogueItem)
    {
        shortID = "";
        shortID = dialogueItem.dialogueID.Substring(0, 2);
        if (shortID == "Ca")
        {
            critterAI.doWander = true;

            firelight.enabled = true;
            fire.Play();
            smoke.Play();
        }
    }


    private void OnDisable()
    {
        DialogueEvents.OnDialogueStart -= Handle_OnDialogueStart;
        DialogueEvents.OnDialogueEnd -= Handle_OnDialogueEnd;

    }
}
