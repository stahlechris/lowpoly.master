using UnityEngine;
using LowPoly.Character;

public class FirstAid : Item 
{
    [SerializeField] AudioClip healSound;
    [SerializeField] int healthToRestore = 20;

    public override void OnUse()
    {
        AudioSource m_AudioSource = GetComponent<AudioSource>();
        HealthSystem playersHealthSystem = player.GetComponent<HealthSystem>();
        m_AudioSource.PlayOneShot(healSound);
        playersHealthSystem.ReceiveHealing(healthToRestore);
        player.inventory.RemoveItem(this);
        Destroy(this.gameObject);
    }
}

/* 0.065, 1.2, 0.436
 * -180, 0, 90
 * 205, 196, 200
 */