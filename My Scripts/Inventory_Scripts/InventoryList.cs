using System;
using System.Collections;//this was added for a corourtine and might mess up everytin
using System.Collections.Generic;
using UnityEngine;

 //InventoryList stores an IList of Stacks that hold like Items.
public class InventoryList : MonoBehaviour
{
    //The number of items you can carry in game.
    private const int NUMBER_OF_SLOTS = 10;
    private IList<InventorySlotStack> m_ListOfSlotStacks = new List<InventorySlotStack>();

    /* This defines the inventory events that our player can raise.
     * 
     * Example: 
     * I pick up a Gem. 
     * Immidiately after, I tell my secretary, OnItemAdded, that I just added an Item.
     * She says "Ok, fine - which Item did you pick up, sweety?" 
     * I say, "Gem".
     * She says "Ok, I'll let anyone subscribed for ItemAdded updates know!"
     * 
     * Conclusion: 
     * Whenever a Player adds, uses, or removes an item... 
     * he lets his EventHandler know and she will take care of it.
     */
    public event EventHandler<InventoryEventArgs> OnItemAdded;
    public event EventHandler<InventoryEventArgs> OnItemRemoved;
    public event EventHandler<InventoryEventArgs> OnItemUsed;

    //Construct the IList
    public InventoryList()
    {
        //i is the unique ID that references every Stack within the IList of Slots
        for (int i = 0; i < NUMBER_OF_SLOTS; i++)
        {
            //Add an empty Stack to every slot in the inventory.
            m_ListOfSlotStacks.Add(new InventorySlotStack(i));
        }
    }

    private InventorySlotStack FindStackableSlot(InventoryItemBase item)
    {
        //Scan the List to see if and we already have one of these things
        foreach (InventorySlotStack slotStack in m_ListOfSlotStacks)
        {
            if (slotStack.IsStackable(item))
            {
                return slotStack;
            }
        }
        return null;
    }

    private InventorySlotStack FindNextEmptySlot()
    {
        foreach (InventorySlotStack slot in m_ListOfSlotStacks)
        {
            if (slot.IsEmpty) //True if Count is == 0
            {
                return slot;
            }
        }
        return null;
    }

    public void AddItem(InventoryItemBase item)
    {
        Debug.Log("You are adding a " + item);
        if(item.ItemType == ItemType.Key)
        {
            KeyManager.SetKeyActive(item.Name, true);
        }
        if(item.ItemType == ItemType.LegendSwordPiece)
        {
            LegendSwordEvents.FireAnEvent_OnLegendarySwordPieceLooted(item.Name);
        }

        //make a temporary Stack. FindStackableSlot runs 10 times if your NUMBER OF SLOTS is 10.
        InventorySlotStack freeSlot = FindStackableSlot(item);
        if (freeSlot == null)
        {
            Debug.Log("An item you don't currently have has been added to an empty slot ");
            freeSlot = FindNextEmptySlot();
        }
        if (freeSlot != null)
        {
            //Debug.Log("Item added");
            freeSlot.AddItem(item);
            Debug.Log("You have " + freeSlot.Count +" "+ item + "'s");
            if (OnItemAdded != null)
            {
                Debug.Log("I raised an OnItemAdded event to let your HUD know you added an item so it can update your UI.");
                OnItemAdded(this, new InventoryEventArgs(item,false));
            }
        }
    }

 /*This method was placed here for QuestGivers when trying to go into 
 * a players inventory, search for quest item name by string
 * and remove it from the inventory.
 */
    public void SearchInventoryAndRemoveIfFound(string itemName)
    {
        foreach (InventorySlotStack slot in m_ListOfSlotStacks)
        {
            InventoryItemBase temp = slot.RemoveByStringID(itemName);
            if (temp != null)
            {
                if (OnItemRemoved != null)
                {
                    //update delgates item has been removed
                    OnItemRemoved(this, new InventoryEventArgs(temp,true));
                    Debug.Log("Item is being deleted from the game");
                    StartCoroutine(DeleteItemFromGame(temp));
                }
                break;
            }
            else
            {
                Debug.Log("temp was null from searchInventoryAndRemoveIfFound," +
                          " meaning that that the player doesn't have the quest item" +
                          "that the questgiver is trying to take to complete the quest");
            }
        }
    }
    //18/08/08 - only QuestGiver's collecting quest items are calling the 
    //above and below functions. Thus QuestGivers are the only ones 
    //that can delete items from the game entirely upon turning in quest
    public IEnumerator DeleteItemFromGame(InventoryItemBase itemToDelete)
    {
        GameObject goItem = (itemToDelete as MonoBehaviour).gameObject;
        /*4 is a magic number that is arbitrarily set to wait for other event handlers
         * to act upon the OnItemRemoved event before this deletes the object.
         */
        yield return new WaitForSeconds(3);
        Destroy(goItem);
    }

    //return the number of RequiredItems in inventory by string id by the individual item
    public int SearchInventory(string itemName)
    {
        int numItems = 0;
        foreach (InventorySlotStack slot in m_ListOfSlotStacks)
        {
            /*"Look at this slot, see if theres an item with this name, 
             * how many are there with that name? "
            */
            numItems = slot.Find(itemName);
            if(numItems > 0)
            {
                //we have found a RequiredItem
                break;
            }
        }
        return numItems;
    }

    internal void UseItem(InventoryItemBase item)
    {
        if (OnItemUsed != null)
        {
            //Debug.Log("I raised an OnItemUsed event to let your player know you used an item");
            OnItemUsed(this, new InventoryEventArgs(item,false));
        }
    }

    public void RemoveItem(InventoryItemBase item)
    {
        if (item.ItemType == ItemType.Key)
        {
            KeyManager.SetKeyActive(item.Name, false);
        }
        if (item.ItemType == ItemType.LegendSwordPiece)
        {
            LegendSwordEvents.FireAnEvent_OnLegendarySwordPieceDropped(item.Name);
        }

        foreach (InventorySlotStack slot in m_ListOfSlotStacks)
        {
            if (slot.Remove(item))
            {
                if (OnItemRemoved != null)
                {
                    //update delgates item has been removed
                    OnItemRemoved(this, new InventoryEventArgs(item,false));
                }
                break;
            }
        }
    }
}
