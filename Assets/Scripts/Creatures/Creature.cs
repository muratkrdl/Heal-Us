using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Creature : MonoBehaviour
{
    public float maxDistance;

    public float minWaitTime;
    public float maxWaitTime;

    public Vector3 newRandomPos;
    public int index;

    public Vector3 direction;
    public Vector3 newPos;

    public float newPosMinDistance;
    public float newPosMaxDistance;

    public bool hasDestination;
    public bool isWalking;
    public bool isRunning;
    public bool isMonster;

    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public Rigidbody myRigidbody;

    public void GetDestination(Transform target)
    {
        if(!hasDestination && !isWalking && !isMonster)
        {
            StartCoroutine(WaitandGoPosition());
        }
        else if(hasDestination && !isMonster)
        {
            //For Village
            StartCoroutine(Escape(target));
            if(!isRunning)
            {
                StopAllCoroutines();
                animator.SetTrigger("Run");
                isRunning = true;
            }
            if(Mathf.Abs(Vector3.Distance(transform.position,target.position)) <= 1.65f)
            {
                StopAllCoroutines();
                //Stop
                //Infected and get damage animation
                animator.CrossFade("Infected",.1f);
                gameObject.GetComponent<Villager>().isInfected = true;
                hasDestination = false;
                isWalking = false;
                isRunning = false;
            }
            isWalking = false;
        }
        else if(hasDestination && isMonster)
        {
            //For Monster
            if(!isRunning)
            {
                StopAllCoroutines();
                animator.SetTrigger("Run");
                isRunning = true;
            }
            StartCoroutine(Catch(target));
            if(Mathf.Abs(Vector3.Distance(transform.position,target.position)) <= 1.65f)
            {
                StopAllCoroutines();
                //bite and bite animation
                hasDestination = false;
                isWalking = false;
                isRunning = false;
                animator.CrossFade("Attack",.1f);
                newRandomPos = transform.position;
                GameManager.Instance.GetNewVillage(target);
            }
            isWalking = false;
        }
    }

    IEnumerator WaitandGoPosition()
    {
        myRigidbody.velocity = Vector3.zero;
        isWalking = true;
        yield return new WaitForSeconds(Random.Range(minWaitTime,maxWaitTime));

        index = Random.Range(0,2);
        if(index % 2 == 1) { newRandomPos = Random.insideUnitSphere * newPosMinDistance; }
        else { newRandomPos = Random.insideUnitSphere * newPosMaxDistance; }
        newRandomPos.y = 0;
        newRandomPos += transform.position;
        if(Mathf.Abs(newRandomPos.x) < 24 && Mathf.Abs(newRandomPos.z) < 24)
        {
            animator.CrossFade("Walk",.1f);
            navMeshAgent.destination = newRandomPos;
            StopAllCoroutines();
        }
        else
        {
            isWalking = false;
        }
    }

    public IEnumerator Catch(Transform target)
    {
        StopAllCoroutines();
        navMeshAgent.destination = target.position;
        yield return null;
    }

    public IEnumerator Escape(Transform target)
    {
        direction = (transform.position - target.position).normalized;
        newPos = transform.position + direction * 3;
        navMeshAgent.destination = newPos;
        yield return null;
    }
    
}
