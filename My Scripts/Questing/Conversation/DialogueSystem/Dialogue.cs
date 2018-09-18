using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using TMPro;
using System;
using UnityEngine.UI;
using LowPoly.Character;
//Dialogue needs to be a prefab instantiated into the npc's UI at runtime, then destroyed when the conversation is over...
//when the player goes to talk to the npc again, a new instantiation (without an update loop) will be made 
public class Dialogue : MonoBehaviour 
{
    //TODO -> murmuring noises like zelda to show conversation happening
    public int numLinesOfDialogue;
    public string dialogueID; 
    public bool assignsQuestWhenOver;
    public TextMeshProUGUI textDisplay;
    public string[] sentence;
    //numLinesOfDialogue,ID,questAssigned == order of txtfile datas before conversation
    public int index = 3;
    public float typingSpeed = 0.25f;
    public bool isTyping = false;
    [SerializeField] Transform friend;
    public bool conversationOver = false;
    public Rigidbody my_rigidBody;
    public Collider my_collider;
    public TextAsset conversation { get; set; }
    public string path { get; set; }
    public bool Initialized { get; set; }
    //public event EventHandler<DialogueEventArgs> OnDialogueStart;
    //public event EventHandler<DialogueEventArgs> OnDialogueEnd;
    bool FacingFriend { get; set; }

    void ConversationConstructor()
    {   
        numLinesOfDialogue = int.Parse(sentence[0]);
        dialogueID = sentence[1];
        assignsQuestWhenOver = bool.Parse(sentence[2]);
    }
    void LoadConversation()
    {
        try
        {
            conversation = Resources.Load<TextAsset>(path);
            //Debug.Log(path);
        }
        catch(NullReferenceException e)
        {
            Debug.Log(e + "Couldn't load " + this.transform + "'s .txt." + path +
                      "Did you assign the txt file in the inspector? AND " +
                      "Manually write out the paths of the conversation in the inspector?");
        }
    }

    void Start()
    {
        path = GetComponentInParent<QuestGiver>().path[0]; //load a conversation initially, but conversations can be changed due to behaving a certain way!!
        my_rigidBody = GetComponentInParent<Rigidbody>();
        my_collider = GetComponentInParent<Collider>();
        LoadConversation();
        sentence = conversation.text.Split('_');
        ConversationConstructor();
    }
    public void SetupNewConversation(string path)
    {
        isTyping = false;
        index = 3;
        this.path = path;
        LoadConversation();
        sentence = conversation.text.Split('_');
        ConversationConstructor();
    }

    void Update()
    {
        if (Initialized) //elimainate a string comparison check in Update()
        {
            if (textDisplay.text == sentence[index])
            {
                if (CrossPlatformInputManager.GetButtonDown("Interact"))
                {
                    Debug.Log("Interact presed and sentence over " + isTyping + " needs to be false to continue");
                    if (!isTyping)
                        NextSentence();
                }
            }
        }
    }

    public void HaveConversation()
    {
        if (!conversationOver)
        {
            Initialized = true;            
            /*Events will return null if 1. nobody is listening to it 
             * 2.somebody is listening but not doing anything in the method
            */
            DialogueEvents.FireAnEvent_OnDialogueStart(this);
            //disable user input..were having a GOD DAMN conversation!
            Global.USER_INPUT_ENABLED = false;
            Global.HAVING_A_CONVERSATION = true;
            textDisplay.text = "";
            textDisplay.enabled = true;
            //must be delayed until player is in a fixed position
            StartCoroutine(FaceMyFriend());
            StartCoroutine(Talk());
        }
        else
        {
            Debug.Log("Conversation is over, nothing to say");
        }
    }
    void ClearText()
    {
        textDisplay.SetText("");
    }
    public IEnumerator Talk()
    {
        yield return new WaitUntil(() => FacingFriend == true);  
        foreach (char letter in sentence[index])
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    void NextSentence()
    {
        isTyping = true;
        if(index < sentence.Length -1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Talk());
            isTyping = false;
        }
        else//conversation complete
        {
            Global.HAVING_A_CONVERSATION = false;
            //my_rigidBody.isKinematic = false;
            Initialized = false;
            conversationOver = true;
            DialogueEvents.FireAnEvent_OnDialogueEnd(this);
            Debug.Log(this + " fired an OnDialogueEnd");
            GetComponentInParent<QuestGiver>().EnableAndAssignMyQuest(assignsQuestWhenOver);


            Debug.Log("Conversation complete, achievement unlocked");//TODO achievement system popup
            PlayerController player = ObjectFinder.PlayerController;
            player.alreadySpeaking = false;
            Global.USER_INPUT_ENABLED = true;
            player.transform.rotation = Quaternion.identity;
            Rigidbody playersRigidBody = player.GetComponent<Rigidbody>();
            playersRigidBody.constraints = RigidbodyConstraints.None;//unfreeze
            playersRigidBody.constraints = RigidbodyConstraints.FreezeRotation;//refreeze to ground
            Invoke("ClearText", 2f);
            textDisplay.enabled = false;
            FacingFriend = false;
            //reset secret dialogue flag so they can load another conversation
            GetComponentInParent<QuestGiver>().hasDisocveredSecretDialogue = false;

            if (dialogueID == "Ip-0")
            {
                NPC_AI ip_AI = GetComponentInParent<NPC_AI>();
                JumpUpAndDown jumpBehavior = GetComponentInParent<JumpUpAndDown>();
                jumpBehavior.enabled = false;
                my_collider.enabled = false;
                Animator animator = GetComponentInParent<Animator>();
                animator.SetBool("run", true);
                ip_AI.GoHome();
                StartCoroutine(EnableKinematicRigidBodyAndCollider());
            }
        }
    }
    IEnumerator EnableKinematicRigidBodyAndCollider()
    {
        yield return new WaitForSeconds(3);
        my_rigidBody.isKinematic = false; //only non-kinematic rigidbodies can trigger OnTriggerEnter()
        my_collider.enabled = true;
    }

    /*
     * ISSUE:
     * Dialogue will always have a different rotation bc lookAt depends on where the camera is looking
     * WHAT WE KNOW:
     * NPC_Canvas is instantiated with same y rotation as parent
     * depending on user selected position of camera when interact, dialogue different
     * FIX?:
     * -manually reset transforms after x amt of time
     */
    IEnumerator FaceMyFriend()
    {
        //trying to simply zero out NPC_canvas(clone) AND TextMeshPro rotation
        //THIS TOOK 2 HOURS TO FIGURE OUT
        //ROTATION VS LOCALROTATION....STILL DONT FULLY UNDERSTAND. FUCK mEEeEEEEeEEeEEEEEEE
        yield return new WaitForSeconds(1.75f);
        //NPC_UI socket needs to be reset as well
        Transform[] parent = GetComponentsInParent<Transform>();
        parent[0].localRotation = Quaternion.Euler(0, 0, 0);
        parent[1].localRotation = Quaternion.Euler(0, -180, 0);
        parent[2].localRotation = Quaternion.Euler(0, 0, 0);
        FacingFriend = true;
    }

}
