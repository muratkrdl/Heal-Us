using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityIcePotion : MonoBehaviour
{
    [SerializeField] Rigidbody myRigidbody;
    [SerializeField] GameObject IceFX;

    [SerializeField] float rotateSpeed;
    [SerializeField] float throwSpeed;

    int slowAmount;
    Vector3 rotateVector;

    public int SetSlowAmount
    {
        set
        {
            slowAmount = value;
        }
    }

    void Start() 
    {
        rotateVector = Random.insideUnitSphere.normalized;
    }

    void FixedUpdate() 
    {
        transform.Rotate(rotateVector * rotateSpeed);
    }

    public void ThrowPotion(Vector3 direction)
    {
        myRigidbody.AddForce(direction * throwSpeed);
    }

    void OnCollisionEnter(Collision other) 
    {
        Vector3 dir = -(transform.position - GameManager.Instance.GetPlayer.position).normalized;
        dir.y = 0;
        SoundManager.Instance.PlaySound3D("Break Glass",transform.position);
        var iceVFX = Instantiate(IceFX,transform.position,Quaternion.identity);
        iceVFX.transform.position += dir * 3;
        IceFX.GetComponent<Ice>().SetSlowAmount = slowAmount;
        IceFX.GetComponent<Ice>().SetPercentSlowAmount();
        Destroy(gameObject,.1f);
    }

}
