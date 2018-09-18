using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HUD : MonoBehaviour
{
    bool m_IsMessagePanelOpened = false;
    public GameObject inventoryPanel;
    public InventoryList inventory;
    public QuestList quests;
    public GameObject MessagePanel;
    public TextMeshProUGUI questText;
    public GameObject questCanvas;
    public Transform[] questCanvasChildren;
    public GameObject discoveryPanel;
    public TextMeshProUGUI discoveryText;
    public int childIndex = 1;

    void Start()
    {   //For button press noises
        //UI_AudioSource = GetComponent<AudioSource>();
        questCanvasChildren = questCanvas.GetComponentsInChildren<Transform>(); 
        questText = GetComponentInChildren<TextMeshProUGUI>();
        //subscribe to events
        inventory.OnItemAdded += Inventory_ItemAdded;
        inventory.OnItemRemoved += Inventory_ItemRemoved;
        quests.OnQuestAdded += Quest_OnQuestAdded;
        quests.OnQuestRemoved += Quest_OnQuestRemoved;
        DialogueEvents.OnDialogueStart += Dialogue_OnDialogueStart;
        DialogueEvents.OnDialogueEnd += Dialogue_OnDialogueEnd;
        DiscoveryEvents.OnDiscovery += Handle_OnDiscovery; //todo consider this being non- static...i dont think it needs to be
    }
#region INVENTORY EVENT HANDLERS
    void Inventory_ItemAdded(object sender, InventoryEventArgs e)
    {
        //Debug.Log("Received item added event. Updating HUD");
        Transform inventoryPanel = transform.Find("InventoryPanel");
        int index = -1;
        foreach (Transform slot in inventoryPanel)
        {
            index++;

            // Border-> Image, Text.
            Transform imageTransform = slot.GetChild(0).GetChild(0);//item image
            Transform textTransform = slot.GetChild(0).GetChild(1);//item count
            Image image = imageTransform.GetComponent<Image>();
            Text txtCount = textTransform.GetComponent<Text>();

            ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();

            //each item is stacked in a Slot's Stack and can be referenced by an Id.
            //We can use Count() to see how many things are in that particular Slot's Stack
            if (index == e.m_Item.Slot.Id)
            {
                //Debug.Log(index + "e.m_Item.Slot.Id " + e.m_Item.Slot.Id);
                int itemCount = e.m_Item.Slot.Count;

                image.enabled = true;
                image.sprite = e.m_Item.image;

                if (itemCount > 1)
                    txtCount.text = itemCount.ToString();
                else
                    txtCount.text = "";

                itemDragHandler.Item = e.m_Item;

                break;
            }//else go to next transform in inventoryPanel
        }
    }

    void Inventory_ItemRemoved(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("InventoryPanel");
        int index = -1;

        foreach (Transform slot in inventoryPanel)
        {
            index++;

            Transform imageTransform = slot.GetChild(0).GetChild(0);
            Transform textTransform = slot.GetChild(0).GetChild(1);
            Image image = imageTransform.GetComponent<Image>();
            Text txtCount = textTransform.GetComponent<Text>();

            ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();

            // We found the item in the UI
            if (itemDragHandler.Item == null)
                continue;

            // Found the slot to remove from
            if (e.m_Item.Slot.Id == index)
            {
                int itemCount = e.m_Item.Slot.Count;
                itemDragHandler.Item = e.m_Item.Slot.FirstItem;

                if (itemCount < 2)
                {
                    txtCount.text = "";
                }
                else
                {
                    txtCount.text = itemCount.ToString();
                }

                if (itemCount == 0)
                {
                    image.enabled = false;
                    image.sprite = null;
                }
                break;
            }
        }
    }
#endregion

    #region QUEST METHODS
    void Quest_OnQuestAdded(object sender, QuestEventArgs e)
    {
        //UI fx to make it glow a little before dissapearing
        WriteQuestOnHUD(e.m_quest);
        InformQuestCanvas(e.m_quest);
        FlashText();//todo send the quest so we can flash the individual one
    }

    void Quest_OnQuestRemoved(object sender, QuestEventArgs e)
    {
        FlashText();
        CrossOffQuestFromList(e.m_quest);
        //TODO: quests need to be removed and all shifted upwards by one
    }

    void WriteQuestOnHUD(QuestBaseClass e)
    {
        string questName = e.QuestName;
        //Debug.Log("Hud rcvd " + questName);
        //EACH QUEST IS ALWAYS 42 CHARACTERS LONG
        questText.text += StringHelperClass.PadQuestTextToLengthOfLine(questName);
    }

    void InformQuestCanvas(QuestBaseClass e)
    {
        questCanvasChildren[++childIndex].GetComponent<ShowQuestDetails>().SetQuestToPanel(e);
    }
    void CrossOffQuestFromList(QuestBaseClass e)
    {
        string currentQuestText = questText.text;

        string questName = e.QuestName;
        int questId = e.Quest_ID;

        //find our questName within the currentQuestText..return the starting char's index 
        int startingCharIndex = currentQuestText.IndexOf(questName, System.StringComparison.CurrentCulture);
        //Debug.Log(startingCharIndex);

        //add strikethrough starting tag to beginning and an ending strikethrough tag at end
        questText.text = questText.text.Insert(startingCharIndex, "<s>");
        questText.text = questText.text.Insert(startingCharIndex + 41, "</s>");

    }
    IEnumerator FlashQuestText()
    {
        float fadeOutTimeInSec = 2f;
        TextMeshProUGUI questTextLocal = questText;
        Color originalColor = questText.color;

        for (float t = 0.01f; t < fadeOutTimeInSec; t += Time.deltaTime)
        {
            questTextLocal.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTimeInSec));
            yield return null;
            questTextLocal.color = Color.Lerp(originalColor, Color.white, Mathf.Min(1, t / fadeOutTimeInSec));
        }
    }

    public void FlashText()
    {
        StartCoroutine(FlashQuestText());
    }

    //This method is here because it is  simple and easy to match a quest by ID and it could be used in the future
    //void UpdateQuestDescription(QuestBaseClass e) //you want to call this whenever anything happens in a quest, discovery, kill, collection, etc, etc....bc pass new reference
    //{
    //    foreach (Transform panel in questCanvasChildren)
    //    {
    //        if(panel.GetComponent<ShowQuestDetails>().quest_id == e.Quest_ID)
    //        {
    //            panel.GetComponent<ShowQuestDetails>().SetQuestToPanel(e);
    //        }
    //    }
    //}


    void Handle_OnDiscovery(string areaDiscoveredName, int questID_AssociatedWithDiscovery)
    {
        //Debug.Log("you just discovered " + areaDiscoveredName + "for the questID " + questID_AssociatedWithDiscovery);
        StartCoroutine(ShowDiscoveryBanner(areaDiscoveredName));
        //UpdateQuestDescription(questID_AssociatedWithDiscovery);
        //UpdateDiscoveryQuestStatus(areaDiscoveredName);
        //this will write the areaDiscoveredName on the questDescription

    }

    IEnumerator ShowDiscoveryBanner(string text)
    {
        //activate the DiscoveryPanelImage
        discoveryPanel.SetActive(true);
        //set the text in the DiscoveryText (must occur on an active GO & parent)
        discoveryText.SetText(text);
        //Show the banner for x seconds.
        yield return new WaitForSeconds(4);
        //clear the text
        discoveryText.SetText("");
        //deactivate the DiscoveryPanelImage
        discoveryPanel.SetActive(false);
    }
