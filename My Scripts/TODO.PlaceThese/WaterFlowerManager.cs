using UnityEngine;
using System.Collections;

public class WaterFlowerManager : MonoBehaviour 
{
    [SerializeField] AudioSource sharedAudioSource;
    [SerializeField] AudioClip[] audioclips;

    private void Start()
    {
        sharedAudioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        int randomNum = Random.Range(0, 2);
        sharedAudioSource.PlayOneShot(audioclips[randomNum]);
    }

}
