using System.Collections;
using UnityEngine;

public class Quest_ExploreTheIsland : Quest 
{
	void Start() 
    {
        Quest_ID = QuestCollectionCounter.AssignQuestID();
        QuestType = new QuestType[]
        {
            global::QuestType.DiscoveryGoal
        };
        QuestGiver_ID = null;
        QuestName = "Explore The Island";
        QuestDescription = "You've finally arrived to this remote isle after being urgently " +
            "called to help fix a 'very important system'. Explore the island!";
        QuestStartPoint = null;
        QuestTurnInPoint = null;
        QuestReward = null;//You can talk to Pers now.
        QuestStatus = false;
        QuestGoal.Add(new DiscoveryGoal(this, false, 0, 15, "island"));
        QuestGoal.ForEach(g => g.Init());
        StartCoroutine(LateStart());
	}

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(3);
        AssignThisQuest();
    }
    internal void AssignThisQuest()
    {
        playersQuestList.AddQuestItem(this);
        transform.SetParent(playersQuestList.transform);
    }
}
