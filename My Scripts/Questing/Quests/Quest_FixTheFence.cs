using UnityEngine;

public class Quest_FixTheFence : Quest
{
	void OnEnable () 
    {
        Quest_ID = QuestCollectionCounter.AssignQuestID();
        QuestType = new QuestType[]
        {
            global::QuestType.CollectionGoal,
            global::QuestType.CollectionGoal,
            global::QuestType.ConversationGoal
        };
        QuestGiver_ID = "Ed-0";
        QuestName = "Fix The Fence";
        QuestDescription = "Ed was the one who called you here urgently. " +
            "He knows a computer programmer sure oughtta be able to fix a fence." +
            "Gather the wood and hammer and get it over with so you can get off this island." +
            "Ed said his brother, Ip, usually 'borrows' his hammer. Ip will probably have some wood too.";
        QuestStartPoint = GameObject.Find("Ed").transform;
        QuestTurnInPoint = GameObject.Find("Ed").transform;
        //QuestReward = GameObject.Find("EdHammer");//can choose a weapon off of his weapon rack
        QuestReward = null;
        QuestStatus = false;
        QuestGoal.Add(new CollectionGoal(this, "Ed", "Ed", false, "Wood", 0, 2)); //wood
        QuestGoal.Add(new CollectionGoal(this, "Ed", "Ed", false, "EdHammer", 0, 1));//hammer
        QuestGoal.ForEach(g => g.Init());
    }
}
