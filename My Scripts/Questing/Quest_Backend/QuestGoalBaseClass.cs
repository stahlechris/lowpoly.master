public class QuestGoalBaseClass 
{
    public string QuestStartPoint { get; set; }
    public string QuestTurnInPoint { get; set; }
    public string QuestObjectiveObject { get; set; }
    public QuestBaseClass Quest;
    public bool CompletedGoal { get; set; }
    public int CurrentAmount { get; set; }
    public int RequiredAmount { get; set; }


    public virtual void Init()
    {
       
    }
    public virtual bool Evaluate()
    {
        CompletedGoal = (CurrentAmount >= RequiredAmount);
        return CompletedGoal;
    }

    public virtual void Terminate()
    {
        
    }
}
