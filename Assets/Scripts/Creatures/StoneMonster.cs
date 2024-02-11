using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneMonster : MonoBehaviour
{

    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] ParticleSystem tailFX;
    [SerializeField] ParticleSystem dieFX;

    [SerializeField] SkinnedMeshRenderer myRenderer;

    [SerializeField] BoxCollider boxCollider;

    [SerializeField] float damage;

    [SerializeField] float minLifeTime;
    [SerializeField] float maxLifeTime;

    float lifeTime;

    void Start() 
    {
        transform.LookAt(GameManager.Instance.GetPlayer.transform);
        lifeTime = Random.Range(minLifeTime,maxLifeTime);
        StartCoroutine(LifeTimeKYS());
        StartCoroutine(StartDestination());
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            PlayerHP.Instance.DecreaseHP(damage);
            StopCoroutine(StartDestination());
            StartCoroutine(KYS());
        }
    }

    IEnumerator StartDestination()  
    {
        while(true)
        {
            yield return null;
            navMeshAgent.destination = GameManager.Instance.GetPlayer.position;
        }
    }

    public void StartKYS()
    {
        StartCoroutine(KYS());
    }

    IEnumerator LifeTimeKYS()
    {
        yield return new WaitForSeconds(lifeTime);
        StartCoroutine(KYS());
    }

    IEnumerator KYS()
    {
        myRenderer.enabled = false;
        boxCollider.enabled = false;
        tailFX.Stop();
        dieFX.Play();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        StopAllCoroutines();
    }

}
