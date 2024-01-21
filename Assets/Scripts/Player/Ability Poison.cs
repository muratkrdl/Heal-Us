using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class AbilityPoison : MonoBehaviour
{
    [SerializeField] Poison poisonPrefab;

    [SerializeField] FirstPersonController firstPersonController;

    [SerializeField] KeyCode keyCode;

    [SerializeField] int abilityCD;

    int currentAbilityCD;

    bool canUsePoison;

    float initialSpeed;

    void Awake() 
    {
        canUsePoison = true;
        currentAbilityCD = 0;
        initialSpeed = firstPersonController.MoveSpeed;
    }

    void Update() 
    {
        if(Mana.Instance.GetCurrentMana < poisonPrefab.GetManaCost)
        {
            return;
        }
        if(Input.GetKeyDown(keyCode) && canUsePoison && GameManager.Instance.GetCanUseAbility)
        {
            StartCoroutine(OpenPoisonAbility());
        }    
    }

    IEnumerator OpenPoisonAbility()
    {
        canUsePoison = false;
        Mana.Instance.DecreaseMana(poisonPrefab.GetComponent<Poison>().GetManaCost);

        poisonPrefab.OpenPoison();
        StartCoroutine(StartLifeCounter());

        currentAbilityCD = abilityCD;
        GameManager.Instance.EnablePoisonCDCounter();
        GameManager.Instance.SetPoisonCDText(currentAbilityCD.ToString());
      
        for (int i = 0; i < abilityCD; i++)
        {
            yield return new WaitForSeconds(1);
            currentAbilityCD--;
            GameManager.Instance.SetPoisonCDText(currentAbilityCD.ToString());
        }
       
        canUsePoison = true;
        StopCoroutine(OpenPoisonAbility());
    }

    IEnumerator StartLifeCounter()
    {
        firstPersonController.MoveSpeed = initialSpeed /2;
        firstPersonController.SprintSpeed = initialSpeed;
        yield return new WaitForSeconds(poisonPrefab.GetLifeTime);
        poisonPrefab.ClosePoison();
        firstPersonController.MoveSpeed = initialSpeed;
        firstPersonController.SprintSpeed = initialSpeed *2;
        StopCoroutine(StartLifeCounter());
    }

}
