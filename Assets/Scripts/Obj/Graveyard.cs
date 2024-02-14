using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graveyard : MonoBehaviour
{
    [SerializeField] Grave[] graves;

    [SerializeField] List<Skeleton> skeletons = new();

    [SerializeField] float spawnRate;

    bool spawned;

    void Start() 
    {
        spawned = false;    
    }

    public void AddSkeleton(Skeleton skeleton)
    {
        skeletons.Add(skeleton);
    }

    void OnTriggerEnter(Collider other) 
    {
        if(spawned) { return; }
        if(other.CompareTag("Player"))
        {
            spawned = true;
            StartCoroutine(SpawnSkeletons());
        }
    }

    void OnTriggerExit(Collider other) 
    {
        StopAllCoroutines();
        foreach (var item in skeletons)
        {
            item.ReturnSpawanPoint();
        }
    }

    IEnumerator SpawnSkeletons()
    {
        foreach (var item in graves)
        {
            yield return new WaitForSeconds(spawnRate);
            item.InstantiateObject();
        }
        StopCoroutine(SpawnSkeletons());
    }

}
