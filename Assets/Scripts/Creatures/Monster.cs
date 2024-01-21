using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Monster : Creature
{
    [SerializeField] float meleeDamage;
    [SerializeField] ParticleSystem lightningFX;
    [SerializeField] ParticleSystem fireBallStunFX;

    [SerializeField] float maxDistanceForPlayer;

    [SerializeField] MonsterExplosionAbility explosionAbility;

    Transform target;

    bool isStunned;

    bool isTargetToPlayer;

    void Start() 
    {
        target = GameManager.Instance.GetNewVillage(target);
        isTargetToPlayer = false;
        newRandomPos = transform.position;
    }

    void Update() 
    {
        if(Mathf.Abs(Vector3.Distance(newRandomPos,transform.position)) <= .1f && isWalking)
        {
            isWalking = false;
            animator.Play("Idle");
            target = GameManager.Instance.GetNewVillage(target);
        }

        if(isStunned) { newRandomPos = transform.position; return; }

        if(Mathf.Abs(Vector3.Distance(transform.position,GameManager.Instance.GetPlayer.position)) <= maxDistanceForPlayer) 
        {
            if(!isTargetToPlayer)
            {
                isTargetToPlayer = true;
                target = GameManager.Instance.GetPlayer;
            }
        }
        else if(Mathf.Abs(Vector3.Distance(transform.position,GameManager.Instance.GetPlayer.position)) > maxDistanceForPlayer)
        {
            if(isTargetToPlayer)
            {
                isTargetToPlayer = false;
                target = GameManager.Instance.GetNewVillage(target);
            }
        }

        GetDestination(target); 
        if(Vector3.Distance(transform.position,target.position) <= maxDistance)
        {
            hasDestination = true;
        }
        else
        {
            hasDestination = false;
            StopCoroutine(Catch(target));
        }

        if(target == GameManager.Instance.GetPlayer) { return; }
        if(target.GetComponent<Villager>().isInfected)
        {
            hasDestination = false;
            navMeshAgent.destination = newRandomPos;
            StopAllCoroutines();
            isWalking = false;
            isRunning = false;
            target = GameManager.Instance.GetNewVillage(target);
        }
    }

    public void MonsterStunned(float time,bool isLightning)
    {
        StopAllCoroutines();
        navMeshAgent.destination = transform.position;
        StartCoroutine(StunMonster(time));
        if(isLightning)
        {
            lightningFX.Play();
        }
        else
        {
            fireBallStunFX.Play();
        }
    }

    IEnumerator StunMonster(float time)
    {
        explosionAbility.SpawnEnergyExplosion();
        isStunned = true;
        target = transform;
        isRunning = false;
        animator.Play("Idle");
        yield return new WaitForSeconds(time);
        target = GameManager.Instance.GetPlayer;
        isTargetToPlayer = true;
        isStunned = false;
        if(lightningFX.isPlaying)
        {
            lightningFX.Stop();
        }
        if(fireBallStunFX.isPlaying)
        {
            fireBallStunFX.Stop();
        }
        StopCoroutine(StunMonster(time));
    }

    public void SetMaxDistanceForPlayer(float amount)
    {
        maxDistanceForPlayer = amount;
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerHP.Instance.DecreaseHP(meleeDamage);
            Vector3 direction = -(transform.position - other.transform.position).normalized;
            other.GetComponent<FirstPersonController>().Knockback(direction,transform);
        }
    }

}
