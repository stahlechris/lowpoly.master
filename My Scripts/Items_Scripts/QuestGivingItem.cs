using UnityEngine;

public class QuestGivingItem : Item 
{
    internal bool hasUsed;
    [SerializeField] Quest quest;

    public override void OnUse()
    {
        if (!hasUsed)
        {
            QuestList questLog = ObjectFinder.QuestLog;
            questLog.AddQuestItem(quest);
            quest.transform.SetParent(questLog.transform);
            //player.inventory.RemoveItem(this);
            //Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("You already used this item to start its quest");
        }

        //The bird ate a bad mushroom and it killed him
    }
}
