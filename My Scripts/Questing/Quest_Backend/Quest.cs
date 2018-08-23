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
            playersQuestList.RemoveQuestItem(this); 
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
