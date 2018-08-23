using UnityEngine;

public class BouncyStepManager : MonoBehaviour 
{
    public AudioSource audioSource;

    public void PlayAudio()
    {
        audioSource.Play();
    }
}
