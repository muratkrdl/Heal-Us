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
        SetPercentSlowAmount();
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            if(other.TryGetComponent<FirstPersonController>(out var player))
            {
                player.MoveSpeed = GameManager.Instance.GetPlayerSpeed / slowAmountPercent;
                player.SprintSpeed = GameManager.Instance.GetPLayerSprintSpeed / slowAmountPercent;
            }
        }
        else if(other.CompareTag("Village"))
        {
            if(other.TryGetComponent<Villager>(out var villager))
            {
                villager.SetAnimatorSpeed(slowAmountPercent / 4);
                villager.navMeshAgent.speed = GameManager.Instance.GetVillagerSpeed / slowAmountPercent;
            }
        }
        else if(other.CompareTag("Monster"))
        {
            if(other.TryGetComponent<Monster>(out var monster))
            {
                monster.SetAnimatorSpeed(slowAmountPercent / 4);
                monster.navMeshAgent.speed = GameManager.Instance.GetMonsterSpeed / slowAmountPercent;
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
                player.MoveSpeed = GameManager.Instance.GetPlayerSpeed;
                player.SprintSpeed = GameManager.Instance.GetPLayerSprintSpeed;
            }
        }
        else if(other.CompareTag("Village"))
        {
            if(other.TryGetComponent<Villager>(out var villager))
            {
                villager.SetAnimatorSpeed(1);
                villager.navMeshAgent.speed = GameManager.Instance.GetVillagerSpeed;
            }
        }
        else if(other.CompareTag("Monster"))
        {
            if(other.TryGetComponent<Monster>(out var monster))
            {
                monster.SetAnimatorSpeed(1);
                monster.navMeshAgent.speed = GameManager.Instance.GetMonsterSpeed;
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

    public void SetPercentSlowAmount()
    {
        slowAmountPercent = slowAmount / 25;
    }

}
