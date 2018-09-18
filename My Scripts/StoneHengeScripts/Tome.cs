using System.Collections;
using UnityEngine;
using LowPoly.Character;

public class Tome : Interactable 
{
    const string PLAYER_NAME = "Liam";
    bool hasEntered = false; //stop triggering OnTriggerEnter twice when you enter once
    PlayerController player;
    bool securityArmed = false;
    public TomeManager tomeManager;
    GameObject canvas;
    bool haventReadMe = false;
    AudioClip my_audioClip;

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
            player.hud.OpenMessagePanel(this.gameObject, "- Press X to Read -");
        }
    }

    public override void Interact()
    {
        if(!securityArmed && !haventReadMe)
        {
            //Debug.Log("Hey this guy tried to read me! Help!!!");
            tomeManager.Handle_ReadingWithoutPermission(player);
        }
        else if(securityArmed && !haventReadMe)
        {

            //before looting a tome have a message popup...
            //It looks like i'll only have time to read one before he wakes up again".

            //play my particle fx and sound fx
            //assign new power
            //AssignPowerToPlayer(gameObject.name);
        }
        else
        {
            Debug.Log("You have already read me for a power");
        }
    }

    IEnumerator ShowDescription()
    {
        canvas.SetActive(true);
        yield return new WaitForSeconds(5);
        canvas.SetActive(false);
    }
    void AssignPowerToPlayer(string tomeName)
    {
        //switch based on string id
    }

    void OnTriggerExit(Collider other)
    {
        if(other.name == PLAYER_NAME)
        {
            hasEntered = false;

            player.hud.CloseMessagePanel();
            player.m_PersonRequestingToBeSpokenWith = null;
        }
    }
}
