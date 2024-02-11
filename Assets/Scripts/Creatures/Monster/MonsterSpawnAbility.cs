using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnAbility : MonoBehaviour
{
    [SerializeField] Transform stoneMonsterSpawnPosition;

    [SerializeField] GameObject stoneMonsterPrefab;

    [SerializeField] int minSpawnMonsterCount;
    [SerializeField] int maxSpawnMonsterCount;

    int monsterCount;

    Vector3 spawnPos;

    public void StartSpawnStoneMonsters()
    {
        StartCoroutine(SpawnStoneMonsters());
    }

    IEnumerator SpawnStoneMonsters()
    {
        monsterCount = Random.Range(minSpawnMonsterCount,maxSpawnMonsterCount);
        spawnPos = transform.position + transform.forward * 2;
        while(true)
        {
            for (int i = 0; i < monsterCount; i++)
            {
                var monster = Instantiate(stoneMonsterPrefab,transform.position,Quaternion.identity);
                monster.transform.position = spawnPos;
                if(i %2 == 1) { spawnPos = transform.position + -transform.right * 1.5f;  }
                else { spawnPos = transform.position + transform.right * 1.5f;  }
                yield return new WaitForSeconds(.2f);
            }
            StopCoroutine(SpawnStoneMonsters());
            break;
        }
    }

}
