using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Ice : MonoBehaviour
{
    [SerializeField] int lifeTime;
    [SerializeField] float slowTime;
    [SerializeField] float slowAmount;

    [SerializeField] ParticleSystem iceFX;
    [SerializeField] BoxCollider boxCollider;

    public int SetSlowAmount
    {
        set
        {
            slowAmount = value;
        }
    }

    float slowAmountPercent;

    void Start()
    {
        StartCoroutine(KYS());
        slowAmountPercent = slowAmount / 25;
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            var player = other.GetComponentInChildren<FirstPersonController>();
            if(player != null)
            {
                player.MoveSpeed /= slowAmountPercent;
                player.SprintSpeed /= slowAmountPercent;
            }
        }
        else if(other.CompareTag("Village"))
        {
            if(other.TryGetComponent<Villager>(out var villager))
            {
                villager.navMeshAgent.speed /= slowAmountPercent;
            }
        }
        else if(other.CompareTag("Monster"))
        {
            if(other.TryGetComponent<Monster>(out var monster))
            {
                monster.navMeshAgent.speed /= slowAmountPercent;
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
                player.MoveSpeed *= slowAmountPercent;
                player.SprintSpeed *= slowAmountPercent;
            }
        }
        else if(other.CompareTag("Village"))
        {
            if(other.TryGetComponent<Villager>(out var villager))
            {
                villager.navMeshAgent.speed *= slowAmountPercent;
            }
        }
        else if(other.CompareTag("Monster"))
        {
            if(other.TryGetComponent<Monster>(out var monster))
            {
                monster.navMeshAgent.speed *= slowAmountPercent;
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
