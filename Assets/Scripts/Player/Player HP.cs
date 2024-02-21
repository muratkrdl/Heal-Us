using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public static PlayerHP Instance { get; private set; }

    [SerializeField] FirstPersonController firstPersonController;

    [SerializeField] Animator[] dieAnimator;

    [SerializeField] Slider hpSlider;

    [SerializeField] float maxHP;
    [SerializeField] float lerpTime;

    float currentHP;

    float targetHP;

    bool playerDead;

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
    }

    void Start() 
    {
        playerDead = false;
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
        while(!playerDead)
        {
            yield return null;
            if(amount > 0 && currentHP >= maxHP) { currentHP = maxHP; targetHP = currentHP; } 
            currentHP = Mathf.Lerp(currentHP, targetHP, Time.deltaTime * lerpTime);
            hpSlider.value = currentHP;

            if(currentHP <= 0 + .2f)
            {
                //Die
                playerDead = true;
                SoundManager.Instance.PlaySound3D("Die",GameManager.Instance.GetPlayer.position);
                DieAnimation();
                firstPersonController.enabled = false;
                GameManager.Instance.LoseGame();
            }

            if(Mathf.Abs(currentHP - targetHP) <= .02f)
            {
                if(increase) { currentHP += 1; }
                currentHP = targetHP;
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

    void DieAnimation()
    {
        foreach (var item in dieAnimator)
        {
            if(!item.enabled)
                item.enabled = true;
            item.SetTrigger("Die");
        }
    }

}
