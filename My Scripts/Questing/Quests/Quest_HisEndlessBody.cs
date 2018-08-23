using UnityEngine;

public class Quest_HisEndlessBody : Quest
{
    void OnEnable()
    {
        Quest_ID = 3;
        QuestType = new QuestType[]
        {
            global::QuestType.CollectionGoal
        };
        QuestGiver_ID = "Carl-0";
        QuestName = "His Endless Body";
        QuestDescription = "Seems Ip's brother, Carl, is having a bad trip. " +
            "Ironically the only thing that can save him is more shrooms." +
            "Carl more or less asked you to find some nearby mushrooms and make" +
            "a detox tea with them using his cauldron. ";
        QuestStartPoint = GameObject.Find("Carl").transform;
        QuestTurnInPoint = GameObject.Find("Carl").transform;
        QuestReward = null;
        QuestStatus = false;
        QuestGoal.Add(new CollectionGoal(this, "Carl", "Carl", false, "Detox Tea", 0, 1));
        QuestGoal.ForEach(g => g.Init());
    }
}

//so you can mess this up and give him more shroom brew and he will be like OOooOo GOD!!!