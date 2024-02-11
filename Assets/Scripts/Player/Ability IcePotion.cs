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
        Instantiate(IceFX,transform.position,Quaternion.identity);
        IceFX.GetComponent<Ice>().SetSlowAmount = slowAmount;
        Destroy(gameObject,.1f);
    }

}
