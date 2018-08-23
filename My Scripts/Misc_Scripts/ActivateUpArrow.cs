using UnityEngine;
using System.Collections;

public class ActivateUpArrow : MonoBehaviour 
{
    public GameObject upArrow;
    public TemporaryFaceTarget tft;
    internal bool hasEntered;
    const string PLAYER_NAME = "Player";
    void OnTriggerEnter(Collider other)
    {
        if(!hasEntered && other.CompareTag(PLAYER_NAME))
        {
            //tell the arrow to start facing the target every fixedUpdate
            tft.enabled = true;
            tft.target = other.transform;
            StartCoroutine(DestroyTimer());
        }
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(20);
        Destroy(upArrow);
        Destroy(this);
    }
}
