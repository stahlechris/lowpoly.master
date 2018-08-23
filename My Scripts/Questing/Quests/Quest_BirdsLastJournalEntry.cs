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
        QuestGiver_ID = "Bird-1";
        QuestName = "Bird's Last Journal Entry";
        QuestDescription = "This journal is bird sized so it's very hard to read anything in it..." + 
            "However the handwriting on the last page doesn't look like it was written by a bird..."+
			"Find the guy with handwriting that looks like this to get some answers!";
        QuestStartPoint = GameObject.Find("Bird").transform;
        QuestTurnInPoint = GameObject.Find("Carl").transform;
        QuestReward = null;
        QuestStatus = true;
        QuestGoal.Add(new ConversationGoal(this, "Bird", "Carl", true, 0, 1, "Carl"));
        QuestGoal.ForEach(g => g.Init());
    }
}
