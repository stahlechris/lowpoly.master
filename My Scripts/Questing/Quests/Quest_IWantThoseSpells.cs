using UnityEngine;

public class Quest_IWantThoseSpells : Quest
{
    void OnEnable()
    {
        Quest_ID = QuestCollectionCounter.AssignQuestID();
        QuestType = new QuestType[]
        {
            global::QuestType.CollectionGoal,
        };
        QuestGiver_ID = "Eyeguard-0";
        QuestName = "I Want Those Spells!!";
        QuestDescription = "There's gotta be a way around the Eyeguard's tricks...";
        QuestStartPoint = null;
        QuestTurnInPoint = GameObject.Find("Eyeguard").transform;
        QuestReward = null;
        QuestStatus = false;
        QuestGoal.Add(new CollectionGoal(this, null, null, false, "ElementalTome", 0, 4));
        //QuestGoal.Add(new CollectionGoal(this, null, null, false, "EarthTome", 0, 1));
        //QuestGoal.Add(new CollectionGoal(this, null, null, false, "FireTome", 0, 1));
        //QuestGoal.Add(new CollectionGoal(this, null, null, false, "WaterTome", 0, 1));
        QuestGoal.ForEach(g => g.Init());
    }
}
