using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] Transform playerCam;

    [SerializeField] float range;
    [SerializeField] LayerMask layerMask;

    [SerializeField] KeyCode keyCode;

    bool hasKey;
    bool gateOpen;
    bool leverOn;

    void Start() 
    {
        gateOpen = false;
        leverOn = false;
        hasKey = false;
    }

    void Update() 
    {
        Debug.DrawRay(playerCam.position, transform.TransformDirection(Vector3.forward),Color.red,range);
        InteractObj();
    }

    void InteractObj()
    {
        if(Physics.Raycast(playerCam.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, range,layerMask))
        {
            if(Input.GetKeyDown(keyCode))
            {
                if(hit.transform.CompareTag("Lever"))
                {
                    if(leverOn) { return; }
                    SoundManager.Instance.PlaySound3D("Lever",hit.transform.position);
                    hit.transform.GetComponent<Animator>().SetTrigger("On");
                    leverOn = true;
                }
                else if(hit.transform.CompareTag("Gate"))
                {
                    if(gateOpen) { return; }
                    if(!hasKey)
                    {
                        GameManager.Instance.SpawnFloatingText(hit.transform.position,"YOU NEED KEY",Color.red);
                        return;
                    }
                    SoundManager.Instance.PlaySound3D("Door",hit.transform.position);
                    hit.transform.GetComponent<Animator>().SetTrigger("Open");
                    gateOpen = true;
                }
                else if(hit.transform.CompareTag("Key"))
                {
                    SoundManager.Instance.PlaySound3D("Get Key", hit.transform.position);
                    hasKey = true;
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
    
}
