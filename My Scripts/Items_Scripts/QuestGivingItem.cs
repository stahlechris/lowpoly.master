using UnityEngine;

public class QuestGivingItem : Item 
{
    internal bool hasUsed;
    [SerializeField] Quest quest;
    public DialogueManager dialogueManager;
    public override void OnUse()
    {
        if (!hasUsed)
        {
            hasUsed = true;
            QuestList questLog = ObjectFinder.QuestLog;
            questLog.AddQuestItem(quest);
            quest.transform.SetParent(questLog.transform);

            InformQuestTurnInPoint();
        }
        else
        {
            Debug.Log("You already used this item to start its quest");
        }

        //The bird ate a bad mushroom and it killed him
    }

    void InformQuestTurnInPoint()
    {

        QuestGiver target = quest.QuestTurnInPoint.GetComponent<QuestGiver>();
        if (target.Quest != null) //if the target has a quest to give && the assigning QuestGiver's quest is complete
        {
            string questTurnInPointName = quest.QuestTurnInPoint.name;
            GameObject questTurnInPointGo = quest.QuestTurnInPoint.gameObject;

            //Below will construct the path of the Conversation_NAME-5
            string result = StringHelperClass.ConstructConversationPath(questTurnInPointName);
            //Below will load a QuestGiver's (that is the QuestTurnInPoint of this Quest) Conversation_NAME-5, 
            dialogueManager.SetupNewDialogue(questTurnInPointGo, result);
            //We then need to change the TurnInPoint's QuestGiver's NPC_UI to show a question mark...
            NPC_UI turnInPointNPC_UI = questTurnInPointGo.GetComponentInChildren<NPC_UI>();
            turnInPointNPC_UI.ChangeQuestStatus("?", true);
            Debug.Log("Changed ? to true from QuestGiver");

            //...and update boolean value so he can act correctly
            target.amTurnInPoint = true;
        }

    }
}
