using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseManaPotion : MonoBehaviour
{
    [SerializeField] float manaAmount;

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(Mana.Instance.GetCurrentMana >= Mana.Instance.GetMaxMana - 1 || ManaPotions.Instance.GetCurrentCount <= 0) { return; }
            Mana.Instance.IncreaseMana(manaAmount);
            ManaPotions.Instance.DecreasePotionCount();
        }    
    }

}
