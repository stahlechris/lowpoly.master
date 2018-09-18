using UnityEngine;
using LowPoly.Character;

public class Cauldron : MonoBehaviour 
{
    public AudioSource audioSource;
    public CameraChanger cameraChanger;
    public GameObject conversationCam;
    bool hasEntered;
    PlayerController player;
    public bool CanCook { get; set; }

    void OnTriggerEnter(Collider other)
    {
        if (CanCook)
        {
            if (other.CompareTag("Player") && !hasEntered)
            {
                hasEntered = true;

                if (player == null)
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
                    //change to their cam
                    cameraChanger.FirstPersonCam = conversationCam;
                }
                player.m_PersonRequestingToBeSpokenWith = this.gameObject;
                player.hud.OpenMessagePanel(this.gameObject, "- Press E to Cook -");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && player != null)
        {
            hasEntered = false;

            player.hud.CloseMessagePanel();
            player.m_PersonRequestingToBeSpokenWith = null;
            player.alreadySpeaking = false;
        }
    }


}
