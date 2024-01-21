using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            if(StaminaPotions.Instance.GetCurrentCount >= StaminaPotions.Instance.GetMaxCount) { return; }
            StaminaPotions.Instance.IncreasePotionCount();
            Destroy(gameObject);
        }
    }
}
