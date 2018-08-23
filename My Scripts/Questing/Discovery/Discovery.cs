using System.Collections;
using UnityEngine;
public class Discovery : MonoBehaviour 
{
    public string areaDiscoveredName;
    public int questID_AssociatedWithDiscovery;
    bool hasDiscoveredArea;
    bool hasStartedTimer;
    Collider myCollider;
    string playerName = "Liam";

    void Start()
    {
        areaDiscoveredName = this.name;
        myCollider = GetComponent<Collider>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (!hasStartedTimer && other.name == playerName)
        {
            StartCoroutine(StartTimer());
        }
    }
    IEnumerator StartTimer()
    {
        float countDown = 5f;
        hasStartedTimer = true;

        while (countDown > 0)
        {
            yield return new WaitForSeconds(1);
            countDown--;
        }
        DiscoverTheArea();
    }
    void DiscoverTheArea()
    {
        hasDiscoveredArea = true;
        DiscoveryEvents.FireAnEvent_OnDiscovery(this.areaDiscoveredName, this.questID_AssociatedWithDiscovery);
        Terminate();
    }
    void Terminate()
    {
        StopAllCoroutines();
        myCollider.enabled = false;
        Destroy(this.gameObject);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.name == playerName)
        {
            hasStartedTimer = false;
            StopAllCoroutines();
            if (hasDiscoveredArea)
            {
                Terminate();
            }
        }
    }
}
