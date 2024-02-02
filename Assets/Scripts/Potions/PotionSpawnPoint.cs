using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawnPoint : MonoBehaviour
{
    [SerializeField] Vector2 spawnPotionsRange;

    [SerializeField] GameObject[] potionPrefabs;

    void Start() 
    {
        StartCoroutine(SpawnPotions());
    }

    public void StartTimerForPotion()
    {
        StartCoroutine(SpawnPotions());
    }

    IEnumerator SpawnPotions()
    {
        yield return new WaitForSeconds(Random.Range(spawnPotionsRange.x,spawnPotionsRange.y));
        var potion = Instantiate(potionPrefabs[Random.Range(0,potionPrefabs.Length)],transform.position,Quaternion.identity,transform);
        StopAllCoroutines();
    }

}
