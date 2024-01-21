using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseHPPotion : MonoBehaviour
{
    [SerializeField] float hpAmount;

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(PlayerHP.Instance.GetCurrentHP >= PlayerHP.Instance.GetMaxHP - 1 || HPPotions.Instance.GetCurrentCount <= 0) { return; }
            PlayerHP.Instance.IncreaseHP(hpAmount);
            HPPotions.Instance.DecreasePotionCount();
        }    
    }

}
