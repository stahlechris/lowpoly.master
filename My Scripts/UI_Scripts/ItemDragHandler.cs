using UnityEngine;
using UnityEngine.EventSystems;

/*
 * This is a Unity supplied class. 
 * It's job is to follow the mouse cursor around 
 */
public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Transform myTransform;
    private void Start()
    {
        myTransform = transform;
    }
    public InventoryItemBase Item 
    { 
        get; 
        set; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        myTransform.position = Input.mousePosition;
        Debug.Log("Dragging..." + myTransform.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        myTransform.localPosition = Vector3.zero;
    }
}
