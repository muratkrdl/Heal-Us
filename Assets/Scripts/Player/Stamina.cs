using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public static Stamina Instance { get; private set; }

    [SerializeField] Slider staminaSlider;

    [SerializeField] float maxStamina;
    [SerializeField] float increaseSpeed;
    [SerializeField] float decreaseSpeed;

    [SerializeField] float waitForLoadTime;

    float currentStamina;

    bool waiting;
    bool canSprint;

    bool IsPressingLShift => Input.GetKey(KeyCode.LeftShift);

    public float GetCurrentStamina
    {
        get
        {
            return currentStamina;
        }
    }

    public float GetMaxStamina
    {
        get
        {
            return maxStamina;
        }
    }

    public bool GetCanSprint
    {
        get
        {
            return canSprint;
        }
    }

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

        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
    }

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && !waiting)
        {
            StopAllCoroutines();
            StartCoroutine(DecreaseStamina());
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            StartCoroutine(IncreaseStamina());
        }

        if(!IsPressingLShift) { canSprint = false; }
    }

    IEnumerator DecreaseStamina()
    {
        while(IsPressingLShift)
        {
            canSprint = true;
            yield return null;
            currentStamina -= Time.deltaTime * decreaseSpeed;
            staminaSlider.value = currentStamina;
            if(currentStamina <= staminaSlider.minValue + .2f)
            {
                StartCoroutine(WaitForLoad());
                StartCoroutine(IncreaseStamina());
                canSprint = false;
                break;
            }
        }
        StopCoroutine(DecreaseStamina());
    }

    IEnumerator IncreaseStamina()
    {
        StopCoroutine(DecreaseStamina());
        while(true)
        {
            yield return null;
            currentStamina += Time.deltaTime * increaseSpeed;
            staminaSlider.value = currentStamina;
            if(currentStamina >= maxStamina - .1f)
            {
                break;
            }
        }
        StopCoroutine(IncreaseStamina());
    }

    public void IncreaseStaminaWithPotion(float amount)
    {
        currentStamina += amount;
        staminaSlider.value = currentStamina;
        if(waiting) { waiting = false; StopCoroutine(WaitForLoad()); }
        if(currentStamina >= maxStamina) { currentStamina = maxStamina;}
    }

    IEnumerator WaitForLoad()
    {
        waiting = true;
        yield return new WaitForSeconds(waitForLoadTime);
        waiting = false;
        StopCoroutine(WaitForLoad());
    }

}
