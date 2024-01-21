using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseStaminaPotion : MonoBehaviour
{
    [SerializeField] float staminaAmount;

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            if(Stamina.Instance.GetCurrentStamina >= Stamina.Instance.GetMaxStamina - .3f || StaminaPotions.Instance.GetCurrentCount <= 0) { return; }
            Stamina.Instance.IncreaseStaminaWithPotion(staminaAmount);
            StaminaPotions.Instance.DecreasePotionCount();
        }    
    }

}
