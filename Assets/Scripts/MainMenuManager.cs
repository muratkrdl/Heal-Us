using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Animator fadeImage;

    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject howToPlayPanel;

    [SerializeField] float waitTimeBetweenLoad;

    void Awake() 
    {
        Cursor.lockState = CursorLockMode.None;
        if(!mainMenuPanel.activeSelf)
            mainMenuPanel.SetActive(true);
        if(howToPlayPanel.activeSelf)
            howToPlayPanel.SetActive(false);
        if(!fadeImage.gameObject.activeSelf)
            fadeImage.gameObject.SetActive(true);
        SetUnFade();
    }

#region ButtonEvents
    public void PlayButtonEvent()
    {
        SetFade();
        StartCoroutine(StartGame());
    }
    public void HowToPlayButtonEvent()
    {
        mainMenuPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }
    public void BackMainMenuButtonEvent()
    {
        mainMenuPanel.SetActive(true);
        howToPlayPanel.SetActive(false);
    }
    public void QuitButtonEvent()
    {
        SetFade();
        StartCoroutine(QuitGame());
    }
#endregion

#region FadeImageFunc
    void SetFade()
    {
        fadeImage.SetTrigger("Fade");
    }
    void SetUnFade()
    {
        fadeImage.SetTrigger("UnFade");
    }
#endregion

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(waitTimeBetweenLoad);
        SceneManager.LoadScene("Game");
    }

    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(waitTimeBetweenLoad);
        Application.Quit();
    }

}
