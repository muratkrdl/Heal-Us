using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;

    [SerializeField] AudioSource walkSFX;

    [SerializeField] Animator animator;
    [SerializeField] Collider[] colliders;

    [SerializeField] ParticleSystem lightningFX;
    [SerializeField] ParticleSystem fireBallStunFX;

    [SerializeField] float meleeDamage;

    [SerializeField] float maxDistanceForPlayer;
    [SerializeField] float haveChasedPlayerDistance;
    [SerializeField] float chasePlayerTime;
    [SerializeField] float haveChasedPlayerTimer;

    [SerializeField] MonsterExplosionAbility explosionAbility;
    [SerializeField] MonsterSpawnAbility spawnAbility;

    public Transform target;

    [HideInInspector] public bool isAttacking;

    bool isTargetToPlayer;
    bool isStunned;
    bool isWalking;
    bool isDead;
    bool haveChasedPlayer;

    public Animator GetAnimator
    {
        get
        {
            return animator;
        }
    }

    void Start()
    {
        target = GameManager.Instance.GetPlayer;
        target = GameManager.Instance.GetNewTarget(target);
        isTargetToPlayer = false;
    }

    void Update() 
    {
        if(isDead) { return; }

        if(Mathf.Abs(Vector3.Distance(target.transform.position,transform.position)) <= .1f && isWalking)
        {
            if(target == GameManager.Instance.GetPlayer) { return; }
            isWalking = false;
            animator.SetTrigger("Idle");
            target = GameManager.Instance.GetNewTarget(target);
        }
        else
        {
            Catch(target);
        }
        if(isStunned) { navMeshAgent.destination = transform.position; return; }

        GetDestination(target);

        if(Mathf.Abs(Vector3.Distance(transform.position,GameManager.Instance.GetPlayer.position)) <= maxDistanceForPlayer) 
        {
            if(isTargetToPlayer || haveChasedPlayer) { return; }

            isTargetToPlayer = true;
            target = GameManager.Instance.GetPlayer;
            StartCoroutine(CheckPlayerDistance());
        }
        else if(Mathf.Abs(Vector3.Distance(transform.position,GameManager.Instance.GetPlayer.position)) > maxDistanceForPlayer + 3)
        {
            if(!isTargetToPlayer || !haveChasedPlayer) { return; }

            isTargetToPlayer = false;
            target = GameManager.Instance.GetNewTarget(target);
            StopCoroutine(CheckPlayerDistance());
        }
    }

    IEnumerator CheckPlayerDistance()
    {
        while(true)
        {
            yield return new WaitForSeconds(chasePlayerTime);
            if(Mathf.Abs(Vector3.Distance(transform.position,GameManager.Instance.GetPlayer.position)) > haveChasedPlayerDistance)
            {
                haveChasedPlayer = true;
                isTargetToPlayer = false;
                target = GameManager.Instance.GetNewTarget(target);
                yield return new WaitForSeconds(haveChasedPlayerTimer);
                haveChasedPlayer = false;
                break;
            }
        }
        StopCoroutine(CheckPlayerDistance());
    }

    public void MonsterStunned(float time,bool isLightning)
    {
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
        if(isStunned) { StopCoroutine(StunMonster(time)); }

        target = transform;
        isStunned = true;
        isWalking = false;
        animator.SetTrigger("GetDamage");
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
            Vector3 direction = -(transform.position - other.transform.position).normalized;
            other.GetComponent<FirstPersonController>().Knockback(direction,transform);
        }
    }

#region AnimationEvents
    public void PlayerMeleeAnimationEvent()
    {
        if(Mathf.Abs(Vector3.Distance(transform.position,target.position)) < 2.5f)
            PlayerHP.Instance.DecreaseHP(meleeDamage);
    }
    public void MeleeAnimationEvent()
    {
        if(target.GetComponent<Villager>() != null)
        {
            target.GetComponent<Villager>().GetDamageFromMonster();
            target = GameManager.Instance.GetNewTarget(target);
        }
        navMeshAgent.destination = transform.position;
        isWalking = false;
    }
    public void MeleeAnimationFinishEvent()
    {
        isAttacking = false;
        isTargetToPlayer = false;
        isWalking = false;
        navMeshAgent.destination = transform.position;
    }
    public void WalkAnimationEvent()
    {
        if(isAttacking) { isAttacking = false; }
        if(!isTargetToPlayer)
        {
            target = GameManager.Instance.GetNewTarget(target);
        }
    }
    public void IdleAnimationEvent()
    {
        target = GameManager.Instance.GetNewTarget(target);
    }
    public void StunnedAttackAnimationEvent()
    {
        if(Random.Range(0,2) %2 == 0)
        {
            explosionAbility.SpawnEnergyExplosion();
        }
        else
        {
            spawnAbility.StartSpawnStoneMonsters();
        }
    }
#endregion

    public void GetDestination(Transform target)
    {
        if(!isWalking && !isAttacking)
        {
            isWalking = true;
            if(!walkSFX.isPlaying)
            {
                walkSFX.Play();
            }
            animator.SetTrigger("Walk");
        }
        if(Mathf.Abs(Vector3.Distance(transform.position,target.position)) <= 1.5f)
        {
            //bite and bite animation
            if(isAttacking) { return; }

            StopWalkSFX();
            isWalking = false;
            isAttacking = true;
            if(target == GameManager.Instance.GetPlayer)
            {
                animator.SetTrigger("PlayerMelee");
                return;
            }
            animator.SetTrigger("Melee");
        }
        else if(Mathf.Abs(Vector3.Distance(transform.position,target.position)) > 3f)
        {
            if(!isAttacking) { return; }

            isAttacking = false;
            isTargetToPlayer = false;
            isWalking = false;
            navMeshAgent.destination = transform.position;
        }
    }

    void Catch(Transform target)
    {
        navMeshAgent.destination = target.position;
    }

    public void Die()
    {
        StopWalkSFX();
        isDead = true;
        StopAllCoroutines();
        navMeshAgent.enabled = true;
        navMeshAgent.destination = transform.position;
        navMeshAgent.enabled = false;
        foreach (var item in colliders)
        {
            item.enabled = false;
        }
    }

    public void StopWalkSFX()
    {
        if(walkSFX.isPlaying)
        {
            walkSFX.Stop();
        }
    }

    public void SetAnimatorSpeed(float value)
    {
        animator.speed = value;
    }

}
