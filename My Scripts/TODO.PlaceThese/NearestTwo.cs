using UnityEngine;

public class NearestTwo : MonoBehaviour 
{
    EndlessPlatformSystem manager;

    bool Entered { get; set; }
    public BoxCollider triggerCollider;

    const string PLAYER_TAG = "Player";
    private void Start()
    {
        triggerCollider = GetComponent<BoxCollider>();
        manager = ObjectFinder.EndlessPlatformSystem;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if(!Entered && other.CompareTag(PLAYER_TAG))
        {
            //Don't let the player trigger it twice.
            Entered = true;
            //Tell the manager a player just entered our trigger 
            //We need to wait here until the manager says its ok to inform him again
            manager.Inform();
            triggerCollider.enabled = false;
        }
    }

    public void Reset()
    {
        Entered = false;
        triggerCollider.center = triggerCollider.center + new Vector3(10,0,0);
        triggerCollider.enabled = true;
    }
}
