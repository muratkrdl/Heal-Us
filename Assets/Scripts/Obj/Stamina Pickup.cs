using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPickup : MonoBehaviour
{
    [SerializeField] float stopParticleTime;

    [SerializeField] ParticleSystem spawnParticleSystem;

    void Start() 
    {
        StartCoroutine(StopParticleSystem());
    }

    IEnumerator StopParticleSystem()
    {
        yield return new WaitForSeconds(stopParticleTime);
        spawnParticleSystem.Stop();
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            if(StaminaPotions.Instance.GetCurrentCount >= StaminaPotions.Instance.GetMaxCount) { return; }
            StaminaPotions.Instance.IncreasePotionCount();
            if(GetComponentInParent<PotionSpawnPoint>() != null)
            {
                GetComponentInParent<PotionSpawnPoint>().StartTimerForPotion();
            }
            Destroy(gameObject);
        }
    }
}
