public class DiscoveryGoal : QuestGoalBaseClass
{
    const int numDiscoveries = 14;
    public string[] DiscoveredArea = new string[numDiscoveries];

    public DiscoveryGoal()
    {
        //I don't understand why the compiler can't generate an empty default constructor,
        //but hey when you can't be bothered, you can't be bothered.
    }
    public DiscoveryGoal(Quest quest, bool completed, int currAmt, int reqAmt, string questObjectiveObject)
    {
        this.Quest = quest;
        this.CompletedGoal = completed;
        this.CurrentAmount = currAmt;
        this.RequiredAmount = reqAmt;
        this.QuestObjectiveObject = questObjectiveObject;
    }

    public override bool Evaluate()
    {
        return base.Evaluate();
    }
    public void CheckIfQuestComplete()
    {
        Quest.CheckGoals();
    }

    public override void Init()
    {
        base.Init();
        DiscoveryEvents.OnDiscovery += Handle_OnDiscovery; //only listen to broadcaster when we are initialized
    }
    void Handle_OnDiscovery(string areaDiscoveredName, int questID_AssociatedWithDiscovery)
    {
        //add incoming areaName to our DiscoveredAreas
        for (int i = 0; i < numDiscoveries; i++)
        {
            if (DiscoveredArea[i] == null)
            {
                DiscoveredArea[i] = areaDiscoveredName;
                break;
            }
        }
        this.CurrentAmount++;
        //Evalute() checks if Completed = (currAmt == reqAmt) on a single Goal
        Evaluate();
        //CheckIfQuestComplete() checks ALL Goals CompletedGoal value
        CheckIfQuestComplete();
    }
    public override void Terminate()
    {
        base.Terminate();
        DiscoveryEvents.OnDiscovery -= Handle_OnDiscovery;
    }

}
