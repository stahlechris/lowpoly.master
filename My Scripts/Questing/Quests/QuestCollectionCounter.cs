/* This class increments Quest_ID in individual Quests.
 * Usedee which quests the player picked up in order.
 */
public static class QuestCollectionCounter 
{
    static int incrementer = -1;

    public static int AssignQuestID()
    {
        return incrementer++;
    }
}
