using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] Rigidbody myRigidbody;

    [SerializeField] float speed;
    [SerializeField] float lifeTime;
    [SerializeField] float stunTime;
    [SerializeField] float lerpTime;
    [SerializeField] float manaCost;

    [SerializeField] float damage;

    [SerializeField] ParticleSystem[] efects;
    [SerializeField] Light ponitLight;
    [SerializeField] GameObject sphere;

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
        StartCoroutine(DeactiveBall());
        StartCoroutine(KYS(lifeTime));
    }
    
    public void ThrowBall(Vector3 direction)
    {
        transform.LookAt(transform.position + direction);
        myRigidbody.AddForce(direction * speed);
    }

    IEnumerator DeactiveBall()
    {
        yield return new WaitForSeconds(lifeTime);
        DisableBall();
    }

    void DisableBall()
    {
        StartCoroutine(AboutSphere());
        GetComponent<SphereCollider>().enabled = false;
        myRigidbody.velocity = Vector3.zero;
        DisableFX();
    }

    IEnumerator AboutSphere()
    {
        while(true)
        {
            yield return null;
            sphere.transform.localScale = Vector3.Lerp(sphere.transform.localScale,Vector3.zero, lerpTime * Time.deltaTime);
            ponitLight.range = Mathf.Lerp(ponitLight.range,0,lerpTime * Time.deltaTime);
            if(sphere.transform.localScale.x <= .1f)
            {
                sphere.SetActive(false);
                StopCoroutine(AboutSphere());
                break;
            }
        }
    }

    IEnumerator KYS(float time)
    {
        yield return new WaitForSeconds(time + 2);
        gameObject.SetActive(false);
        StopAllCoroutines();
    }

    void DisableFX()
    {
        ponitLight.enabled = false;
        foreach (var item in efects)
        {
            item.Stop();
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Monster"))
        {
            StopAllCoroutines();
            //Low Damage
            if(other.TryGetComponent<Monster>(out var monster))
            {
                monster.MonsterStunned(stunTime,false);
            }
            else if(other.TryGetComponent<StoneMonster>(out var stoneMonster))
            {
                stoneMonster.StartKYS();
            }
            DisableBall();
            StartCoroutine(KYS(0));
        }
    }

}
