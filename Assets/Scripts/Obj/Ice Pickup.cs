using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePickup : MonoBehaviour
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
            if(IcePotions.Instance.GetCurrentCount >= IcePotions.Instance.GetMaxCount) { return; }
            IcePotions.Instance.IncreasePotionCount();
            if(GetComponentInParent<PotionSpawnPoint>() != null)
            {
                GetComponentInParent<PotionSpawnPoint>().StartTimerForPotion();
            }
            Destroy(gameObject);
        }
    }
}
