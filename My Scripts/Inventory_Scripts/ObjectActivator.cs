﻿using UnityEngine;

/* This script disables resource intensive(RIO) GO's when a conversation is started and
 * enables them when a conversation is finished...
 * This script assumes all conversations will take place away from RIO's */
public class ObjectActivator : MonoBehaviour 
{
    public GameObject[] ResourceIntensiveObjects;

    void Start()
    {
        DialogueEvents.OnDialogueStart += Handle_OnDialogueStart;
        DialogueEvents.OnDialogueEnd += Handle_OnDialogueEnd;
    }

    void Handle_OnDialogueStart(Dialogue dialogueItem)
    {
        //Ex: Text will display slower if water is on.
        ActivateResourceIntensiveObjects(false);
    }
    void Handle_OnDialogueEnd(Dialogue dialogueItem)
    {
        ActivateResourceIntensiveObjects(true);
    }

    public void ActivateResourceIntensiveObjects(bool active)
    {
        foreach (GameObject go in ResourceIntensiveObjects)
        {
            if (!active)
            {
                go.SetActive(false);
            }
            else
            {
                go.SetActive(true);
            }
        }
    }

    // ! VERY important to unsubscribe when using static events ! \\
    void OnDisable()
    {
        DialogueEvents.OnDialogueStart -= Handle_OnDialogueStart;
        DialogueEvents.OnDialogueEnd -= Handle_OnDialogueEnd;
    }
    // ! VERY important to unsubscribe when using static events ! \\
}
