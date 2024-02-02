using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationEvent : MonoBehaviour
{
    [SerializeField] Monster monster;

    public void MeleeAnimationEvent()
    {
        monster.MeleeAnimationEvent();
    }

    public void MeleeAnimationFinishEvent()
    {
        monster.MeleeAnimationFinishEvent();
    }

    public void WalkAnimationEvent()
    {
        monster.WalkAnimationEvent();
    }

    public void IdleAnimationEvent()
    {
        monster.IdleAnimationEvent();
    }

}
