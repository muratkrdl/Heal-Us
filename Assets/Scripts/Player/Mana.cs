using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    public static Mana Instance { get; private set; }

    [SerializeField] Slider manaSlider;

    [SerializeField] float maxMana;
    [SerializeField] float lerpTime;

    float currentMana;

    float targetMana;

    public float GetMaxMana
    {
        get
        {
            return maxMana;
        }
    }

    public float GetCurrentMana
    {
        get
        {
            return currentMana;
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

        manaSlider.maxValue = maxMana;
        currentMana = maxMana;
        targetMana = currentMana;
        manaSlider.value = currentMana;
    }

    public void IncreaseMana(float amount)
    {
        StartCoroutine(SmoothManaVisual(amount));
    }

    public void DecreaseMana(float amount)
    {
        StartCoroutine(SmoothManaVisual(-amount));
    }

    IEnumerator SmoothManaVisual(float amount)
    {
        targetMana += amount;
        while(true)
        {
            yield return null;
            if(amount > 0 && currentMana >= maxMana) { currentMana = maxMana; targetMana = currentMana; } 
            currentMana = Mathf.Lerp(currentMana, targetMana, Time.deltaTime * lerpTime);
            manaSlider.value = currentMana;
            if(Mathf.Abs(currentMana - targetMana) <= .2f)
            {
                break;
            }
        }
        StopAllCoroutines();
    }

}
