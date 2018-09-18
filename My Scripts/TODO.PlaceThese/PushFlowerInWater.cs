using UnityEngine;
using System.Collections;

public class PushFlowerInWater : MonoBehaviour 
{
    ParticleSystem particle;
    WaterFlowerManager waterFlowerManager;
    const string PLAYER_TAG = "Player";

    public bool HasEntered { get; set; }

    private void Start()
    {
        waterFlowerManager = GetComponentInParent<WaterFlowerManager>();
        particle = GetComponentInChildren<ParticleSystem>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(PLAYER_TAG))
        {
            if(!HasEntered)
            {
                HasEntered = true;
                waterFlowerManager.PlaySound();
                particle.Play();
                StartCoroutine(WaitASec());
            }
        }
    }
    IEnumerator WaitASec()
    {
        yield return new WaitForSeconds(0.5f);
        HasEntered = false;
    }
}
