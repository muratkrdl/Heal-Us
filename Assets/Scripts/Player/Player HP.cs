using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public static PlayerHP Instance { get; private set; }

    [SerializeField] Slider hpSlider;

    [SerializeField] float maxHP;
    [SerializeField] float lerpTime;

    float currentHP;

    float targetHP;

    public float MaxHP
    {
        get
        {
            return maxHP;
        }
        set
        {
            maxHP = value;
        }
    }

    public float GetCurrentHP
    {
        get
        {
            return currentHP;
        }
    }

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

        hpSlider.maxValue = maxHP;
        currentHP = maxHP;
        targetHP = currentHP;
        hpSlider.value = currentHP;
    }

    public void IncreaseHP(float amount)
    {
        StartCoroutine(SmoothHPVisual(amount,true));
    }

    public void DecreaseHP(float amount)
    {
        StartCoroutine(SmoothHPVisual(-amount,false));
    }

    IEnumerator SmoothHPVisual(float amount,bool increase)
    {
        targetHP += amount;
        while(true)
        {
            yield return null;
            if(amount > 0 && currentHP >= maxHP) { currentHP = maxHP; targetHP = currentHP; } 
            currentHP = Mathf.Lerp(currentHP, targetHP, Time.deltaTime * lerpTime);
            hpSlider.value = currentHP;

            if(currentHP <= 0 + .2f)
            {
                //Die
            }

            if(Mathf.Abs(currentHP - targetHP) <= .02f)
            {
                if(increase) { currentHP += 1; }
                Mathf.RoundToInt(currentHP);
                hpSlider.value = currentHP;
                break;
            }
        }
        StopAllCoroutines();
    }

    public void UpdateValue()
    {
        hpSlider.maxValue = maxHP;
    }

}
