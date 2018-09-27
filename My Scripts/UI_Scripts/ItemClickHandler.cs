using UnityEngine;
using UnityEngine.UI;

/*
 * TODO -
 * FIX DRAG AND DROP PROBLEM & character movement affecting inventory UI
 * 
 * WHAT'S THE PROBLEM - 
 * DRAGGING AN ITEM BACK INTO THE SCENE FROM THE INVENTORY PANEL BROKEN.
 * 
 * IF SLOT IS SELECTED (BLUE), DRAG & DROP WONT WORK.
 * IF SLOT IS NOT SELECTED(NOT HIGHLIGHTED BLUE), DRAG & DROP WILL WORK (its impossible to have a slot not selected when trying to drag and drop)
 * 
 * -MUST FIGURE OUT WHY CHARACTER MOVEMENT IS AFFECTING INVENTORY 
 * 
 */
public class ItemClickHandler : MonoBehaviour
{
    public InventoryList m_Inventory;
    public KeyCode m_Key; //assignable hotkeys

    private Button m_Button;
    InventoryItemBase AttachedItem
    {
        get
        {
            ItemDragHandler dragHandler =
                transform.GetComponentInChildren<ItemDragHandler>();
            //            Debug.Log("ItemClickHandler tried to make an ItemDraghandler. got icon to drag" + dragHandler);
            return dragHandler.Item;
        }
    }

    void Awake()
    {
        m_Button = GetComponent<Button>();
    }
    void Update()
    {
        if (Input.GetKeyDown(m_Key))
        {
            FadeToColor(m_Button.colors.pressedColor);

            // "Click the button" with hotkey press
            m_Button.onClick.Invoke();
        }
        else if (Input.GetKeyUp(m_Key))
        {
            FadeToColor(m_Button.colors.normalColor);
        }
    }
    void FadeToColor(Color color)
    {
        Graphic graphic = GetComponent<Graphic>();
        graphic.CrossFadeColor(color, m_Button.colors.fadeDuration, true, true);
    }


    public void OnItemClicked()
    {
        //todo...stay highlighted or selected if in hand
        m_Button.Select();
        m_Button.OnSelect(null);
        InventoryItemBase item = AttachedItem;

        if (item != null)
        {
            m_Inventory.UseItem(item);
            /*OnUse() sets a predefined behvaior, 
               like transforming the position and rotation of item 
               so it fits in Player's hand.
            */
            item.OnUse();
        }
    }
}
