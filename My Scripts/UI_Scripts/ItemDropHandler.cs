using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public InventoryList m_Inventory;

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;

        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel,Input.mousePosition))
        {

            InventoryItemBase item = eventData.pointerDrag.gameObject.GetComponent<ItemDragHandler>().Item;
            if (item != null)
            {
                Debug.Log(item +  " isn't null from dropHandler");
                m_Inventory.RemoveItem(item);
                item.OnDrop();
            }
            Debug.Log(item + " is null from drop handler");
        }
    }
}
//WHY DOESN'T THIS WORK