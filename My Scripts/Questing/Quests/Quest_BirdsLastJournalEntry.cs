using UnityEngine;

public class Quest_BirdsLastJournalEntry : Quest
{
    void OnEnable()
    {
        Quest_ID = QuestCollectionCounter.AssignQuestID();
        QuestType = new QuestType[]
        {
            global::QuestType.ConversationGoal
        };
        QuestGiver_ID = "Bird-0";
        QuestName = "Bird's Last Journal Entry";
        QuestDescription = "This journal is bird sized so it's very hard to read anything in it..." + 
            "However the handwriting doesn't look like it was written by a bird..."+
			"It looks like theres some initials M & C...";
        QuestStartPoint = GameObject.Find("Bird").transform;
        QuestTurnInPoint = GameObject.Find("Carl").transform;
        QuestTurnInPoint.GetComponent<QuestGiver>().hasDisocveredSecretDialogue = true;
        QuestReward = null;
        QuestStatus = true;//QuestStatus is true, you just need to turn in the Quest
        QuestGoal.Add(new ConversationGoal(this, "Bird", "Carl", false, 0, 1, "Carl"));
        QuestGoal.ForEach(g => g.Init());
    }
}
