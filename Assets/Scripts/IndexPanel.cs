using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndexPanel : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI currentHPText;
    [SerializeField] TextMeshProUGUI maxHPText;

    [Header("Mana")]
    [SerializeField] Slider manaSlider;
    [SerializeField] TextMeshProUGUI currentManaText;
    [SerializeField] TextMeshProUGUI maxManaText;

    [Header("Stamina")]
    [SerializeField] Slider staminaSlider;
    [SerializeField] TextMeshProUGUI currentStaminaText;
    [SerializeField] TextMeshProUGUI maxStaminaText;

    void Start() 
    {
        UpdateMaxValues();   
        StartCoroutine(UpdateCurrentValues());
    }

    IEnumerator UpdateCurrentValues()
    {
        while(true)
        {
            yield return new WaitForSeconds(.1f);
            currentHPText.text = ((int)hpSlider.value).ToString();

            currentManaText.text = ((int)manaSlider.value).ToString();

            currentStaminaText.text = ((int)staminaSlider.value).ToString();
        }
    } 

    public void UpdateMaxValues()
    {
        maxHPText.text = hpSlider.maxValue.ToString();

        maxManaText.text = manaSlider.maxValue.ToString();

        maxStaminaText.text = staminaSlider.maxValue.ToString();
    }

}
