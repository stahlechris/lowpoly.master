using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * This is a Unity supplied class. 
 * It's job is to follow the mouse cursor around 
 */
public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{

    public InventoryItemBase Item 
    { 
        get; 
        set; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        Debug.Log("Draggin..." + transform.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
    }
}
