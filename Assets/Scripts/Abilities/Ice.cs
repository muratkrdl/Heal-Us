using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Ice : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] float slowTime;
    [SerializeField] float slowAmount;

    [SerializeField] ParticleSystem iceFX;
    [SerializeField] BoxCollider boxCollider;

    void Start()
    {
        StartCoroutine(KYS());
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            var player = other.GetComponentInChildren<FirstPersonController>();
            if(player != null)
            {
                player.MoveSpeed /= slowAmount;
                player.SprintSpeed /= slowAmount;
            }
        }
        else if(other.CompareTag("Village"))
        {
            if(other.TryGetComponent<Villager>(out var villager))
            {
                villager.navMeshAgent.speed /= slowAmount;
            }
        }
        else if(other.CompareTag("Monster"))
        {
            if(other.TryGetComponent<Monster>(out var monster))
            {
                monster.navMeshAgent.speed /= slowAmount;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var player = other.GetComponentInChildren<FirstPersonController>();
            if(player != null)
            {
                player.MoveSpeed *= slowAmount;
                player.SprintSpeed *= slowAmount;
            }
        }
        else if(other.CompareTag("Village"))
        {
            if(other.TryGetComponent<Villager>(out var villager))
            {
                villager.navMeshAgent.speed *= slowAmount;
            }
        }
        else if(other.CompareTag("Monster"))
        {
            if(other.TryGetComponent<Monster>(out var monster))
            {
                monster.navMeshAgent.speed *= slowAmount;
            }
        }
    }

    IEnumerator KYS()
    {
        yield return new WaitForSeconds(lifeTime);
        iceFX.Stop();
        boxCollider.size = Vector3.zero;
        Destroy(gameObject,slowTime + 5);
        StopAllCoroutines();
    }

}
