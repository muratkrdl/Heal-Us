using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] Monster monster;
    [SerializeField] Villager[] villagers;
    [SerializeField] Transform player;

    [SerializeField] float increaseDamageMultiplier;

    float damageMultiplier;

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
    }

#region CanUseAbility
    public bool GetCanUseAbility
    {
        get
        {
            return canUseAbility;
        }
    }
    public bool SetCanUseAbility
    {
        set
        {
            canUseAbility = value;
        }
    }
#endregion

#region DamageMultiplier
    public float DamageMultiplier
    {
        get
        {
            return damageMultiplier;
        }
    }
    public void CalculatePlayerDamage()
    {
        damageMultiplier = 0;
        foreach(var item in villagers)
        {
            if(!item.isInfected)
            {
                damageMultiplier += increaseDamageMultiplier;
            }
        }
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
        foreach (var item in villagers)
        {
            if(!item.isInfected)
            {
                return false;
            }
        }
        return true;
    }

    public Transform GetNewVillage(Transform target)
    {
        foreach (Villager villager in villagers)
        {
            if(target == null) { target = villagers[0].transform; }
            if(villager.GetComponent<Villager>().isInfected) 
            { 
                if(CheckGameLose())
                {
                    monster.SetMaxDistanceForPlayer(60);
                    target = GetPlayer;
                } 
                continue; 
            }
            if(Mathf.Abs(Vector3.Distance(villager.transform.position,transform.position)) < Mathf.Abs(Vector3.Distance(target.position,transform.position)))
            {
                target = villager.transform;
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

    public Monster GetMonster
    {
        get
        {
            return monster;
        }
    }

}
