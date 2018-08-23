using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LowPoly.Character;

public class SpaceCube : MonoBehaviour
{
    public float criticalHitChance = 0.05f;
    public float criticalMultiplier = 1.5f;
    public StonehengeManager stonehengeManager;
    Behaviour[] allComponents;
    public ParticleSystem chargeLaserParticlefx;
    public ParticleSystem laserBeamParticlefx;
    public AudioSource audioSource;
    public AudioClip chargeUp;
    public AudioClip fireLaser;
    public Light my_light;
    public PlayerController player;
    //If the player has Ed's shield, he can deflect the laser beam back into the eye
    //Causing the tomes to reactivate and become lootable


    //There's an animation event that calls this.
    IEnumerator EnableMyComponents()
    {
        allComponents = gameObject.GetComponents<Behaviour>();
        yield return null;
        foreach (Behaviour comp in allComponents)
        {
            comp.enabled = true;
        }
        yield return new WaitForSeconds(1.5f);
        FireLayzar();
    }
    public void AcceptPlayerReference(PlayerController reference)
    {
        player = reference;
    }
    void FireLayzar()
    {
        StartCoroutine(FireLazer());
    }
    IEnumerator FireLazer()
    {
        PlayChargeLaserSound();
        yield return new WaitForSeconds(1f);
        chargeLaserParticlefx.Play();
        yield return new WaitForSeconds(2.5f);
        laserBeamParticlefx.Play();
        PlayFireLaserSound();
        StartCoroutine(DealKnockbackDamageRoutine());
        //CleanupParticles();
    }
    IEnumerator DealKnockbackDamageRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        DealKnockbackDamage();
    }

    void CleanupParticles()
    {
        Destroy(chargeLaserParticlefx, 4);
        Destroy(laserBeamParticlefx, 4);
    }

    void PlayChargeLaserSound()
    {
        audioSource.PlayOneShot(chargeUp);
    }
    void PlayFireLaserSound()
    {
        audioSource.PlayOneShot(fireLaser);
    }
    void PlayAriseFromGroundSound()
    {

    }

    public void DealKnockbackDamage()
    {
        stonehengeManager.EnableStonehengeCam(false);
        Debug.Log("Dealing knockback damage");
        KnockBack();
        DealDamage();
    }

    #region DAMAGE_METHODS
    public void DealDamage()
    {
        HealthSystem playersHs = player.GetComponent<HealthSystem>();
        int initialDamage = Random.Range(25, 50);
        playersHs.TakeDamage(CalculateLaserDamage(initialDamage));
    }
    int CalculateLaserDamage(int initialDamage)
    {
        int totalDmg = 1;
        bool isCriticalHit = Random.Range(0f, 1f) <= criticalHitChance;//5%

        if (isCriticalHit)
        {
            Debug.Log("CRITICAL LASER HIT!");
            totalDmg = initialDamage * Mathf.RoundToInt(criticalMultiplier);//1.5 x normal
        }
        else
        {
            totalDmg = initialDamage;
        }
        return totalDmg;
    }
    public void KnockBack()
    {
        Rigidbody playersRb = player.GetComponent<Rigidbody>();
        //Vector3 moveDirection = playersRb.transform.position - transform.position;
        //playersRb.AddForce(moveDirection.normalized * -5000f);
        //playersRb.AddExplosionForce() player.transform.forward, 10f);
        playersRb.AddForce(0f, 7.5f, 500f, ForceMode.Impulse);
    }
#endregion
}
