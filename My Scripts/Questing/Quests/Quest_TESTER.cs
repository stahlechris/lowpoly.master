using UnityEngine;

public class Quest_TESTER : Quest
{
    void OnEnable()
    {
        Quest_ID = QuestCollectionCounter.AssignQuestID();
        QuestType = new QuestType[]
        {
            global::QuestType.CollectionGoal
        };
        QuestGiver_ID = "Bird-0";
        QuestName = "Test Quest";
        QuestDescription = "This is a test for a collection goal";
        QuestStartPoint = null;
        QuestTurnInPoint = transform.parent;
        QuestReward = null;
        QuestStatus = false;
        QuestGoal.Add(new CollectionGoal(this, "Bird","Bird",false,"testItem",0,2));
        QuestGoal.ForEach(g => g.Init());
    }
}
