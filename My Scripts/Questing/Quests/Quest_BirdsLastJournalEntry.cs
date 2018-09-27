public class Quest_BirdsLastJournalEntry : Quest
{
    void OnEnable()
    {
        Quest_ID = QuestCollectionCounter.AssignQuestID();
        QuestType = new QuestType[]
        {
            global::QuestType.CollectionGoal
        };
        QuestGiver_ID = "Bird-0";
        QuestName = "Bird's Last Journal Entry";
        QuestDescription = "This journal is bird sized so it's very hard to read anything in it..." + 
            "However the handwriting on this last page doesn't look like it was written by a bird..."+
			"It looks like theres some initials M & C...";
        QuestStartPoint = ObjectFinder.BirdTransform;
        QuestTurnInPoint = ObjectFinder.CarlTransform;
        QuestTurnInPoint.GetComponent<QuestGiver>().hasDisocveredSecretDialogue = true;
        QuestReward = null;
        QuestStatus = false;//change this to false once you figure out the collection goal bug
        QuestGoal.Add(new CollectionGoal(this, "Bird_Book", "Carl", false, "Bird_Book", 0, 1));
    }
}
