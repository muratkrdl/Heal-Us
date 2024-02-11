using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseIcePotion : MonoBehaviour
{
    [SerializeField] GameObject iceAbility;
    [SerializeField] Transform throwPosition;

    [SerializeField] FPSAnimation fpsAnimator;

    [SerializeField] float range;
    [SerializeField] int slowAmount;

    [SerializeField] KeyCode keyCode;

    public int SlowAmount
    {
        get
        {
            return slowAmount;
        }
        set
        {
            slowAmount = value;
        }
    }

    void Update() 
    {
        if(Input.GetKeyDown(keyCode) && GameManager.Instance.GetCanUseAbility)
        {
            if(IcePotions.Instance.GetCurrentCount <= 0) { return; }
            fpsAnimator.Throw();
        }
    }

    public void ThroeIcePotion()
    {
        var ice = Instantiate(iceAbility,throwPosition.position,Quaternion.identity);
        ice.GetComponent<AbilityIcePotion>().ThrowPotion(RaycastForMousePos());
        ice.GetComponent<AbilityIcePotion>().SetSlowAmount = slowAmount;
        IcePotions.Instance.DecreasePotionCount();
    }

    Vector3 RaycastForMousePos()
    {
        Vector3 direction = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitData, range))
        {
            direction = (hitData.point - transform.position).normalized;
        }
        return direction;
    }
    
}
