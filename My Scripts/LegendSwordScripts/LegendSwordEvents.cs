using UnityEngine;

public class LegendSwordEvents : MonoBehaviour
{
    public delegate void EventHandler<LegendSwordEventArgs>(string legendSwordItem);
    public static event EventHandler<LegendSwordEventArgs> OnLegendarySwordPieceLooted;
    public static event EventHandler<LegendSwordEventArgs> OnLegendarySwordPieceDropped;

    public static void FireAnEvent_OnLegendarySwordPieceLooted(string legendSwordItem)
    {
        if (OnLegendarySwordPieceLooted != null)
        {
            OnLegendarySwordPieceLooted(legendSwordItem);
        }
        else
        {
            Debug.Log("null start LegendSwordEvent event");
        }
    }
    public static void FireAnEvent_OnLegendarySwordPieceDropped(string legendSwordItem)
    {
        if (OnLegendarySwordPieceDropped != null)
        {
            OnLegendarySwordPieceDropped(legendSwordItem);
        }
        else
        {
            Debug.Log("null end LegendSwordEvent  of " + legendSwordItem);

        }
    }

}