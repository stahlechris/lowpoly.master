using UnityEngine;
using System.Collections;
public class CheckIfIpsHome : MonoBehaviour 
{
    Collider myCollider;
    bool secretActivated = false;
    public DialogueManager dialogueManager;

    const string IP = "Ip";
    public GameObject IpGameObject;
    public QuestGiver Ip_Questgiver;
    public Collider Ip_Collider;
    const string PLAYER = "Liam";

    public GameObject ruby_For_Legend_Sword;
    public Animator rubyAnimator;
    public AudioSource rubyAudio;
    public Item rubyItem;
    public Levitate rubyLevitate;

    void OnTriggerEnter(Collider other)
    {
        if (!secretActivated) //can't do it twice
        {
            //if the player beats Ip home, a secret Dialogue is loaded and the player is rewarded
            if (other.transform.name == PLAYER)
            {
                myCollider = GetComponent<Collider>();
                DialogueEvents.OnDialogueEnd += Handle_OnDialogueEnd;
                SetupDialogue();
            }
        }
    }
    void SetupDialogue()
    {
        secretActivated = true;
        string path = "Conversation_Ip-2";
        dialogueManager.SetupNewDialogue(IpGameObject, path);
        Ip_Questgiver.hasDisocveredSecretDialogue = true;
    }
    void Handle_OnDialogueEnd(Dialogue d)
    {
        //Debug.Log(d.dialogueID);
        if(d.dialogueID == "Ip-2")
        {
            SetupRuby();
        }
        DestroyMyself();
    }
    void SetupRuby()
    {
        ruby_For_Legend_Sword.SetActive(true);
        //cant move unless animation is off....so wait for animation to finish
        rubyAnimator.SetTrigger("Reveal");
        rubyAudio.Play();
        StartCoroutine(ActivateLevitate());
    }
    IEnumerator ActivateLevitate()
    {
        yield return new WaitForSeconds(1f);
        rubyLevitate.enabled = true;
        rubyAnimator.enabled = false;
        rubyItem.enabled = true;
    }
    void DestroyMyself()
    {
        //reset secret flag
        Ip_Questgiver.hasDisocveredSecretDialogue = false;
        Destroy();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.name == IP && secretActivated)
        {
            other.transform.GetComponent<Animator>().SetBool("run", false);
            //re-enable kinematic so player doesnt bounce off of him
            other.transform.GetComponent<Rigidbody>().isKinematic = true;
            myCollider.enabled = false;
        }
        else if(other.transform.name == IP)
        {
            other.transform.GetComponent<Animator>().SetBool("run", false);
            //re-enable kinematic so player doesnt bounce off of him
            other.transform.GetComponent<Rigidbody>().isKinematic = true;
            Destroy();
        }
    }

    void Destroy()
    {
        DialogueEvents.OnDialogueEnd -= Handle_OnDialogueEnd;
        Destroy(this.gameObject,2f);
    }
    void OnDisable()
    {
        DialogueEvents. OnDialogueEnd -= Handle_OnDialogueEnd;
    }
}
