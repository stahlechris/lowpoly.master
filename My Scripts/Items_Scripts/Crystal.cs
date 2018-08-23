//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using LowPoly.Character;

//public class Crystal : InventoryItemBase
//{
//    [SerializeField] PlayerController playerController;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (this.isActiveAndEnabled && this != null)
//        {
//            hud.OpenMessagePanel(this);
//            playerController.mItemRequestingToBeCollected = this;
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        hud.CloseMessagePanel();
//    }
//}

///*0, 0.4, 0.1
 //* 0, 0, 0
 //* 
 //*/