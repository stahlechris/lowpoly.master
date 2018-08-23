using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPlayer : MonoBehaviour 
{
    ParticleSystem ps;
    private void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {

        ps.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        ps.Stop();
    }
}
