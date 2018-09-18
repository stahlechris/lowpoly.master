using UnityEngine;

public class EnableNearbyArea : MonoBehaviour 
{
    [SerializeField] GameObject area;

    const string PLAYER_TAG = "Player";
    bool Entered { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (!Entered && other.CompareTag(PLAYER_TAG))
        {
            Entered = true;
            area.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(Entered && other.CompareTag(PLAYER_TAG))
        {
            Entered = false;
        }
    }
}
