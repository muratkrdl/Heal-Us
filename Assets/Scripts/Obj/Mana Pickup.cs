using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            if(ManaPotions.Instance.GetCurrentCount >= ManaPotions.Instance.GetMaxCount) { return; }
            ManaPotions.Instance.IncreasePotionCount();
            Destroy(gameObject);
        }
    }

}
