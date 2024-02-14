using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] TextMeshPro text;

    [SerializeField] float speed;
    [SerializeField] float lerpTimeColor;
    [SerializeField] float lerpTimeAlpha;

    [SerializeField] float lifeTime;

    Vector3 randomVector3;

    void Awake() 
    {
        randomVector3 = Random.insideUnitSphere;
        randomVector3.y -= .5f;
    }

    void Start() 
    {
        transform.position += randomVector3;
        StartCoroutine(LerpAlpha());
        Destroy(gameObject,lifeTime);
    }

    void Update()
    {
        transform.LookAt(GameManager.Instance.GetPlayer.position);
        transform.Translate(speed * Time.deltaTime * Vector3.up);
        text.color = Color.Lerp(text.color, Color.white, lerpTimeColor * Time.deltaTime);
    }

    public void SetTextValues(string text, Color color)
    {
        this.text.text = text;
        this.text.color = color;
    }

    IEnumerator LerpAlpha()
    {
        float elapsed = 0;
        float duration = 1;
        float t = 0;
        while(elapsed <= 3)
        {
            yield return null;
            elapsed += Time.deltaTime;
            t = elapsed / duration;
            text.alpha = Mathf.Lerp(text.alpha, 0, t * Time.deltaTime);
        }
    }

}
