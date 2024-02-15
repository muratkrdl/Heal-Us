using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Animator animator;

    [SerializeField] float minLifeTime;
    [SerializeField] float maxLifeTime;

    Vector3 spawnPoint;

    float lifeTime;

    bool canWalk;
    bool isDead;

    void Awake() 
    {
        canWalk = false;
        isDead = false;
        lifeTime = Random.Range(minLifeTime,maxLifeTime);
        spawnPoint = transform.position;
    }

    void Update()
    {
        if(!canWalk || isDead) { return; }
        if(!navMeshAgent.enabled)
        { 
            navMeshAgent.enabled = true; 
        }
        navMeshAgent.destination = GameManager.Instance.GetPlayer.position;
    }

    public void ReturnSpawanPoint()
    {
        canWalk = false;
        navMeshAgent.enabled = true;
        navMeshAgent.destination = spawnPoint;
    }

    public void AnimationEvent()
    {
        navMeshAgent.enabled = true;
        canWalk = true;
        animator.SetTrigger("Walk");
        StartCoroutine(Die());
    }

    public void DieAnimationEvent()
    {
        Destroy(gameObject);
    }

    IEnumerator Die()
    {
        GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(lifeTime);
        KYS();
    }

    void KYS()
    {
        isDead = true;
        canWalk = false;
        navMeshAgent.enabled = false;
        animator.SetTrigger("Die");
    }

}
