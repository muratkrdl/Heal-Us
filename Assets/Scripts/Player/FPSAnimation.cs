using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] AbilityFireball abilityFireball;
    [SerializeField] AbilityLightning abilityLightning;
    [SerializeField] AbilityPoison abilityPoison;
    [SerializeField] UseIcePotion useIcePotion;
    [SerializeField] AbilityHeal abilityHeal;

#region CanUseAbility
    public void SetFalseCanUseAbility()
    {
        if(GameManager.Instance.GetCanUseAbility)
        {
            GameManager.Instance.SetFalseCanUseAbility();
        }
    }
    public void SetTrueCanUseAbility()
    {
        if(!GameManager.Instance.GetCanUseAbility)
        {
            GameManager.Instance.SetTrueCanUseAbility();
        }    
    }
#endregion

#region Heal
    public void Heal()
    {
        animator.SetTrigger("heal");
    }
    public void HealAnimationEvent()
    {
        abilityHeal.HealAnimationEvent();
    }
    public void HealStartVisualArea()
    {
        abilityHeal.StartFillArea();
    }
    public void HealHideVisualArea()
    {
        abilityHeal.HideVisualArea();
    }
    public void HealingTrue()
    {
        abilityHeal.HealingTrue();
    }
    public void HealingFalse()
    {
        abilityHeal.HealingFalse();
    }
#endregion

#region IcePotion
    public void Throw()
    {
        animator.SetTrigger("throw");
    }
    public void IceAnimationEvent()
    {
        useIcePotion.ThroeIcePotion();
    }
    public void IceThrowSoundEvent()
    {
        SoundManager.Instance.PlaySound3D("Throw",transform.position);
    }
#endregion

#region Fireball
    public void Fireball()
    {
        animator.SetTrigger("fireball");
    }
    public void FireballAnimationEvent()
    {
        abilityFireball.ThrowFireBall();
    }
    public void FireBallSoundEvent()
    {
        SoundManager.Instance.PlaySound3D("Fireball",transform.position);
    }
#endregion

#region Lightning
    public void Lightning()
    {
        animator.SetTrigger("lightning");
    }
    public void LightningAnimationEvent()
    {
        abilityLightning.ThrowLightning();
    }
#endregion

#region Poison
    public void Poison()
    {
        animator.SetTrigger("poison");
    }
    public void PoisonAnimationEvent()
    {
        abilityPoison.ThrowIcePotion();
    }
    public void PoisonSoundEvent()
    {
        SoundManager.Instance.PlaySound3D("Poison",transform.position);
    }
#endregion

#region Walk
    public bool GetWalkAnimationBool()
    {
        return animator.GetBool("walk");
    } 
    public void SetWalkAnimationTrue()
    {
        animator.SetBool("walk",true);
    }
    public void SetWalkAnimationFalse()
    {
        animator.SetBool("walk",false);
    }
#endregion

#region Speed
    public void SetAnimatorSpeedNormal()
    {
        if(animator.speed == 1) { return; }
        animator.speed = 1f;
    }
    public void SetAnimatorSpeedSpeedUp()
    {
        if(animator.speed == 2 || !GameManager.Instance.GetCanUseAbility) { return; }
        animator.speed = 2f;
    }
#endregion

    public void Idle()
    {
        animator.SetTrigger("idle");
    }

}
