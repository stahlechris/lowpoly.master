using System.Collections;
using UnityEngine;

public class OfferingBowl : MonoBehaviour 
{
    const string ITEM_ONE = "Lamb Leg";
    const string ITEM_TWO = "Space Rock";
    const string ITEM_THREE = "Ancient Flower";
    const string ITEM_FOUR = "Bird_Book";
    const string ITEM_FIVE = "Lamb Leg";

    bool GaveItemOne { get; set; }
    bool GaveItemTwo { get; set; }
    bool GaveItemThree { get; set; }
    bool GaveItemFour { get; set; }
    bool GaveItemFive { get; set; }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.name == ITEM_ONE)
        {
            //destroy the object, set bool flag
            GaveItemOne = true;
            //first object gets you a legendary piece.
        }
        if (collision.transform.name == ITEM_TWO)
        {
            GaveItemTwo = true;
            //do a thing 
        }
        if (collision.transform.name == ITEM_THREE)
        {
            GaveItemThree = true;
            //do a thing 
        }
        if (collision.transform.name == ITEM_FOUR)
        {
            GaveItemFour = true;
            //do a thing 
        }
        if (collision.transform.name == ITEM_FIVE)
        {
            GaveItemFive = true;
            //last object gets you a legendary piece
            //do a thing 
        }
    }

    IEnumerator ReceiveItem()
    {
        yield return new WaitForSeconds(2);
    }
}
