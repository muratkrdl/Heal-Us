using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AbilityFireball : MonoBehaviour
{
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] Transform fireballSpawnPosition;

    [SerializeField] FPSAnimation fpsAnimator;

    [SerializeField] Transform lookPos;

    [SerializeField] KeyCode keyCode;

    [SerializeField] int abilityCD;

    [SerializeField] float damage;
    [SerializeField] int stunTime;

    public int StunTime
    {
        get
        {
            return stunTime;
        }
        set
        {
            stunTime = value;
        }
    }

    int currentAbilityCD;

    bool canThrowball;

    void Awake() 
    {
        canThrowball = true;
        currentAbilityCD = 0;
    }

    void Update() 
    {
        if(Mana.Instance.GetCurrentMana < fireballPrefab.GetComponent<FireBall>().GetManaCost)
        {
            return;
        }
        if(Input.GetKeyDown(keyCode) && canThrowball && GameManager.Instance.GetCanUseAbility)
        {
            fpsAnimator.Fireball();
        }    
    }

    public void ThrowFireBall()
    {
        StartCoroutine(ThrowBall());
    }

    IEnumerator ThrowBall()
    {
        canThrowball = false;
        Mana.Instance.DecreaseMana(fireballPrefab.GetComponent<FireBall>().GetManaCost);

        Vector3 direction = (lookPos.position - transform.position).normalized;

        var fireball = Instantiate(fireballPrefab,fireballSpawnPosition.position,Quaternion.identity);
        fireball.GetComponent<FireBall>().ThrowBall(direction);
        fireball.GetComponent<FireBall>().SetFireBallDamage = damage;
        fireball.GetComponent<FireBall>().SetStunTime = stunTime;

        currentAbilityCD = abilityCD;
        GameManager.Instance.EnableFireballCDCounter();
        GameManager.Instance.SetFireBallCDText(currentAbilityCD.ToString());
      
        for (int i = 0; i < abilityCD; i++)
        {   
            yield return new WaitForSeconds(1);
            currentAbilityCD--;
            GameManager.Instance.SetFireBallCDText(currentAbilityCD.ToString());
        }
       
        canThrowball = true;
        StopCoroutine(ThrowBall());
    }

}
