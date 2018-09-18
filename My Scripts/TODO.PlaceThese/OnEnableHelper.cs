using UnityEngine;

public class OnEnableHelper : MonoBehaviour 
{
    [SerializeField]ParticleSystem beachFog;

    void Start () 
    {
        beachFog = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        beachFog.Play();
    }

}
