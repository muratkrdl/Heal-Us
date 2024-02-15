using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseManaPotion : MonoBehaviour
{
    [SerializeField] float manaAmount;

    [SerializeField] KeyCode keyCode;

    void Update() 
    {
        if(Input.GetKeyDown(keyCode))
        {
            if(Mana.Instance.GetCurrentMana >= Mana.Instance.MaxMana - 1 || ManaPotions.Instance.GetCurrentCount <= 0) { return; }
            SoundManager.Instance.PlaySound3D("Drink",transform.position);
            Mana.Instance.IncreaseMana(manaAmount);
            ManaPotions.Instance.DecreasePotionCount();
        }    
    }

}
