using UnityEngine;
using System.Collections;
using LowPoly.Character;

public class HelpfulFish : Interactable 
{
    const string PLAYER_NAME = "Liam";
    public PlayerController player;
    public Rigidbody rb;

    public AudioSource audioSource;
    public Animator animator;
    public float thrust;
    public GameObject canvas;

    public TurnObjectsOffWhenInWater sheepSphere;

    void OnTriggerStay(Collider other) //idea. ride the fish. make a little cave 
    {
        if (player == null)
        {
            player = other.GetComponent<PlayerController>();
            rb = other.GetComponent<Rigidbody>();
        }

        player.m_PersonRequestingToBeSpokenWith = this.gameObject;
        player.hud.OpenMessagePanel(this.gameObject, "-Pls help me-");
    }

    public override void Interact()
    {
        //add enough force to player to kick him back to the shore
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        //Based on player's forward, apply a backwards & upward force to his rb
        rb.AddRelativeForce(new Vector3(0, 500, 0) + ((Vector3.forward*-1) * thrust));
        //Play a water jet noise
        audioSource.Play();
        //"NOOOOO!!! HELPFULFISH - I LOVE YOU!!
        Animator anim = player.GetComponent<Animator>();
        anim.SetTrigger("ReachingForward");
        InformSheepSphere(); //activates sheep again in scene.
        StartCoroutine(SayGoodbye(anim));
    }

    //TODO make a secret system. collect secrets
    IEnumerator SayGoodbye(Animator anim)
    {
        //Wait for player to look back
        yield return new WaitForSeconds(2);
        PutPlayerUpright();
        canvas.SetActive(true);
        animator.SetTrigger("GoUnder");
        yield return new WaitForSeconds(4);
        canvas.SetActive(false);
        Destroy(this.gameObject, 2);
    }

    void PutPlayerUpright()
    {
        StartCoroutine(Wait());
        player.alreadySpeaking = false; //reset speaking flag
        player.transform.rotation = Quaternion.identity; //reset rotation
        rb.constraints = RigidbodyConstraints.None;//unfreeze
        rb.constraints = RigidbodyConstraints.FreezeRotation;//refreeze to ground
    }
    IEnumerator Wait()
    {
        //This is here so the click noise happens when the player is re-adjusted
        yield return new WaitForSeconds(1);
    }
    void OnTriggerExit(Collider other)
    {
        player.hud.CloseMessagePanel();
        player.m_PersonRequestingToBeSpokenWith = null;
    }
    void InformSheepSphere()
    {
        sheepSphere.ActivateComponents();
    }


    void CleanUpTheOcean()
    {
        //TODO destroy/disable objects here...
        //After you talk to the fish, or return to shore with the coin,
        //I want a boulder to fall to block the ocean so we can destroy all objects and save on performance
    }
}
