/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats 
{
    //When equipment is implemented...use this logic

    private void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if(newItem != null)
        {
            armor.AddModifier(newItem.armorModifier);
        }
        if(oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
        }
    }

}
*/