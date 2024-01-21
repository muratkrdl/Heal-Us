using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterExplosionAbility : MonoBehaviour
{
    [SerializeField] GameObject energyExplosion;

    public void SpawnEnergyExplosion()
    {
        var explosion = Instantiate(energyExplosion,transform.position,Quaternion.identity);
        explosion.transform.position = GameManager.Instance.GetPlayer.position;
    }
    
}
