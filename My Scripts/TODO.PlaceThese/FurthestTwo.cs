using System.Collections;
using UnityEngine;

public class FurthestTwo : MonoBehaviour 
{
    ParticleSystem particleSystem;
    AudioSource audioSource;

    BoxCollider triggerCollider;

    private void Start()
    {
        triggerCollider = GetComponent<BoxCollider>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFX()
    {
        particleSystem.Play();
        audioSource.pitch = 1.5f; //speed up the clip
        audioSource.Play();
    }
    IEnumerator Wait()
    {
        Transform particleHolderTemp = transform.GetChild(0);
        //Don't shift the position of the particleHolder until it's done playing
        yield return new WaitUntil(() => !particleSystem.isPlaying);

        //shift the particle holder so the smoke plays in the right place
        particleHolderTemp.localPosition = particleHolderTemp.localPosition + new Vector3(-30, 0, 0);
        //move child 0 to the end
        particleHolderTemp.SetAsLastSibling(); //this will always be the ParticleHolder

        triggerCollider.center = triggerCollider.center + new Vector3(-30, 0, 0);
        triggerCollider.enabled = true;
    }
    public void Reset()
    {
        StartCoroutine(Wait());
    }

}
