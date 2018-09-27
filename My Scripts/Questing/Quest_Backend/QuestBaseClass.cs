using UnityEngine;
using System.Collections.Generic;

public enum QuestType
{
    Default,
    CollectionGoal,
    KillGoal,
    DiscoveryGoal,
    ConversationGoal,
    DeliveryGoal
}
public class QuestBaseClass : MonoBehaviour 
{
    public QuestList playersQuestList;
    public int Quest_ID { get; set; }
    public QuestType[] QuestType { get; set; }
    public string QuestGiver_ID { get; set; }
    public string QuestName { get; set; }
    public string QuestDescription { get; set; }
    public List<QuestGoalBaseClass> QuestGoal = new List<QuestGoalBaseClass>();
    public Transform QuestStartPoint { get; set; }
    public Transform QuestTurnInPoint { get; set; }
    public GameObject QuestReward { get; set; }
    public bool QuestStatus { get; set; }
    public bool MustTurnInToComplete { get; set; }
    public bool QuestCompletedAndTurnedIn { get; set; }
    public NPC_UI QuestTurnInPointUI { get; set; }

    public virtual void CheckGoals()
    {
        Debug.Log("QUestBaseClass had CheckGoals() called");
        //throw new NotImplementedException();
    }
    //public virtual string GetCurrentAmount()
    //{
    //    Debug.Log("QUestBaseClass had GetCurrentAmount() called");
    //    throw new NotImplementedException();
    //}

    public virtual void GiveReward()
    {
        if(QuestReward != null)
        {
            Debug.Log("Here's your reward!\n LOL it's a Debug.Log() message " + "from base class");
            QuestCompletedAndTurnedIn = true;
        }
    }
}
