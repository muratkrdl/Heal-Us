using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseHPPotion : MonoBehaviour
{
    [SerializeField] float hpAmount;

    [SerializeField] KeyCode keyCode;

    void Update() 
    {
        if(Input.GetKeyDown(keyCode))
        {
            if(PlayerHP.Instance.GetCurrentHP >= PlayerHP.Instance.MaxHP - 1 || HPPotions.Instance.GetCurrentCount <= 0) { return; }
            SoundManager.Instance.PlaySound3D("Drink",transform.position);
            PlayerHP.Instance.IncreaseHP(hpAmount);
            HPPotions.Instance.DecreasePotionCount();
        }    
    }

}
