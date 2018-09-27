public class Quest_LetEdKnowYoureHere : Quest 
{
    void OnEnable()
    {
        Quest_ID = QuestCollectionCounter.AssignQuestID();
        QuestType = new QuestType[]
        {
            global::QuestType.ConversationGoal
        };
        QuestGiver_ID = "Ip-0";
        QuestName = "Let Ed Know Youre Here";
        QuestDescription = "Find and speak with Ed. " +
            "You've just met Ip, a caretaker of the isle." +
            " Ip said his brother, Ed, was the one who called you " +
            "and he should be on the Templ's right side, next to the herd.";
        QuestStartPoint = transform.parent;
        QuestTurnInPoint = ObjectFinder.EdTransform;
        QuestReward = null;
        QuestStatus = true; //QuestStatus is true, you just need to turn in the Quest
        QuestGoal.Add(new ConversationGoal(this, "Ip","Ed", false, 0, 1, "Ed")); //Completed is false, you need to turn in the Quest to complete it
        QuestGoal.ForEach(g => g.Init());
    }
}
