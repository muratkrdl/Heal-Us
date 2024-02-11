using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollisionParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem poisonFX;

    [SerializeField] int poisonDamageCounter;
    [SerializeField] int poisonDamageDecreaseComplierAmount;

    bool isPoisened;

    int poisonComplier;

    public int PoisonDamage
    {
        get
        {
            return poisonDamageCounter;
        }
        set
        {
            poisonDamageCounter = value;
        }
    }

    void OnParticleCollision(GameObject other) 
    {
        poisonComplier++;
        if(!isPoisened) { StartCoroutine(GetDamageFromPoison()); }
    }

    IEnumerator GetDamageFromPoison()
    {
        isPoisened = true;
        poisonFX.Play();
        yield return new WaitForSeconds(2.85f);
        int damage = Mathf.RoundToInt(poisonComplier / poisonDamageDecreaseComplierAmount);
        for (int i = 0; i < poisonDamageCounter; i++)
        {
            MonsterHP.Instance.DecreaseHP(damage * DamageMultiplier.Instance.GetDamageMultiplierValue);
            yield return new WaitForSeconds(1);
        }
        poisonFX.Stop();
        poisonComplier = 0;
        isPoisened = false;
        StopCoroutine(GetDamageFromPoison());
    }

}
