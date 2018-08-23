﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LowPoly.Character;

//TODO make this class accept generic types because we've used this same logic thee times
public class DamageZone : MonoBehaviour  
{
    [SerializeField]AudioSource m_AudioSource;
    //am i currently cuasing damage
    private bool m_IsCausingDamage = false;

    [SerializeField] int damageAmount = 10;
    //after how many seconds do I apply damage
    [SerializeField] float damageRepeatRate = 2f;
    [SerializeField] bool isDamageAppliedRepeatedly = true;

    private void OnTriggerEnter(Collider other)
    {
        HealthSystem playersHealth = other.gameObject.GetComponent<HealthSystem>();
        m_IsCausingDamage = true;

        if(isDamageAppliedRepeatedly)
        {
            StartCoroutine(DealDamageRepeatedly(playersHealth, damageRepeatRate));
        }
        else //just inflict one-time damage
        {
            playersHealth.TakeDamage(damageAmount);
        }
    }

    IEnumerator DealDamageRepeatedly(HealthSystem playersHealthSystem, float repeatRate)
    {
        while(m_IsCausingDamage)
        {
            playersHealthSystem.TakeDamage(damageAmount);
            DealDamageRepeatedly(playersHealthSystem, repeatRate);
            yield return new WaitForSeconds(repeatRate);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        m_IsCausingDamage = false;
        StopAllCoroutines();
    }
}
