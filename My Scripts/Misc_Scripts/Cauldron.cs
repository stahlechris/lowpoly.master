using UnityEngine;
using LowPoly.Character;
using System;

public class Cauldron : MonoBehaviour 
{
    public AudioSource audioSource;
    public CameraChanger cameraChanger;
    public GameObject conversationCam;
    bool hasEntered;
    PlayerController player;
    public bool CanCook;

    #region Const Strings
    const string PLAYER_TAG = "Player";
    const string COOKING_MESSAGE = "- Press E to Cook -";
    #endregion


    void OnTriggerEnter(Collider other)
    {
        if (CanCook)
        {
            if (other.CompareTag(PLAYER_TAG) && !hasEntered)
            {
                hasEntered = true;

                if (player == null) //is this a memory leak or is ok??
                {
                    player = other.GetComponent<PlayerController>();
                }
                if (conversationCam != null)
                {
                    if (audioSource != null)
                    {
                        //play their conversationMusic
                        audioSource.Play();
                    }
                    cameraChanger.FirstPersonCam = conversationCam;
                }
                player.m_PersonRequestingToBeSpokenWith = this.gameObject;
                player.hud.OpenMessagePanel(this.gameObject, COOKING_MESSAGE);
            }
        }
        else
        {
            Debug.Log("Carl hasn't given you permission to use his cauldron yet!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG) && player != null)
        {
            hasEntered = false;

            player.hud.CloseMessagePanel();
            player.m_PersonRequestingToBeSpokenWith = null;
            player.alreadySpeaking = false;
        }
    }

}
