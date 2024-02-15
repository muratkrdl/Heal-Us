using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePotions : PotionsAbstract
{
    public static IcePotions Instance { get; private set; }

    void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentCount = startCount;
        SetPotionCount();
    }
}
