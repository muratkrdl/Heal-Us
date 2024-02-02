using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPickup : MonoBehaviour
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
            if(HPPotions.Instance.GetCurrentCount >= HPPotions.Instance.GetMaxCount) { return; }
            HPPotions.Instance.IncreasePotionCount();
            if(GetComponentInParent<PotionSpawnPoint>() != null)
            {
                GetComponentInParent<PotionSpawnPoint>().StartTimerForPotion();
            }
            Destroy(gameObject);
        }
    }
}
