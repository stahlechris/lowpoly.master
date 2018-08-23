using UnityEngine;
using System.Linq;

public class QuestGiver : NPC_Behaviors 
{
    [SerializeField] string questGiver_ID; 
    public QuestBaseClass Quest;
    public NPC_UI questGiver_UI;
    public QuestList playersQuestLog;
    bool AssignedQuest { get; set; }
    bool PlayerCompletedMyTask { get; set; }
    public Dialogue dialogue;
    public DialogueManager dialogueManager;
    //QuestGiver can have 6 different types of convo (set this arbitrarily higher than u think)
    public static int numConversations = 10;
    public string[] path = new string[numConversations];
    public bool hasDisocveredSecretDialogue = false;
    public bool amTurnInPoint = false;
    public bool amNeededInOtherQuest = false;
    public bool alreadyLoadedThisDialogue = false;


    public override void Interact()
    {
        Debug.Log("Interact()-QuestGiver- from top of method before making a decision");
        if(amNeededInOtherQuest)
        { 
            //UNDER CONSTRUCTION
            Debug.Log("I am acknowledging i am needed in another quest");
        }
        //If I am the QuestTurnInPoint of another QuestGiver's Quest and the Quest is Completed...
        else if(amTurnInPoint)
        {
            Debug.Log("I am acknowledging i am the turn in point of another quest");
            //Have conversation to acknowledge that (will drop out to base.Interact)
        }

        //Then choose between the below choices

        // if (the player has discovered a secret and loaded a new dialogue by doing so.
        else if( hasDisocveredSecretDialogue == true)
        {
            Debug.Log("You have discovered a secret dialog");
        }
        //I have not assigned a quest and i have a quest to give
        else if ( !AssignedQuest && !PlayerCompletedMyTask)
        {
            //Possibly a redundant load so this behavior can stay in this class and not in NPC_UI
            dialogueManager.SetupNewDialogue(this.gameObject, path[0]);
            Debug.Log("I have not assigned you a quest and you have not completed the quest, but i have a quest to give");
        }
        // I have assigned A quest and need to check if hes done with it
        else if ( AssignedQuest && !PlayerCompletedMyTask)
        {
            Debug.Log("I have assigned a quest and need to check if you are finished");
            CheckIfPlayerCompletedMyQuest();
        }
        else //The player has talked to me when i have no quests to give and he has completed my quests
        {
            Debug.Log("You have talked to me after i have no quests to give and you have completed my quests");
            dialogueManager.SetupNewDialogue(this.gameObject, path[4]);
        }
        base.Interact();
    }
    //Dialogue calls this because you must have a "conversation" (read some text) with any object before it assigns you a quest.
    public void EnableAndAssignMyQuest(bool assignsQuest)
    {
        if (assignsQuest && Quest != null)
        {
            Quest.enabled = true;
            AssignedQuest = true;
            Debug.Log("Questgiver attempting to add quest to log");
            playersQuestLog.AddQuestItem(Quest);
            Quest.transform.SetParent(playersQuestLog.transform);

            /*Only inform the quest turn in point to update itself if : 
             * 1.) It isn't going to be destroyed immidiately afterwards...like the test bird
             * 2.) It has a Quest to assign (indicated by a "!") 
             */
            InformQuestTurnInPoint();

        }
        else
        {
            Debug.Log("I don't have a quest to assign you after speaking to me " + this);
        }
    }

    /*All Conversation_NAME-5's are to load a conversation that acknowledges 
     * that it is the Turn-in point of a Quest. Thus, you must turn in a Quest
     * before you can start a Quest on the same QuestGiver.
     */
    void InformQuestTurnInPoint()
    {
        if (Quest.QuestTurnInPoint.name != "Bird") //the bird is special for testing purposes
        {
            QuestGiver target = Quest.QuestTurnInPoint.GetComponent<QuestGiver>();
            if (target.Quest != null && this.Quest.QuestStatus) //if the target has a quest to give && the assigning QuestGiver's quest is complete
            {
                string questTurnInPointName = Quest.QuestTurnInPoint.name;
                GameObject questTurnInPointGo = Quest.QuestTurnInPoint.gameObject;

                //Below will construct the path of the Conversation_NAME-5
                string result = StringHelperClass.ConstructConversationPath(questTurnInPointName);
                //Below will load a QuestGiver's (that is the QuestTurnInPoint of this Quest) 
                //Conversation_NAME-5, 
                dialogueManager.SetupNewDialogue(questTurnInPointGo, result);
                //We then need to change the TurnInPoint's QuestGiver's NPC_UI to show a question mark
                NPC_UI turnInPointNPC_UI = questTurnInPointGo.GetComponentInChildren<NPC_UI>();
                turnInPointNPC_UI.ChangeQuestStatus("?", true);
                Debug.Log("Changed ? to true from QuestGiver");

                //And update boolean value so he can act correctly
                target.amTurnInPoint = true;
            }
        }
    }

    void InformNeededInOtherQuest()
    {
        
    }

    void CheckIfPlayerCompletedMyQuest()
    {
        Quest.CheckGoals();
        if (Quest.QuestStatus)
        {
            questGiver_UI.ChangeQuestStatus("?", false);
            Debug.Log("Changed ? to false from QuestGiver");

            if (Quest.QuestType.Contains(QuestType.CollectionGoal))
            {
                TakeQuestItemsFromInventory();
            }
                Debug.Log("questgiver completed quest method,");
                dialogueManager.SetupNewDialogue(this.gameObject, path[1]);
                Debug.Log("questgiver loaded new dialogue");
                Quest.GiveReward();
                Quest.QuestGoal.ForEach(g => g.Terminate());
                PlayerCompletedMyTask = true;
                AssignedQuest = false;
                Quest = null; //reset the current quest,
                              //check if I have another quest to assign
                              //put that new Quest into the variable, or don't depending
        }
        else
        {//load assignedQuest=true but has not completed it yet text
            Debug.Log("i have given you a quest, but you have not yet completed it");
            dialogueManager.SetupNewDialogue(this.gameObject, path[3]);
        }
    }

    //under constunction...this method DELETES items from the game
    void TakeQuestItemsFromInventory()
    {
        Debug.Log("taking quests from inventory to complete collection quest");
        //OnRequestQuestItems?
        InventoryList playersInventory = GameObject.FindWithTag("Inventory").GetComponent<InventoryList>();
        //get all QuestGoals of type CollectionGoal
        foreach(CollectionGoal c in Quest.QuestGoal)
        {
            //fetch the RequiredAmount to see how many to remove
            int reqNumItemsToRemove = c.RequiredAmount;
            //fetch the RequiredItemName and attempt to remove it
            string questItemName = c.RequiredItemName;
            for (int i = 0; i < reqNumItemsToRemove;i++)
            {
                playersInventory.SearchInventoryAndRemoveIfFound(questItemName);
            }
        }

    }
}
