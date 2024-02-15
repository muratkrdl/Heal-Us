using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPotions : PotionsAbstract
{
    public static HPPotions Instance { get; private set; }

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
