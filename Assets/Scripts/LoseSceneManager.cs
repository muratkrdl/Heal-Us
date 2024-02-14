using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseSceneManager : MonoBehaviour
{
    [SerializeField] Animator[] villagers;

    [SerializeField] Animator fadeImage;

    void Start() 
    {
        SetDieAllVillagers();
        Cursor.lockState = CursorLockMode.None;
    }

    void SetDieAllVillagers()
    {
        foreach (Animator villager in villagers)
        {
            if(Random.Range(0,2) == 0)
            {
                villager.SetTrigger("Die1");
            }
            else
            {
                villager.SetTrigger("Die2");
            }
        }
    }

    public void RestartButtonEvent()
    {
        StartCoroutine(RestartGame());
    }

    public void HomeButtonEvent()
    {
        StartCoroutine(HomeButton());
    }

    IEnumerator RestartGame()
    {
        SetFade();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Game");
    }

    IEnumerator HomeButton()
    {
        SetFade();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("MainMenu");
    }

    void SetFade()
    {
        fadeImage.SetTrigger("Fade");
    }

    void SetUnFade()
    {
        fadeImage.SetTrigger("UnFade");
    }

}
