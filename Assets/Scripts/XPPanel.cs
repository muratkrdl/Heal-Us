using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPPanel : MonoBehaviour
{
    public static XPPanel Instance { get; private set; }

    [SerializeField] GameObject levelUpPanel;

    [SerializeField] Slider xpSlider;

    [SerializeField] float maxXP;
    [SerializeField] float lerpTime;

    [SerializeField] float increaseAmountMaxXP;

    float currentXP;

    float targetXP;

    public float MaxXP
    {
        get
        {
            return maxXP;
        }
        set
        {
            maxXP = value;
        }
    }

    public float GetCurrentHP
    {
        get
        {
            return currentXP;
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

        xpSlider.maxValue = maxXP;
        currentXP = 0;
        targetXP = currentXP;
        xpSlider.value = currentXP;
    }

    public void IncreaseXP(float amount)
    {
        StartCoroutine(SmoothHPVisual(amount));
    }

    IEnumerator SmoothHPVisual(float amount)
    {
        targetXP += amount;
        while(true)
        {
            yield return null;
            if(amount > 0 && currentXP >= maxXP) { currentXP = maxXP; targetXP = currentXP; } 
            currentXP = Mathf.Lerp(currentXP, targetXP, Time.deltaTime * lerpTime);
            xpSlider.value = currentXP;

            if(Mathf.Abs(currentXP - targetXP) <= .01f)
            {
                xpSlider.value = targetXP;
                break;
            }
        }
        if(Mathf.Abs(currentXP - maxXP) <= .2f)
        {
            currentXP = 0;
            targetXP = 0;
            maxXP += increaseAmountMaxXP;
            UpdateVisual();
            levelUpPanel.SetActive(true);
        }
        StopAllCoroutines();
    }

    public void UpdateVisual()
    {
        xpSlider.maxValue = maxXP;
        xpSlider.value = currentXP;
    }

}
