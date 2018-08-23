using System.Collections;
using UnityEngine;
using LowPoly.Character;

public class Bird : Interactable 
{
    const string PLAYER_NAME = "Liam";
    PlayerController player;

    public GameObject canvas;

    void OnTriggerEnter(Collider other)
    {
        if (other.name == PLAYER_NAME)
        {
            if (player == null)
            {
                player = other.GetComponent<PlayerController>();
            }

            player.m_PersonRequestingToBeSpokenWith = this.gameObject;
            player.hud.OpenMessagePanel(this.gameObject, null);
        }
    }

    public override void Interact()
    {
        StartCoroutine(ShowDescription());    
    }

    IEnumerator ShowDescription()
    {
        canvas.SetActive(true);
        yield return new WaitForSeconds(5);
        canvas.SetActive(false);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == PLAYER_NAME)
        {
            canvas.SetActive(false);
            StopCoroutine(ShowDescription());
            player.hud.CloseMessagePanel();
            player.m_PersonRequestingToBeSpokenWith = null;
        }
    }

    /*I want one bird to fly away ...hes guilty, he killed him!
    *Player must keep following this bird to unconver the mystery.
    *Bird flies away 3 times before going up to top of temple.
    *aka conversation goal == complete when with have conversation w/ bird at top of mountain.
    *You get him to confess at the top of the temple.
    *He jumps off to his death in a less than dramatic "cutscene" (bloodsplatter)
    *
    */
}
