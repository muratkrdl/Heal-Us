using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] float stunTime;
    [SerializeField] float manaCost;
    [SerializeField] float damage;

    [SerializeField] GameObject missedLightningFXPrefab;

    public float GetDamage
    {
        get
        {
            return damage;
        }
    }

    public float GetManaCost
    {
        get
        {
            return manaCost;
        }
    }

    void Start() 
    {
        StartCoroutine(KYS());
    }

    IEnumerator KYS()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
        StopAllCoroutines();
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Monster"))
        {
            MonsterHP.Instance.DecreaseHP(damage);
            other.GetComponent<Monster>().MonsterStunned(stunTime,true);
        }
    }

}
