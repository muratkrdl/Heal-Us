using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.Mathematics;
using UnityEngine;

public class Villager : Creature
{
    Transform target;

    public bool isInfected;

    void Start() 
    {
        target = GameManager.Instance.GetMonster.transform;
        newRandomPos = transform.position;
    }

    void Update() 
    {
        if(Mathf.Abs(Vector3.Distance(newRandomPos,transform.position)) <= .1f && isWalking)
        {
            transform.position = newRandomPos;
            animator.Play("Idle");
            isWalking = false;
        }
    }

    void FixedUpdate() 
    {
        GetDestination(target);
        if(isInfected) { return; }
        if(math.abs(Vector3.Distance(transform.position,target.position)) <= maxDistance)
        {
            hasDestination = true;
        }
        else
        {
            hasDestination = false;
            StopCoroutine(Escape(target));
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Vector3 direction = -(transform.position - other.transform.position).normalized;
            other.GetComponent<FirstPersonController>().Knockback(direction,transform);
        }
    }

}
