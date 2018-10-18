using System;
/* This is what our secretary, EventHandler, needs to know to inform subscribers.
 * Just tell her the name of the item that is being added, used, or removed.
 * She'll do the rest.
 */
public class InventoryEventArgs : EventArgs
{
    public InventoryItemBase m_Item;
    public bool m_Destroy;
    public InventoryEventArgs(InventoryItemBase item, bool destroy)
    {
        m_Item = item;
        m_Destroy = destroy;
    }
}
