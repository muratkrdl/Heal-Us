using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Fireball")]
    [SerializeField] Image fireballBG;
    [SerializeField] TextMeshProUGUI fireballCDText;

    [Header("Lightning")]
    [SerializeField] Image lightningBG;
    [SerializeField] TextMeshProUGUI lightningCDText;

    [Header("Poison")]
    [SerializeField] Image poisonBG;
    [SerializeField] TextMeshProUGUI poisonCDText;

    [SerializeField] Villager[] villagers;
    [SerializeField] Transform monster;
    [SerializeField] Transform player;

    [SerializeField] Image healFillArea;

    bool canUseAbility;

    void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    
        DisableFireballCDCounter();
        DisableLightningCDCounter();
        DisablePoisonCDCounter();
        canUseAbility = true;
        healFillArea.fillAmount = 0;
    }

    public float HealFillAreaFillAmount
    {
        get
        {
            return healFillArea.fillAmount;
        }
        set
        {
            healFillArea.fillAmount = value;
        }
    }

#region CanUseAbility
    public bool GetCanUseAbility
    {
        get
        {
            return canUseAbility;
        }
    }
    public void SetFalseCanUseAbility()
    {
        canUseAbility = false;
    }
    public void SetTrueCanUseAbility()
    {
        canUseAbility = true;
    }
#endregion

#region FireBallRegion
    public void SetFireBallCDText(string text)
    {
        fireballCDText.text = text;
        if(text == 0.ToString())
        {
            DisableFireballCDCounter();
        }
    }

    public void EnableFireballCDCounter()
    {
        fireballCDText.enabled = true;
        fireballBG.enabled = true;
    }

    public void DisableFireballCDCounter()
    {
        fireballCDText.enabled = false;
        fireballBG.enabled = false;
    }
#endregion

#region LightningRegion
    public void SetLightningCDText(string text)
    {
        lightningCDText.text = text;
        if(text == 0.ToString())
        {
            DisableLightningCDCounter();
        }
    }

    public void EnableLightningCDCounter()
    {
        lightningCDText.enabled = true;
        lightningBG.enabled = true;
    }

    public void DisableLightningCDCounter()
    {
        lightningCDText.enabled = false;
        lightningBG.enabled = false;
    }
#endregion

#region PoisonRegion
    public void SetPoisonCDText(string text)
    {
        poisonCDText.text = text;
        if(text == 0.ToString())
        {
            DisablePoisonCDCounter();
        }
    }

    public void EnablePoisonCDCounter()
    {
        poisonCDText.enabled = true;
        poisonBG.enabled = true;
    }

    public void DisablePoisonCDCounter()
    {
        poisonCDText.enabled = false;
        poisonBG.enabled = false;
    }
#endregion

    public bool CheckGameLose()
    {
        foreach (Villager villager in villagers)
        {
            if(!villager.GetIsInfected)
            {
                return false;
            }
        }
        return true;
    }

    public Transform GetNewTarget(Transform target)
    {
        foreach (Villager villager in villagers)
        {
            if(villager.GetIsInfected) 
            {
                continue;
            }
            else
            {
                float a = Mathf.Abs(Vector3.Distance(villager.transform.position, monster.transform.position));
                float b = Mathf.Abs(Vector3.Distance(target.position, monster.position));
                if(target.GetComponent<Villager>() != null && target.GetComponent<Villager>().GetIsInfected) { b = Mathf.Infinity; }
                if(a < b) 
                {
                    target = villager.transform;
                }
            }
        }
        return target;
    }

    public Transform GetPlayer
    {
        get
        {
            return player;
        }
    }

    public Transform GetMonster
    {
        get
        {
            return monster;
        }
    }

}
