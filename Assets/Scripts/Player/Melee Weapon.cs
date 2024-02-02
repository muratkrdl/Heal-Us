using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float range;

    [SerializeField] Animator animator;

    void Update() 
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(animator.GetBool("Attack")) { return; }
            animator.CrossFade("Attack",.1f);
        }
    }
    
    void RaycastForAnimationEvent()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hitData, range))
        {
            if(hitData.transform.CompareTag("Monster"))
            {
                if(hitData.transform.TryGetComponent<StoneMonster>(out var stoneMonster))
                {
                    stoneMonster.StartKYS();
                    return;
                }
                MonsterHP.Instance.DecreaseHP(damage);
            }
        }
    }

}
