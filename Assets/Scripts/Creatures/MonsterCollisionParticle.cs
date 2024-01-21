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

    void OnParticleCollision(GameObject other) 
    {
        Debug.Log("hitted");
        poisonComplier++;
        if(!isPoisened) { StartCoroutine(GetPoisonDamage()); }
    }

    IEnumerator GetPoisonDamage()
    {
        isPoisened = true;
        poisonFX.Play();
        yield return new WaitForSeconds(2.85f);
        int damage = Mathf.RoundToInt(poisonComplier / poisonDamageDecreaseComplierAmount);
        for (int i = 0; i < poisonDamageCounter; i++)
        {
            MonsterHP.Instance.DecreaseHP(damage);
            Debug.Log(damage);
            yield return new WaitForSeconds(1);
        }
        poisonFX.Stop();
        poisonComplier = 0;
        isPoisened = false;
        StopCoroutine(GetPoisonDamage());
    }

}
