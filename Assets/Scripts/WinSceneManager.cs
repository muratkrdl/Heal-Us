using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneManager : MonoBehaviour
{
    [SerializeField] Animator[] villagers;

    [SerializeField] Animator fadeImage;

    void Awake() 
    {
        if(!fadeImage.gameObject.activeSelf)
            fadeImage.gameObject.SetActive(true);
        SetUnFade();    
    }

    void Start() 
    {
        SetWinAllVillagers();
        Cursor.lockState = CursorLockMode.None;
    }

    void SetWinAllVillagers()
    {
        foreach (Animator villager in villagers)
        {
            villager.SetTrigger("Win");
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
