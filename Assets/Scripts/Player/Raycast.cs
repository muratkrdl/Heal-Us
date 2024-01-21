using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    [SerializeField] Transform playerCam;
    [SerializeField] float range;
    [SerializeField] LayerMask layerMask;

    void Update() 
    {
        //Debug.DrawRay(playerCam.position, transform.TransformDirection(Vector3.forward) * range, Color.red);
        if(Input.GetKeyDown(KeyCode.E))
        {
            RaycastObject();
        }
    }

    void RaycastObject()
    {
        if(Physics.Raycast(playerCam.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, range,layerMask))
        {
            var villager = hit.transform.GetComponent<Villager>();
            if(villager.isInfected)
            {
                Debug.Log("healed");
                villager.isInfected = false;
                //heal animation;
            }
            else
            {
                Debug.Log("No infected villager");
            }
        }
    }

}
