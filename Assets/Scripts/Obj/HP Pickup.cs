using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            if(HPPotions.Instance.GetCurrentCount >= HPPotions.Instance.GetMaxCount) { return; }
            HPPotions.Instance.IncreasePotionCount();
            Destroy(gameObject);
        }
    }
}
