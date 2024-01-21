using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            if(IcePotions.Instance.GetCurrentCount >= IcePotions.Instance.GetMaxCount) { return; }
            IcePotions.Instance.IncreasePotionCount();
            Destroy(gameObject);
        }
    }
}
