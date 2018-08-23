using System.Collections;
using UnityEngine;

public class GoToBetterPlace : MonoBehaviour 
{
    public SkinnedMeshRenderer my_skin;
    public Collider my_collider;
    public NPC_Behaviors my_behaviors;
    bool initiatedDeadSequence = false;
    public GameObject m_loot;

    public void BeFree()
    {
        if (!initiatedDeadSequence)
        {
            initiatedDeadSequence = true;
            ActivateLootItems();
            TurnOffComponents();
            PlayDeathFX();
        }
        else
        {
            Debug.Log("you already started the death sequence in " + this);
        }
    }

    void TurnOffComponents()
    {
        Destroy(GetComponentInParent<Collider>());
        my_skin.enabled = false;
        my_behaviors.enabled = false;

    }

    void PlayDeathFX()
    {
        AudioSource[] deathSound = m_loot.GetComponentsInChildren<AudioSource>();
        ParticleSystem[] fx = GetComponentsInChildren<ParticleSystem>();
        fx[0].Play();
        deathSound[0].Play();
        deathSound[1].Play();
        StartCoroutine(LootAppears(deathSound[2], fx[2]));
    }
    IEnumerator LootAppears(AudioSource itemAppears, ParticleSystem itemCloud)
    {
        yield return new WaitForSeconds(4.1f);
        itemAppears.Play();
        itemCloud.Play();
        Destroy(this,5f);
    }
    void ActivateLootItems()
    {
        if (m_loot != null)
        {   //Transform implements numerable NOT gameobject...this is stupid
            m_loot.SetActive(true);
            foreach (Transform item in m_loot.transform)
            {
                item.gameObject.SetActive(true);
                foreach (Behaviour component in item.GetComponentsInChildren<Behaviour>())
                {
                    component.enabled = true;
                }
            }
        }
    }
}
