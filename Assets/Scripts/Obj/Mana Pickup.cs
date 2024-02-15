using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPickup : MonoBehaviour
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
            SoundManager.Instance.PlaySound3D("Pick Up",transform.position);
            if(ManaPotions.Instance.GetCurrentCount >= ManaPotions.Instance.GetMaxCount) { return; }
            ManaPotions.Instance.IncreasePotionCount();
            if(GetComponentInParent<PotionSpawnPoint>() != null)
            {
                GetComponentInParent<PotionSpawnPoint>().StartTimerForPotion();
            }
            Destroy(gameObject);
        }
    }

}
