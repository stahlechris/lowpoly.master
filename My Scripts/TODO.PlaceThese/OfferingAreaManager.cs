using System.Collections;
using UnityEngine;

/*This class is responsible for managing the UI, light and the offering bowl.
 * 
 * 
 */
public class OfferingAreaManager : MonoBehaviour 
{

    AudioSource audioSource;

    [SerializeField] GameObject lightCylinder;
    [SerializeField] AudioClip itemDoesntMatchClip;
    [SerializeField] AudioClip itemMatchesClip;
    [SerializeField] AudioClip lightOnClip;
    [SerializeField] AudioClip beamClip;
    [SerializeField] AudioClip rewardItemClip;

    OfferingBowl offeringBowl;
    OfferingRock offeringRock;
    public bool StartedSequence { get; set; }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        offeringBowl = GetComponentInChildren<OfferingBowl>();
        offeringRock = GetComponentInChildren<OfferingRock>();

    }
    public void Inform(bool itemMatches)
    {
        if (!StartedSequence)
        {
            StartedSequence = true;
            if (!itemMatches)
            {
                // play a not matching sound
                PlaySound(itemDoesntMatchClip);
                offeringBowl.CheckForCollisions = true; //start checking for collisions again 
                StartedSequence = false; //Allow the sequence to begin again.
            }
            else
            {
                StartCoroutine(AcceptTheOffer());
            }
        }
        else
        {
            Debug.Log("You already contacted the OfferingAreaManager for this item!");
        }
    }
    void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    IEnumerator AcceptTheOffer()
    {
        PlaySound(itemMatchesClip);
        yield return new WaitForSeconds(1.5f);
        lightCylinder.SetActive(true);
        PlaySound(lightOnClip);
        yield return new WaitForSeconds(1);
        PlaySound(beamClip);
        ParticleSystem ps = lightCylinder.GetComponentInChildren<ParticleSystem>();
        ps.Play();
        yield return new WaitForSeconds(2.5f);
        lightCylinder.SetActive(false);
        offeringBowl.GiveLoot();
    }

    public void SequenceOver()
    {
        offeringRock.ChangeUI();
        //Disable the previous UI on the rock(image or text)
        //Load the next offering riddle
    }

}
