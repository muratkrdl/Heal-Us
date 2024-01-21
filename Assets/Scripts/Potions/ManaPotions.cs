using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotions : PotionsAbstract
{
    public static ManaPotions Instance { get; private set; }

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
