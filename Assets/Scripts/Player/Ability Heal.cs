using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHeal : MonoBehaviour
{
    [SerializeField] FPSAnimation fpsAnimator;

    [SerializeField] Transform playerCam;
    [SerializeField] LayerMask layerMask;

    [SerializeField] float range;
    [SerializeField] float lerpTime;

    [SerializeField] KeyCode keyCode;

    [SerializeField] float healGainXPAmount;

    bool isHealing;
    public void HealingTrue()
    {
        isHealing = true;
    }
    public void HealingFalse()
    {
        isHealing = false;
    }

    void Update() 
    {
        if(Physics.Raycast(playerCam.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, range,layerMask))
        {
            if(!hit.transform.GetComponent<Villager>().GetIsInfected || hit.transform.GetComponent<Villager>().GetIsDead) 
            {
                HideVisualArea();
                StopAllCoroutines();
                return;
            }
            if(Input.GetKeyDown(keyCode))
            {
                fpsAnimator.Heal();
            }
            else if(Input.GetKeyUp(keyCode) || hit.transform.GetComponent<Villager>().GetIsDead)
            {
                fpsAnimator.Idle();
                StopAllCoroutines();
                HideVisualArea();
            }
        }
        else
        {
            if(!isHealing) { return; }
            isHealing = false;
            fpsAnimator.Idle();
            StopAllCoroutines();
            HideVisualArea();
        }
    }

    public void HealAnimationEvent()
    {
        if(Physics.Raycast(playerCam.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, range,layerMask))
        {
            var villager = hit.transform.GetComponent<Villager>();
            if(villager.GetIsInfected)
            {
                XPPanel.Instance.IncreaseXP(healGainXPAmount);
                GameManager.Instance.SpawnFloatingText(villager.transform.position, ("+" + healGainXPAmount).ToString(), Color.cyan);
                villager.GetHealFromPlayer();
            }
        }
    }

    public void StartFillArea()
    {
        GameManager.Instance.HealFillAreaFillAmount = 0;
        StartCoroutine(StartRoutine());
    }

    public void HideVisualArea()
    {
        GameManager.Instance.HealFillAreaFillAmount = 0;
    }

    IEnumerator StartRoutine()
    {
        float elapsed = 0;
        float timer = 1.2f;
        float t = 0;
        while(true)
        {
            if(Physics.Raycast(playerCam.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, range,layerMask))
            {
                if(hit.transform.GetComponent<Villager>().GetIsDead)
                {
                    HideVisualArea();
                }
            }
            elapsed += Time.deltaTime;
            t = elapsed / timer;
            GameManager.Instance.HealFillAreaFillAmount = Mathf.Lerp(GameManager.Instance.HealFillAreaFillAmount, 1, Time.deltaTime * t);
            if(GameManager.Instance.HealFillAreaFillAmount + .016f >= 1)
            {
                GameManager.Instance.HealFillAreaFillAmount = 1;
                StopCoroutine(StartRoutine());
                break;
            }
            yield return null;
        }
        StopCoroutine(StartRoutine());
    }

}
