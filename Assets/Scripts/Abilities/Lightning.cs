using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] float stunTime;
    [SerializeField] float manaCost;

    [SerializeField] BoxCollider boxCollider;

    int damage;

    public int SetDamage
    {
        set
        {
            damage = value;
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
        //SoundManager.Instance.PlaySound3D("Lightning",transform.position);
        StartCoroutine(KYS());
    }

    IEnumerator KYS()
    {
        yield return new WaitForSeconds(lifeTime);
        GetComponent<LineRenderer>().enabled = false;
        boxCollider.enabled = false;
        yield return new WaitForSeconds(lifeTime * 4);
        Destroy(gameObject);
        StopAllCoroutines();
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Monster"))
        {
            if(other.TryGetComponent<Monster>(out var monster))
            {
                boxCollider.enabled = false;
                GameManager.Instance.SpawnFloatingText(GameManager.Instance.GetMonster.position, damage.ToString(), Color.red);
                MonsterHP.Instance.DecreaseHP(damage * DamageMultiplier.Instance.GetDamageMultiplierValue);
                other.GetComponent<Monster>().MonsterStunned(stunTime,true);
            }
            else if(other.TryGetComponent<StoneMonster>(out var stoneMonster))
            {
                stoneMonster.StartKYS();
            }
        }
    }

}
