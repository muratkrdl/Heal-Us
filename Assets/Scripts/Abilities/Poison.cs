using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    [SerializeField] ParticleSystem poisonVFX;

    [SerializeField] float lifeTime;
    [SerializeField] float manaCost;

    public float GetLifeTime
    {
        get
        {
            return lifeTime;
        }
    }

    public float GetManaCost
    {
        get
        {
            return manaCost;
        }
    }

    public void OpenPoison()
    {
        if(!poisonVFX.isPlaying)
            poisonVFX.Play();
    }

    public void ClosePoison()
    {
        if(poisonVFX.isPlaying)
            poisonVFX.Stop();
    }

}
