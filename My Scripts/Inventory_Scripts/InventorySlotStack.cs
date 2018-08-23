using System.Collections.Generic;

public class InventorySlotStack
{
    /*
     * This class defines a Stack. 
     * Each InventorySlot implements a Stack.
     * Each InventorySlot is stored in an iList.   
     * 
     *  1  2  3  4  5                
     *  [] [] [] [] []     ^ s
     *  [] [] [] [] []     | t
     *  [] [] [] [] []     | a 
     *  [] [] [] [] []     | c
     *  [] [] [] [] []     | k 
     * [----iList----->]   | s 
     *                      
     */
    private Stack<InventoryItemBase> m_ItemStack = new Stack<InventoryItemBase>();
    private int m_Id = 0;
    //You need an ID to create and reference every Stack in an Inventory Slot 
    public InventorySlotStack(int id)
    {
        m_Id = id;
    }

    public int Id
    {
        get
        { 
            return m_Id; 
        }
    }

    public InventoryItemBase FirstItem
    {
        get
        {
            if (IsEmpty)
            {
                return null;
            }

            return m_ItemStack.Peek();
        }
    }

    public bool IsEmpty
    {
        get
        {
            return Count == 0;
        }
    }

    public int Count
    {
        get
        {
            return m_ItemStack.Count;
        }
    }
    public int Find(string itemName)
    {
        if(IsEmpty)
        {
            return 0;
        }

        int numItems = 0;

        //peek returns the first item in the stack without popping it
        InventoryItemBase first = m_ItemStack.Peek();

        if(first.Name == itemName)
        {
            //get the count of how many items there are of that name
            numItems = Count;
        }

        return numItems;
    }
    public void AddItem(InventoryItemBase item)
    {
        item.Slot = this;
        m_ItemStack.Push(item);
    }

    public bool IsStackable(InventoryItemBase item)
    {
        if (IsEmpty)
        {
            return false;
        }
        //peek returns the first item in the stack without popping it
        InventoryItemBase first = m_ItemStack.Peek();

        if (first.Name == item.Name)
        {
            return true;
        }

        return false;
    }

    public bool Remove(InventoryItemBase item)
    {
        if (IsEmpty)
        {
            return false;
        }

        InventoryItemBase first = m_ItemStack.Peek();

        if (first.Name == item.Name)
        {
            m_ItemStack.Pop();
            return true;
        }

        return false;
    }

    /*This method was placed here for QuestGivers when trying to go into 
     * a players inventory, search for quest item name by string
     * and remove it from the inventory.
     */
    public InventoryItemBase RemoveByStringID(string itemName)
    {
        if (IsEmpty)
        {
            return null;
        }

        InventoryItemBase first = m_ItemStack.Peek();

        if (first.Name == itemName)
        {
            m_ItemStack.Pop();
            return first;
        }

        return null;
    }

} 
