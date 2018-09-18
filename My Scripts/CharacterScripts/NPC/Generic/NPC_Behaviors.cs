using UnityEngine;
using LowPoly.Character;
using System.Collections;
using LowPoly.CameraUI;

public class NPC_Behaviors : Interactable 
{
    public CameraChanger cameraChanger;
    public GameObject conversationCam;
    public AudioSource audioSource;
    PlayerController player;
    public bool playerHasTalkedToMe = false;
    [SerializeField]Dialogue my_Dialogue;
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
            //this must be here rather than interact because this operation takes a while
            if (conversationCam != null)
            {
                if(audioSource!= null)
                {
                    //play their conversationMusic
                    audioSource.Play();
                }
                //change to their cam
                cameraChanger.FirstPersonCam = conversationCam;
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
        //This is coming up null when you get the bird's journal quest idk why
        if (my_Dialogue != null)
        {
            Debug.Log("Interacted in NPC_Behaviors to HaveConversation()");
            my_Dialogue.HaveConversation();
        }
    }
   
    void OnDisable()
    {
        //StopAllCoroutines();
    }
}