#endregion QUEST METHODS
    #region Message Panel Popup Bar 

    public bool IsMessagePanelOpened
    {
        get
        {
            return m_IsMessagePanelOpened;
        }
    }
    //use this method for inventory
    public void OpenMessagePanel(InventoryItemBase item, string message)
    {
        MessagePanel.SetActive(true);

        Text mpText = MessagePanel.transform.Find("TextMessage").GetComponent<Text>();
        if(message != null)
        {
            mpText.text = message;
        }
        else
        {
            mpText.text = "- Press E to Pickup -";
        }
        m_IsMessagePanelOpened = true;
    }
    public void OpenMessagePanel(GameObject item, string message)
    {
        MessagePanel.SetActive(true);

        Text mpText = MessagePanel.transform.Find("TextMessage").GetComponent<Text>();
        if(message == null)
        {
            mpText.text = "- Press E to Talk -";
        }
        else
        {
            mpText.text = message;
        }
        m_IsMessagePanelOpened = true;
    }

    public void CloseMessagePanel()
    {
        MessagePanel.SetActive(false);

        m_IsMessagePanelOpened = false;
    }
    #endregion Message Panel Popup Bar

    #region Dialogue Hides Inventory Panel
    void Dialogue_OnDialogueStart(Dialogue d)
    {
        //keep an eye on this...inventory will get fucked if you loot and talk at the same time?
        inventoryPanel.SetActive(false);
    }
    void Dialogue_OnDialogueEnd(Dialogue d)
    {
        inventoryPanel.SetActive(true);
    }


    #endregion
    void OnDisable()
    {
        DialogueEvents.OnDialogueStart -= Dialogue_OnDialogueStart;
        DialogueEvents.OnDialogueEnd -= Dialogue_OnDialogueEnd;
        DiscoveryEvents.OnDiscovery -= Handle_OnDiscovery;
    }
}
