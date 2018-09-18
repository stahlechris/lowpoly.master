using UnityEngine;

public class Quest_HisEndlessBody : Quest
{
    void OnEnable()
    {
        GameObject.FindWithTag("Cauldron").GetComponent<Cauldron>().CanCook = true;
        Quest_ID = QuestCollectionCounter.AssignQuestID();
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
        QuestStartPoint = transform.parent;
        QuestTurnInPoint = transform.parent;
        QuestReward = null;
        QuestStatus = false;
        QuestGoal.Add(new CollectionGoal(this, "Carl", "Carl", false, "Eternity Potion", 0, 1));
        QuestGoal.ForEach(g => g.Init());
    }
}

//so you can mess this up and give him more shroom brew and he will be like OOooOo GOD!!!