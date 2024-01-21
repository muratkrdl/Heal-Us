using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseIcePotion : MonoBehaviour
{
    [SerializeField] GameObject iceAbility;
    [SerializeField] Transform throwPosition;

    [SerializeField] float range;

    [SerializeField] KeyCode keyCode;

    void Update() 
    {
        if(Input.GetKeyDown(keyCode) && GameManager.Instance.GetCanUseAbility)
        {
            if(IcePotions.Instance.GetCurrentCount <= 0) { return; }
            //Use Skill
            var ice = Instantiate(iceAbility,throwPosition.position,Quaternion.identity);
            ice.GetComponent<AbilityIcePotion>().ThrowPotion(RaycastForMousePos());

            IcePotions.Instance.DecreasePotionCount();
        }
    }

    Vector3 RaycastForMousePos()
    {
        Vector3 direction = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitData, range))
        {
            direction = hitData.point - transform.position;
        }
        return direction;
    }
    
}
