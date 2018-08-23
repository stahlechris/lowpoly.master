////TODO...REPLACE THIS WITH WEAPON
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;
//using LowPoly.Character;

//public class Axe : InventoryItemBase
//{
//    PlayerController playerController;

//    void Start()
//    {
//        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        Debug.Log(other);
//        if(this.isActiveAndEnabled && this != null)
//        {
//            hud.OpenMessagePanel(this);
//            playerController.mItemRequestingToBeCollected = this;
//        }
//    }

//    void OnTriggerExit(Collider other)
//    {
//        hud.CloseMessagePanel();
//    }
//}


///*0.4, 0.238, 0.05
 //* -167.5, 90, -90
 //* 313, 320, 328
 //*/
