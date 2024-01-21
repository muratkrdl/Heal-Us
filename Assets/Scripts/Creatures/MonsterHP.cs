using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHP : MonoBehaviour
{

    public static MonsterHP Instance { get; private set; }

    [SerializeField] Slider hpSlider;

    [SerializeField] float maxHP;
    [SerializeField] float lerpTime;

    float currentHP;

    float targetHP;

    public float GetMaxHP    
    {
        get
        {
            return maxHP;
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
        currentHP = maxHP /2;
        targetHP = currentHP;
        hpSlider.value = currentHP;
    }

    public void IncreaseHP(float amount)
    {
        StartCoroutine(SmoothHPVisual(amount));
    }

    public void DecreaseHP(float amount)
    {
        StartCoroutine(SmoothHPVisual(-amount));
    }

    IEnumerator SmoothHPVisual(float amount)
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

            if(Mathf.Abs(currentHP - targetHP) <= .2f)
            {
                break;
            }
        }
        StopAllCoroutines();
    }
}
