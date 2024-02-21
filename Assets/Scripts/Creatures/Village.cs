using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class Villager : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;

    [SerializeField] AudioSource walkSFX;
    [SerializeField] AudioSource runSFX;
    
    [SerializeField] Animator animator;

    [SerializeField] Vector2 areaSize;

    [SerializeField] ParticleSystem healVFX;
    [SerializeField] ParticleSystem infectedVFX;

    [SerializeField] float maxViewDistance;
    [SerializeField] float lifeTime;
    [SerializeField] float decreaseLifeTimeAmount;

    [SerializeField] float minWaitTime;
    [SerializeField] float maxWaitTime;

    [SerializeField] float newPosMinDistance;
    [SerializeField] float newPosMaxDistance;

    float initialSpeed;

    Vector3 newRandomPos;
    Vector3 direction;
    Vector3 newPos;

    Transform target;

    bool hasDestination;

    bool isInfected;

    bool isDead;

    bool isWaiting;
    bool isWalking;
    bool isRunning;

    public bool GetIsInfected
    {
        get
        {
            return isInfected;
        }
    }

    public bool GetIsDead
    {
        get
        {
            return isDead;
        }
    }

    void Start() 
    {
        target = GameManager.Instance.GetMonster;
        newRandomPos = transform.position;
        initialSpeed = navMeshAgent.speed;
    }

    void Update() 
    {
        if(isDead) { return; }
        GetDestination();
        
        newRandomPos.y = transform.position.y;

        if(Mathf.Abs(Vector3.Distance(newRandomPos,transform.position)) <= .01f && isWalking)
        {
            if(!isWalking) { return; }
            if(isWalking) { isWalking = false; }
            if(isRunning) { isRunning = false; }
            StopWalkSFX();
            StopRunSFX();
            StopCoroutine(CheckWalkingCharacter());
            ResetTriggerWalk();
            ResetTriggerRun();
            animator.SetTrigger("Idle");
            newRandomPos = transform.position;
        }

        if(isInfected) { return; }

        if(Mathf.Abs(Vector3.Distance(transform.position,target.position)) <= maxViewDistance)
        {
            hasDestination = true;
            isWaiting = false;
            isWalking = false;
        }
        else if(Mathf.Abs(Vector3.Distance(transform.position,target.position)) > maxViewDistance + 5f)
        {
            if(!hasDestination) { return; }
            ResetTriggerWalk();
            ResetTriggerRun();
            animator.SetTrigger("Idle");
            StopWalkSFX();
            StopRunSFX();
            hasDestination = false;
            isRunning = false;
            StopCoroutine(Escape(target));
        }
    }

    void GetDestination()
    {
        if(!hasDestination && !isWalking && !isWaiting)
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
                StopWalkSFX();
                if(!runSFX.isPlaying)
                {
                    runSFX.Play();
                }
                isRunning = true;
                ResetTriggerIdle();
                ResetTriggerWalk();
                animator.SetTrigger("Run");
            }
            isWaiting = false;
            isWalking = false;
        }
    }

    IEnumerator WaitandGoPosition()
    {
        isWaiting = true;
        isWalking = true;
        yield return new WaitForSeconds(Random.Range(minWaitTime,maxWaitTime));
        isWaiting = false;
        if(hasDestination) 
        {
            StopCoroutine(WaitandGoPosition());
            yield return null;
        }
        newRandomPos = Random.insideUnitSphere * Random.Range(newPosMinDistance,newPosMaxDistance);
        newRandomPos.y = 0;
        newRandomPos += transform.position;
        if(Mathf.Abs(newRandomPos.x) < areaSize.x && Mathf.Abs(newRandomPos.z) < areaSize.y)
        {
            if(!walkSFX.isPlaying)
            {
                walkSFX.Play();
            }
            ResetTriggerIdle();
            ResetTriggerRun();
            animator.SetTrigger("Walk");
            navMeshAgent.destination = newRandomPos;
            StartCoroutine(CheckWalkingCharacter());
        }
        else 
        {
            isWalking = false;
            hasDestination = false;
        }
        StopCoroutine(WaitandGoPosition());
    }

    IEnumerator Escape(Transform target)
    {
        isWaiting = false;
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
        StopAllCoroutines();
        SoundManager.Instance.PlaySound3D("Heal",transform.position);
        animator.speed = 1;
        navMeshAgent.speed = initialSpeed;
        lifeTime *= decreaseLifeTimeAmount;
        isWaiting = false;
        isWalking = false;
        isRunning = false;
        hasDestination = false;
        infectedVFX.Stop();
        healVFX.Play();
        isInfected = false;
    }

    public void GetDamageFromMonster()
    {
        DamageMultiplier.Instance.UpdateText();
        SoundManager.Instance.PlaySound3D("Infect",transform.position);
        animator.speed = .5f;
        navMeshAgent.speed = initialSpeed / 2;
        StopRunSFX();
        StopWalkSFX();
        StopAllCoroutines();
        ResetTriggerIdle();
        ResetTriggerWalk();
        ResetTriggerRun();
        animator.SetTrigger("GetDamage");
        isInfected = true;
        hasDestination = false;
        isWaiting = false;
        isWalking = false;
        isRunning = false;
        infectedVFX.Play();
        StartCoroutine(StartDieTimer());
    }

    IEnumerator CheckWalkingCharacter()
    {
        _ = Vector3.zero;
        while (true)
        {
            Vector3 checkPos = transform.position;

            yield return new WaitForSeconds(.1f);
            if(checkPos == transform.position)
            {
                //Stop
                isWaiting = false;
                isWalking = false;
                isRunning = false;
                animator.SetTrigger("Idle");
                break;
            }
        }
        StopCoroutine(CheckWalkingCharacter());
    }

    IEnumerator StartDieTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        Die();
    }

    void Die()
    {
        StopWalkSFX();
        StopRunSFX();
        infectedVFX.Stop();
        isDead = true;
        DamageMultiplier.Instance.UpdateText();
        StopAllCoroutines();
        navMeshAgent.destination = transform.position;
        ResetTriggerIdle();
        if(Random.Range(0,2) == 0)
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
        enabled = false;
    }

    public void StopWalkSFX()
    {
        if(walkSFX.isPlaying)
        {
            walkSFX.Stop();
        }
    }
    public void StopRunSFX()
    {
        if(runSFX.isPlaying)
        {
            runSFX.Stop();
        }
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

    public void SetAnimatorSpeed(float value)
    {
        if(isInfected) { return; }
        animator.speed = value;
    }

}
