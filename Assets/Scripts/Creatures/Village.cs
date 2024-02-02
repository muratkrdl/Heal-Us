using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class Villager : Creature
{
    public NavMeshAgent navMeshAgent;
    
    [SerializeField] Animator animator;

    [SerializeField] Vector2 areaSize;

    [SerializeField] float maxViewDistance;
    [SerializeField] float lifeTime;

    [SerializeField] float minWaitTime;
    [SerializeField] float maxWaitTime;

    [SerializeField] float newPosMinDistance;
    [SerializeField] float newPosMaxDistance;

    Vector3 newRandomPos;
    Vector3 direction;
    Vector3 newPos;

    Transform target;

    int index;

    bool hasDestination;

    bool isInfected;

    bool isDead;

    bool isWalking;
    bool isRunning;

    public bool GetIsInfected
    {
        get
        {
            return isInfected;
        }
    }

    void Start() 
    {
        target = GameManager.Instance.GetMonster.transform;
        newRandomPos = transform.position;
    }

    void Update() 
    {
        if(isDead) { return; }
        GetDestination(target);
        
        if(Mathf.Abs(Vector3.Distance(newRandomPos,transform.position)) <= .01f && isWalking)
        {
            if(!isWalking) { return; }
            if(isWalking) { isWalking = false; }
            if(isWalking) { isRunning = false; }
            ResetTriggerWalk();
            ResetTriggerRun();
            animator.SetTrigger("Idle");
            newRandomPos = transform.position;
        }

        if(isInfected) { return; }

        if(Mathf.Abs(Vector3.Distance(transform.position,target.position)) <= maxViewDistance)
        {
            hasDestination = true;
            isWalking = false;
        }
        else if(Mathf.Abs(Vector3.Distance(transform.position,target.position)) > maxViewDistance + 5f)
        {
            if(!hasDestination) { return; }
            ResetTriggerWalk();
            ResetTriggerRun();
            animator.SetTrigger("Idle");
            hasDestination = false;
            isWalking = false;
            isRunning = false;
            StopCoroutine(Escape(target));
        }
    }

    void GetDestination(Transform target)
    {
        if(!hasDestination && !isWalking)
        {
            StartCoroutine(WaitandGoPosition());
        }
        else if(hasDestination)
        {
            if(isInfected) { hasDestination = false; return; }
            StopAllCoroutines();
            StartCoroutine(Escape(target));
            if(!isRunning)
            {
                isRunning = true;
                ResetTriggerIdle();
                ResetTriggerWalk();
                animator.SetTrigger("Run");
            }
            isWalking = false;
        }
    }

    IEnumerator WaitandGoPosition()
    {
        isWalking = true;
        yield return new WaitForSeconds(Random.Range(minWaitTime,maxWaitTime));

        if(hasDestination) 
        {
            StopCoroutine(WaitandGoPosition());
            yield return null;
        }
        index = Random.Range(0,2);
        if(index % 2 == 1) { newRandomPos = Random.insideUnitSphere * newPosMinDistance; }
        else { newRandomPos = Random.insideUnitSphere * newPosMaxDistance; }
        newRandomPos.y = 0;
        newRandomPos += transform.position;
        if(Mathf.Abs(newRandomPos.x) < areaSize.x && Mathf.Abs(newRandomPos.z) < areaSize.y)
        {
            ResetTriggerIdle();
            ResetTriggerRun();
            animator.SetTrigger("Walk");
            navMeshAgent.destination = newRandomPos;
            StopCoroutine(WaitandGoPosition());
        }
        else
        {
            isWalking = false;
            hasDestination = false;
        }
    }

    IEnumerator Escape(Transform target)
    {
        isWalking = false;
        direction = (transform.position - target.position).normalized;
        newPos = transform.position + direction;
        navMeshAgent.destination = newPos;
        yield return null;
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Vector3 direction = -(transform.position - other.transform.position).normalized;
            other.GetComponent<FirstPersonController>().Knockback(direction,transform);
        }
    }

    public void GetHealFromPlayer()
    {
        
    }

    public void GetDamageFromMonster()
    {
        StopAllCoroutines();
        isInfected = true;
        //Infected and get damage animation
        ResetTriggerIdle();
        ResetTriggerWalk();
        ResetTriggerRun();
        animator.SetTrigger("GetDamage");
        hasDestination = false;
        isWalking = false;
        isRunning = false;
        StartCoroutine(StartDieTimer());
    }

    IEnumerator StartDieTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        Die();
    }

    void Die()
    {
        isDead = true;
        navMeshAgent.destination = transform.position;
        if(index == 0)
        {
            animator.SetTrigger("Die1");
        }
        else
        {
            animator.SetTrigger("Die2");
        }
        navMeshAgent.enabled = false;
        foreach (var item in GetComponents<BoxCollider>())
        {
            item.enabled = false;
        }
        GetComponent<Rigidbody>().isKinematic = true;
        StopAllCoroutines();
        enabled = false;
    }

#region ResetTrigger
    void ResetTriggerIdle()
    {
        animator.ResetTrigger("Idle");
    }

    void ResetTriggerWalk()
    {
        animator.ResetTrigger("Walk");
    }

    void ResetTriggerRun()
    {
        animator.ResetTrigger("Run");
    }
#endregion
    
}
