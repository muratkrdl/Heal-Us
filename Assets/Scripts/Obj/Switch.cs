using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] GameObject moveObject;

    public void AnimationEvent()
    {
        moveObject.SetActive(false);
    }

}
