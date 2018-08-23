using UnityEngine;

public class DiscoveryEvents : MonoBehaviour
{
    public delegate void EventHandler<DiscoveryEventArgs>(string discoveryItem, int questID_AssociatedWithDiscovery);
    public static event EventHandler<DiscoveryEventArgs> OnDiscovery;

    public static void FireAnEvent_OnDiscovery(string discoveryItemName, int questID_AssociatedWithDiscovery)
    {
        if (OnDiscovery != null)
        {
            OnDiscovery(discoveryItemName, questID_AssociatedWithDiscovery);
        }
    }
}