using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    [SerializeField] GameObject instantiatePrefab;

    [SerializeField] Graveyard graveyard;

    [SerializeField] Transform spawnPoint;

    public void InstantiateObject()
    {
        var obj = Instantiate(instantiatePrefab,spawnPoint.position,Quaternion.identity);
        graveyard.AddSkeleton(obj.GetComponent<Skeleton>());
    }

}
