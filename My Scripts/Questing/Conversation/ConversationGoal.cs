using UnityEngine;

public class ConversationGoal : QuestGoalBaseClass 
{
    public ConversationGoal()
    {
        /*Why do you have to spell out a default constructor if you want to have
        *an overloaded constructor? default is constructed automatically.
        */
    }
    public ConversationGoal(Quest quest, string questStartPoint, string questTurnInPoint, bool completed, int currAmt, int reqAmt, string questObjectiveObject)
    {
        this.Quest = quest;
        this.QuestStartPoint = questTurnInPoint;
        this.QuestTurnInPoint = questTurnInPoint;
        this.CompletedGoal = completed;
        this.CurrentAmount = currAmt;
        this.RequiredAmount = reqAmt;
        this.QuestObjectiveObject = questObjectiveObject;
    }
    public override bool Evaluate()
    {
        return base.Evaluate();
    }
    public void Complete()
    {
        Quest.CheckGoals();

        CompletedGoal = true;
        Terminate();
    }

    public override void Init()
    {
        base.Init();
        //only listen to events when a quest is Init-tialized
        DialogueEvents.OnDialogueStart += Handle_TestOnDialogueStart;
    }
    void Handle_TestOnDialogueStart(Dialogue dialogueItem)
    {
        Debug.Log("ConversationGoal backend listener heard that a dialogue was started " +
                  "evaluating to see if you complete a quest by speaking to this object");
        string dialogue_ID = dialogueItem.dialogueID;    //"Ip-0"
        string questGiver_ID = dialogue_ID.Split('-')[0];//"Ip"
        Debug.Log(questGiver_ID + " must equalequal " + this.QuestObjectiveObject);
        if(questGiver_ID == this.QuestObjectiveObject)
        {
            //i want the below to display:
            //"0/1 Ed spoken to" -> "1/1 Ed spoken to"
            this.CurrentAmount++;
            //Debug.Log(this.CurrentAmount + " must == " + this.RequiredAmount);
            //base evaluates (currentAmt >= requiredAmt)
            if (Evaluate())
            {
                Complete();
            }

        }
    }
    public override void Terminate()
    {
        base.Terminate();
        //unsubscribe from event, then in GameObject that holds the quest, destroy
        DialogueEvents.OnDialogueStart -= Handle_TestOnDialogueStart;
    }
}
