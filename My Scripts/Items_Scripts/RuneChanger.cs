using System.Collections;
using UnityEngine;

public class RuneChanger : MonoBehaviour 
{
    public GameObject my_gameObject;
    public ParticleSystem ps;
    public Light l;
    public Texture2D[] runes;
    bool runesChangeRepeatedly = true;
    bool runesAreChanging = false;
    float repeatRate = 1.5f;
    bool isInside = true;

    void OnTriggerEnter(Collider other)
    {
        if (my_gameObject.activeInHierarchy)
        {
            isInside = true;
            if (isInside)
            {
                ps.Play();
                runesAreChanging = true;

                if (runesChangeRepeatedly)
                {
                    StartCoroutine(ChangeRunes());
                }
                else
                {
                    //cookie shows a specific rune(image on ground)
                }
            }
        }
    }

    IEnumerator ChangeRunes()
    {
        while (runesAreChanging)
        {
            l.cookie = runes[Random.Range(0, runes.Length)];
            yield return new WaitForSeconds(repeatRate);
        }
    }

    void OnTriggerExit(Collider other)
    {
        isInside = false;
        runesAreChanging = false;
        ps.Stop();
        StopAllCoroutines();
    }
}
