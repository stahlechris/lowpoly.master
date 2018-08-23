using UnityEngine;
public class CollectionGoal : QuestGoalBaseClass 
{
    public string RequiredItemName { get; set; }
    public InventoryList inventory;

    public CollectionGoal()
    {
    }
    public CollectionGoal(Quest quest, string questStartPoint, string questTurnInPoint, bool completed, string requiredItemName, int currAmt, int reqAmt)
    {
        //todo make requiedItem an array?
        this.RequiredItemName = requiredItemName;
        this.Quest = quest;
        this.QuestStartPoint = questTurnInPoint;
        this.QuestTurnInPoint = questTurnInPoint;
        this.CompletedGoal = completed;
        this.CurrentAmount = currAmt;
        this.RequiredAmount = reqAmt;
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
        Debug.Log("Init called in CollectionGoal");
        base.Init();
        inventory = GameObject.FindWithTag("Inventory").GetComponent<InventoryList>();
        CheckIfPlayerHasQuestItems();
        Evaluate();
        //CheckIfQuestComplete();
        //Above is crossed because following series of events will attempt to remove a quest that isnt in the quest log because this completes the quest before it even adds it
        //I am thinking we just show that the quest is done, and then talk to the quest giver to complete the quest

        //if the player has ALL quest items, dont start listening
        if (!Quest.QuestStatus)//this is failing, still subscribing
        {
            Debug.Log("This Collection goal needed to start listening to inventory added events " +
                      "because the inventory did not contain all the required quest items after searching");
            inventory.OnItemAdded += Handle_TestOnItemAdded;
            inventory.OnItemRemoved += Handle_TestOnItemRemoved;
        }
    }

    void CheckIfPlayerHasQuestItems()
    {
        //Example. We have 2 baseballs required for this goal, this value should return 2.
        int numReqItemsInInventory = inventory.SearchInventory(RequiredItemName);
        Debug.Log("After searching for quest items, I found you already had " + numReqItemsInInventory + " " + RequiredItemName + "'s");
        //call this.CurrentAmount++ depending on the value of numReqItemsInInventory   
        for (int i = 0; i < numReqItemsInInventory;i++)
        {
            IncrementCurrentAmount();
        }
    }
    void IncrementCurrentAmount()
    {
        this.CurrentAmount++;
    }


    //Both these handler methods are STILL listening after the quest completes

    void Handle_TestOnItemAdded(object sender, InventoryEventArgs e)
    {
        Debug.Log("collection goal heard item looted, checking if quest item");
        
        Debug.Log(e.m_Item.Name + " must equalequal " + this.RequiredItemName);
        if (e.m_Item.Name  == this.RequiredItemName)
        {
            this.CurrentAmount++;
            Debug.Log(this.CurrentAmount + " must == " + this.RequiredAmount);
            Evaluate();
            CheckIfQuestComplete();
        }
    }
    void Handle_TestOnItemRemoved(object sender, InventoryEventArgs e)
    {
        Debug.Log("collection goal heard item dropped, checking if quest item...ignore this if I was called because you turned in quest item");
        Debug.Log(e.m_Item.Name + " must equalequal " + this.RequiredItemName);

        if (e.m_Item.Name == this.RequiredItemName)
        {
            this.CurrentAmount--;
            Evaluate();
            CheckIfQuestComplete();
        }

    }
    public override void Terminate()
    {
        //base.Terminate();
        inventory.OnItemAdded -= Handle_TestOnItemAdded;
        inventory.OnItemRemoved -= Handle_TestOnItemRemoved;
    }
}
