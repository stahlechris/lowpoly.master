using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour 
{
    const string PLAYER_NAME = "Liam";
    public GameObject canvas;
    bool Sqwaaking { get; set; }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == PLAYER_NAME)
        {
            StartCoroutine(ShowDescription());    
        }
    }


    IEnumerator ShowDescription()
    {
        if (!Sqwaaking)
        {
            Sqwaaking = true;
            canvas.SetActive(true);
            yield return new WaitForSeconds(3.5f);
            canvas.SetActive(false);
            Sqwaaking = false;
        }
    }

    /*I want one bird to fly away ...hes guilty, he killed him!
    *Player must keep following this bird to unconver the mystery.
    *Bird flies away 3 times before going up to top of temple.
    *aka conversation goal == complete when with have conversation w/ bird at top of mountain.
    *You get him to confess at the top of the temple.
    *He jumps off to his death in a less than dramatic "cutscene" (bloodsplatter)
    *
    */
}
