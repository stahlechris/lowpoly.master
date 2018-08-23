using UnityEngine;
using LowPoly.Character;

public class Item : InventoryItemBase
{
    protected PlayerController player = null;

    void OnTriggerStay(Collider other)
    {
        if (this.isActiveAndEnabled && this != null)
        {
            if (player != null)
            {
                hud.OpenMessagePanel(this,itemPickupMessage);
                player.mItemRequestingToBeCollected = this;
            }
            else
            {
                player = other.GetComponent<PlayerController>();
            }

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (player != null)
        {
            hud.CloseMessagePanel();
            player.mItemRequestingToBeCollected = null;
        }
    }
}
