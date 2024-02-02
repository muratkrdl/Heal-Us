using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;

public class AbilityLightning : MonoBehaviour
{
    [SerializeField] GameObject lightningPrefab;

    [SerializeField] FPSAnimation fpsAnimator;

    [SerializeField] float range;

    [SerializeField] KeyCode keyCode;

    [SerializeField] int abilityCD;
    
    int currentAbilityCD;

    bool canUseLightning;

    Vector3 mousePos;
    Vector3 startPos;
    Vector3 endPos;

    void Awake() 
    {
        canUseLightning = true;
        currentAbilityCD = 0;
    }

    void Update() 
    {
        if(Mana.Instance.GetCurrentMana < lightningPrefab.GetComponent<Lightning>().GetManaCost)
        {
            return;
        }
        if(Input.GetKeyDown(keyCode) && canUseLightning && GameManager.Instance.GetCanUseAbility)
        {
            fpsAnimator.Lightning();
        }
    }

    public void ThrowLightning()
    {
        StartCoroutine(UseLightning());
    }

    IEnumerator UseLightning()
    {
        canUseLightning = false;
        Mana.Instance.DecreaseMana(lightningPrefab.GetComponent<Lightning>().GetManaCost);

        RaycastForMousePos();

        var lightning = Instantiate(lightningPrefab,mousePos,Quaternion.identity);

        endPos = mousePos + new Vector3(0,-5,0);
        startPos = endPos + new Vector3(0,20,0);
        lightning.GetComponent<LightningBoltScript>().StartPosition = startPos;
        lightning.GetComponent<LightningBoltScript>().EndPosition = endPos;

        currentAbilityCD = abilityCD;
        GameManager.Instance.EnableLightningCDCounter();
        GameManager.Instance.SetLightningCDText(currentAbilityCD.ToString());
      
        for (int i = 0; i < abilityCD; i++)
        {   
            yield return new WaitForSeconds(1);
            currentAbilityCD--;
            GameManager.Instance.SetLightningCDText(currentAbilityCD.ToString());
        }
       
        canUseLightning = true;
        StopCoroutine(UseLightning());
    }

    void RaycastForMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitData, range))
        {
            mousePos = hitData.point;
        }
    }

}
