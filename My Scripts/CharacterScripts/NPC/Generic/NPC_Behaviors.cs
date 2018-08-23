using UnityEngine;
using LowPoly.Character;
using System.Collections;

public class NPC_Behaviors : Interactable 
{
    PlayerController player;
    public bool playerHasTalkedToMe = false;
    Dialogue my_Dialogue;
    const string PLAYER_NAME = "Liam";
    bool hasEntered = false; //stop triggering OnTriggerEnter twice when you enter once

    void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(4f);
        my_Dialogue = transform.Find("NPC_UISocket/NPC_Canvas(Clone)").
                    GetComponentInChildren<Dialogue>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.name == PLAYER_NAME && !hasEntered)
        {
            hasEntered = true;
            if (player == null)
            {
                player = other.GetComponent<PlayerController>();
            }

            player.m_PersonRequestingToBeSpokenWith = this.gameObject;
            player.hud.OpenMessagePanel(this.gameObject, null);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == PLAYER_NAME)
        {
            hasEntered = false;
            player.hud.CloseMessagePanel();

            player.m_PersonRequestingToBeSpokenWith = null;
        }
    }

    public override void Interact()
    {
        if (my_Dialogue != null)
        {
            Debug.Log("Interacted in NPC_Behaviors to HaveConversation()");
            my_Dialogue.HaveConversation();
        }
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }
}
