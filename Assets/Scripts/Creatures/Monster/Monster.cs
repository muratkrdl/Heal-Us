using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class Monster : Creature
{
    public NavMeshAgent navMeshAgent;

    [SerializeField] Animator animator;

    [SerializeField] ParticleSystem lightningFX;
    [SerializeField] ParticleSystem fireBallStunFX;

    [SerializeField] float waitForAnimationTime;

    [SerializeField] float meleeDamage;

    [SerializeField] float maxDistanceForPlayer;

    [SerializeField] MonsterExplosionAbility explosionAbility;
    [SerializeField] MonsterSpawnAbility spawnAbility;

    [HideInInspector] public Transform target;

    [HideInInspector] public bool isAttacking;

    [HideInInspector] public bool isTargetToPlayer;

    bool isStunned;
    bool isWalking;

    void Awake() 
    {
        target = GameManager.Instance.GetPlayer;
        target = GameManager.Instance.GetNewTarget(target);
        isTargetToPlayer = false;
    }

    void Update() 
    {
        if(Mathf.Abs(Vector3.Distance(target.transform.position,transform.position)) <= .1f && isWalking)
        {
            isWalking = false;
            animator.SetTrigger("Idle");
            Debug.Log("getnewtarget");
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
            if(isTargetToPlayer) { return; }

            isTargetToPlayer = true;
            target = GameManager.Instance.GetPlayer;
        }
        else if(Mathf.Abs(Vector3.Distance(transform.position,GameManager.Instance.GetPlayer.position)) > maxDistanceForPlayer)
        {
            if(!isTargetToPlayer) { return; }

            isTargetToPlayer = false;
            target = GameManager.Instance.GetNewTarget(target);
        }

        //if(Vector3.Distance(transform.position,target.position) <= maxDistance)
        //{
        //    hasDestination = true;
        //}
        //else
        //{
        //    hasDestination = false;
        //}

        //if(target == GameManager.Instance.GetPlayer) { return; }

        //if(target.GetComponent<Villager>().isInfected)
        //{
        //    hasDestination = false;
        //    navMeshAgent.destination = transform.position;
        //    StopAllCoroutines();
        //    isWalking = false;
        //    target = GameManager.Instance.GetNewVillage(target);
        //}
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

        StartCoroutine(WaitForAnim());

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

    IEnumerator WaitForAnim()
    {
        yield return new WaitForSeconds(waitForAnimationTime);
        if(Random.Range(0,2) %2 == 0)
        {
            explosionAbility.SpawnEnergyExplosion();
        }
        else
        {
            spawnAbility.StartSpawnStoneMonsters();
        }
        StopCoroutine(WaitForAnim());
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

#region AnimationEvents
        public void MeleeAnimationEvent()
    {
        if(target.GetComponent<Villager>() != null)
        {
            target.GetComponent<Villager>().GetDamageFromMonster();
            target = GameManager.Instance.GetNewTarget(target);
        }
        else if(target.GetComponent<FirstPersonController>() != null)   
        {
            PlayerHP.Instance.DecreaseHP(meleeDamage);
        }
        navMeshAgent.destination = transform.position;
        isWalking = false;
        //hasDestination = false;
    }

    public void MeleeAnimationFinishEvent()
    {
        isAttacking = false;
        isTargetToPlayer = false;
        isWalking = false;
        //hasDestination = false;
        navMeshAgent.destination = transform.position;
        target = GameManager.Instance.GetNewTarget(target);
        //if(target.CompareTag("Village")) { return; }
        //if(Mathf.Abs(Vector3.Distance(transform.position,target.position)) <= 1.85f)
        //{
        //    hasDestination = true;
        //    isWalking = true;
        //    isTargetToPlayer = true;
        //    isAttacking = false;
        //}
    }

    public void WalkAnimationEvent()
    {
        if(isAttacking) { isAttacking = false; }
    }

    public void IdleAnimationEvent()
    {
        target = GameManager.Instance.GetNewTarget(target);
    }
#endregion

    public void GetDestination(Transform target)
    {
        //if(hasDestination)
            //For Monster
            if(!isWalking && !isAttacking)
            {
                isWalking = true;
                animator.SetTrigger("Walk");
            }
            if(Mathf.Abs(Vector3.Distance(transform.position,target.position)) <= 1.85f)
            {
                //bite and bite animation
                if(isAttacking) { return; }

                isWalking = false;
                isAttacking = true;
                animator.SetTrigger("Melee");
            }
            else if(Mathf.Abs(Vector3.Distance(transform.position,target.position)) > 2.2f)
            {
                if(!isAttacking) { return; }

                isAttacking = false;
                isTargetToPlayer = false;
                isWalking = false;
                //hasDestination = false;
                navMeshAgent.destination = transform.position;
            }
        //else
        //{
        //    isWalking = false;
        //    Catch(target);
        //}
    }

    void Catch(Transform target)
    {
        navMeshAgent.destination = target.position;
    }

}
