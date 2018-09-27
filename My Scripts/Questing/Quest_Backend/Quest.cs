using System.Linq;
using UnityEngine;

public class Quest : QuestBaseClass //todo why does this class exist, consider choosing between quest and questbaseclass
{
    public override void CheckGoals()
    {
        //checks ALL Goals' Completed properties value; 
        QuestStatus = QuestGoal.All(g => g.CompletedGoal);
        if (QuestStatus)
        {
            Debug.Log(this + " backend evaluated Goals as CompletedGoals == true from CheckGoals()");
            InformQuestGiverOfQuestStatus();

            //TODO we want the quest giver to call this after turning in quest
            //then we don't have to pass a reference to the playersQuestList on every quest
            Debug.Log("Quest removed " + this.name + "This will cross off the quest via an OnQuestRemoved event");
            playersQuestList.RemoveQuestItem(this); //Here we remove the Quest from the QuestList, but there are still references to it in the UI. => TODO: decide if we want to keep the references to display "0/1 completed". Or we want to delete the references and just show "Completed"
        
        //TODO setp24. We are trying to not cross off the quest until it is turned in. Right now, when you collect all the mats, it crosses off before you can turn it in.
        }
    }

    /*TODO - So you speak with the person, they turn off their icon,
     * then it comes here, which turns on the icon IMMIDIATELY afterwards
     */
    public void InformQuestGiverOfQuestStatus()
    {
        Debug.Log(this + " backend informed QuestGiver of quest status");
        NPC_UI questGiver_UI;

        questGiver_UI = this.QuestTurnInPoint.GetComponentInChildren<NPC_UI>();
        questGiver_UI.ChangeQuestStatus("?",true);
        Debug.Log(this + " backend changed ? to true");
    }
}
