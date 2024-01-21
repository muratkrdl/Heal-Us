using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyExplosion : MonoBehaviour
{
    [SerializeField] float damage;

    void Start() 
    {
        Destroy(gameObject,2);    
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            PlayerHP.Instance.DecreaseHP(damage);
        }
    }

}
