using UnityEngine;

public class GivePlayerTheHammer : MonoBehaviour 
{
    string match = "Ip-1";
    const string DROP_ITEM = "tr_drop";

    public GameObject ipCampsiteEmptyGameObject;
    public GameObject edHammer;
    public Animator ipAnimator;
    public Item edHammerItemScript;

    private void Start()
    {
        DialogueEvents.OnDialogueEnd += Handle_OnDialogueEnd;
    }

    private void Handle_OnDialogueEnd(Dialogue dialogueItem)
    {
        if(match.Equals(dialogueItem.dialogueID))
        {
            DropEdsHammer();
        }
    }

    private void DropEdsHammer()
    {
        GameObject itemToDrop = edHammer;
        Rigidbody rbItem = itemToDrop.AddComponent<Rigidbody>();

        if (rbItem != null)
        {
            ipAnimator.SetTrigger(DROP_ITEM);

            rbItem.AddForce(transform.up * 2.0f, ForceMode.Impulse);

            //we need a delay so the item doesn't just hang in the air
            Invoke("DestroyDropItem", 0.6f);
        }
    }
    void DestroyDropItem()
    {
        Destroy(edHammer.GetComponent<Rigidbody>());
        PrepareHammerForPickup();
    }

    void PrepareHammerForPickup()
    {
        edHammer.transform.SetParent(ipCampsiteEmptyGameObject.transform);
        edHammer.AddComponent<SphereCollider>();
        edHammerItemScript.enabled = true;
        TellIpToGoToHisTent();
    }


    private void OnDisable()
    {
        DialogueEvents.OnDialogueEnd -= Handle_OnDialogueEnd;
    }


    void TellIpToGoToHisTent()
    {
        NPC_AI m_AI = GetComponentInParent<NPC_AI>();
        Animator animator = GetComponentInParent<Animator>();
        animator.SetBool("run", true);
        m_AI.GoToTent();
        DialogueEvents.OnDialogueEnd -= Handle_OnDialogueEnd;
        Destroy(this);
    }
}
