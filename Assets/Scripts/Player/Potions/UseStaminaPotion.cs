using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseStaminaPotion : MonoBehaviour
{
    [SerializeField] float staminaAmount;

    [SerializeField] KeyCode keyCode;

    void Update() 
    {
        if(Input.GetKeyDown(keyCode))
        {
            if(Stamina.Instance.GetCurrentStamina >= Stamina.Instance.MaxStamina - .3f || StaminaPotions.Instance.GetCurrentCount <= 0) { return; }
            Stamina.Instance.IncreaseStaminaWithPotion(staminaAmount);
            StaminaPotions.Instance.DecreasePotionCount();
        }    
    }

}
