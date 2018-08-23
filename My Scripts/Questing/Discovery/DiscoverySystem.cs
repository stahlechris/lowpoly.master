using UnityEngine;

public class DiscoverySystem : MonoBehaviour 
{
    AudioSource my_audioSource;

	void Start ()
    {
        my_audioSource = GetComponent<AudioSource>();
        DiscoveryEvents.OnDiscovery += Handle_DiscoveryEvent;
    }

    void Handle_DiscoveryEvent(string areaDiscoveredName, int questID_AssociatedWithDiscovery)
    {
        my_audioSource.Play();
    }

    void OnDisable()
    {
        //It is VERY important to unsubscibe to static events!!
        DiscoveryEvents.OnDiscovery -= Handle_DiscoveryEvent;
    }
}
