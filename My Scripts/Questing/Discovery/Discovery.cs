using System.Collections;
using UnityEngine;

public class Discovery : MonoBehaviour 
{
    //The culling system taps into the Discovery System's colliders around all the areas of the map.
    [SerializeField]CullingSystem cullingSystem;

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
            Debug.Log("Liam entered  " + this);
            cullingSystem.InformOnTrigger(true, this.areaDiscoveredName);

            if (!hasDiscoveredArea)
            {
                StartCoroutine(StartTimer());
            }
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


    //Update this so it doesn't destroy it...but has Culling System share the colliders 
    void Terminate()
    {
        StopAllCoroutines();
        //myCollider.enabled = false;
        //Destroy(this.gameObject);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.name == playerName )
        {
            cullingSystem.InformOnTrigger(false, this.areaDiscoveredName);
            Debug.Log("Liam left " + this);
            hasStartedTimer = false;
            StopAllCoroutines();
            if (hasDiscoveredArea)
            {
                Terminate();
            }
        }
    }
}
