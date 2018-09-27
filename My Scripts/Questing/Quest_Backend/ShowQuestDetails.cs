using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ShowQuestDetails : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject questDescriptionPanel;
    public static TextMeshProUGUI descriptionText;
    public QuestBaseClass quest;
    bool active;
    bool questComplete { get; set; }

    public void SetQuestToPanel(QuestBaseClass e)
    {
        this.quest = e;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData != null)
        {
            if (quest != null)
            {
                active = true;

                ActivateQuestDescription();
                ConstructQuestDescription(quest);
            }
        }
    }
    void ActivateQuestDescription()
    {
        questDescriptionPanel.SetActive(active);
    }
    void ConstructQuestDescription(QuestBaseClass updatedQuest)
    {
        if (updatedQuest != null)//this is NOT redundant, it is needed here because computers are weird and choose when they want to do something in order i guess???
        {
            int index = -1;
            int[] currAmt = new int[20];//assumed you wont have a goal over that needs you to interact 20 times
            int[] reqAmt = new int[20];
            string currVsReq = "<br><br>";
            string lastQuestWord = StringHelperClass.DetermineLastQuestWord(updatedQuest.QuestType.ToString());

            if (descriptionText == null)
            {
                descriptionText = questDescriptionPanel.GetComponentInChildren<TextMeshProUGUI>();
            }

            if (active)
            {
                foreach (QuestGoalBaseClass q in updatedQuest.QuestGoal)//May you always remember that time you freaked out about reference vs value and really it was just a problem of "++"
                {                                                       //sept 24 - i am paying my respects to ^ this ^ comment. My god, I really did freak out.
                    if (q != null)
                    {
                        currAmt[++index] = q.CurrentAmount;
                        reqAmt[index] = q.RequiredAmount;

                        currVsReq += currAmt[index] + " / " + reqAmt[index] + " " + q.QuestObjectiveObject;
                    }
                }
                if (!updatedQuest.QuestCompletedAndTurnedIn)
                {
                    descriptionText.SetText(updatedQuest.QuestDescription + currVsReq + lastQuestWord + "<br>"); //use html tags here bc we are inputting into a text thingy that eats those instead of "\n"
                }
                else
                {
                    descriptionText.SetText("COMPLETED!");
                }
            }
            if (!active)
            {
                descriptionText.SetText("");
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        active = false;

        ActivateQuestDescription();
        ConstructQuestDescription(quest);
    }
}
