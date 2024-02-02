using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityIcePotion : MonoBehaviour
{
    [SerializeField] Rigidbody myRigidbody;
    [SerializeField] GameObject IceFX;

    [SerializeField] float rotateSpeed;
    [SerializeField] float throwSpeed;

    Vector3 rotateVector;

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
        Destroy(gameObject,.1f);
    }

}
