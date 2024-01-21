using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PotionsAbstract : MonoBehaviour
{
    public Image BG;
    public TextMeshProUGUI potionCountText;

    public int maxCount;
    public int currentCount;
    public int startCount;

    public int GetMaxCount
    {
        get
        {
            return maxCount;
        }
    }

    public int GetCurrentCount
    {
        get
        {
            return currentCount;
        }
    }

    public void IncreasePotionCount()
    {
        if(currentCount >= maxCount) { return; }
        currentCount++;
        SetPotionCount();
    }

    public void DecreasePotionCount()
    {
        if(currentCount <= 0) { return; }
        currentCount--;
        SetPotionCount();
    }

    public void SetPotionCount()
    {
        potionCountText.text = currentCount.ToString();
        if(currentCount <= 0) { BG.enabled = true; } else { BG.enabled = false; }
    }

}
