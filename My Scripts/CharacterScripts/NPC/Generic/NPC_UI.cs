using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPC_UI : MonoBehaviour
{
    Transform m_Transform;
    NPC_Behaviors myNPCbehaviors;
    [SerializeField] GameObject canvasPrefab;
    public Image[] questIcon; //0 => "!" | 1 => "?"...TODO 2 => "..." 3 => "?.."(in progress) 
    Camera cameraToLookAt;
    public int id;
    bool LookAt{ get; set; }
    bool HasQuestIconEnabled { get; set; }
    /*
     * If Quaternion.identity: the object keeps the same rotation as the prefab. 
     * If transform.rotation: the object's rotation is combined with the  prefab's
     */
    void Start()
    {
        m_Transform = transform;
        LookAt = true;
        cameraToLookAt = Camera.main;
        Instantiate(canvasPrefab, m_Transform.position, Quaternion.identity, m_Transform);
        //hud = FindObjects.HUD;
        HasQuestIconEnabled = true;
        questIcon = GetComponentsInChildren<Image>();
        myNPCbehaviors = GetComponentInParent<NPC_Behaviors>();
        DialogueEvents.OnDialogueStart += DialogueEvents_OnDialogueStart;
        DialogueEvents.OnDialogueEnd += DialogueEvents_OnDialogueEnd;

        questIcon[1].enabled = false; //Unity can't be inactive gameobjects, so both of them start on and we have to turn one off.
    }

    void LateUpdate()
    {
        if (LookAt && HasQuestIconEnabled)
        {
            //don't do this if we are in a conversation
            m_Transform.LookAt(cameraToLookAt.transform);
        }
    }

    /*There is a sequential order of who receives notice of the event first.
     * To my knowledge, whosever Start or enable runs first, is first.
     * 
     * TODO: Consider timing each objects Start or Enable method so we 
     * have a guaranteed Order of who receives events first.
     */
    void DialogueEvents_OnDialogueStart(Dialogue dialogueItem)
    {
        //don't calculate lookAt direction every lateUpdate frame if in convo
        LookAt = false;
        //if they're talking to me, get questIcon out of their face 
        if (dialogueItem.dialogueID == myNPCbehaviors.GetComponentInChildren<Dialogue>().dialogueID)
        {
            if (questIcon != null)//TODO change to check that the length is greater > 1
            {
                StartCoroutine(WaitThenDisableQuestIcon());
            }

        }
    }
    IEnumerator WaitThenDisableQuestIcon()
    {
        yield return new WaitForEndOfFrame();
        //Debug.Log("Responding to an OnDialogueStart, NPC_UI disabled ! and ?");
        questIcon[0].enabled = false;
        questIcon[1].enabled = false; 
    }
    void DialogueEvents_OnDialogueEnd(Dialogue dialogueItem)//TODO -> if you plan on keeping the stupid fucking bird, Give him his own component so you can just delete him after hes done
    {
        LookAt = true;
        //Debug.Log(this + " heard dialogue was over");
        //Debug.Log(dialogueItem.dialogueID == myNPCbehaviors.GetComponentInChildren<Dialogue>().dialogueID);

        if (dialogueItem.dialogueID == myNPCbehaviors.GetComponentInChildren<Dialogue>().dialogueID)
        {
            if (dialogueItem.dialogueID.Equals("Bird-1"))//the bird is special
            {
                GoToBetterPlace betterPlace = GetComponentInParent<GoToBetterPlace>();
                betterPlace.BeFree();
                DialogueEvents.OnDialogueStart += DialogueEvents_OnDialogueStart;
                DialogueEvents.OnDialogueEnd -= DialogueEvents_OnDialogueEnd;
            }
            CheckIfQuestIconEnabled();
            //we need to check if our QuestGiver is a turnInPoint and has a quest to give
            //TODO we need to also check if the player has completed the Quest,
            //If they haven't completed the quest, lets put up a grey questionMark 
            QuestGiver I = GetComponentInParent<QuestGiver>();
            if(I.amTurnInPoint && I.Quest != null)
            {
                ChangeQuestStatus("!", true);
                Debug.Log("NPC_UI changed ! to true");
                I.amTurnInPoint = false;
            }
        }
    }
    void CheckIfQuestIconEnabled()//questIcons are Images
    {//This is used to determine if LookAt() is executed every LateUpdate()
        if (!questIcon[0].isActiveAndEnabled && !questIcon[1].isActiveAndEnabled)
        {
            HasQuestIconEnabled = false;
        }
        else
        {
            HasQuestIconEnabled = true;
        }
    }
    public void ChangeQuestStatus(string icon, bool active)
    {
        if (icon != null)
        {
            if (icon.Equals("!"))
            {
                questIcon[0].enabled = active;
                questIcon[1].enabled = false;
            }
            else
            {
                questIcon[1].enabled = active;
                questIcon[0].enabled = false;
            }
        }
    }
    void OnDestroy()
    {
//        Debug.Log(this + " has been destroyed");
        DialogueEvents.OnDialogueStart -= DialogueEvents_OnDialogueStart;
        DialogueEvents.OnDialogueEnd -= DialogueEvents_OnDialogueEnd;
    }
}
/* NPC UI starts with !
 * ! is toggled off by starting a dialogue
 * 
 * ? is toggled on when called from Quest
 * ? is toggled off when called from QuestGiver's quest complete check
 */

/*I am thinking to normalize the ChangeQuestStatus method out by switch statement
 * Example: 
 * ChangeQuestStatus(string icon, bool active)
 * {
 *    //You can pass either a "!" or a "?" and true or false to toggle
 * }
 */