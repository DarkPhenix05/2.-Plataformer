using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    [Header ("GAME OVER")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header ("PAUSE")]
    public GameObject pauseScreen;
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject rightsScreen;
    [SerializeField] private GameObject mainScreen;

    

    private void Awake()
    {
        gameOverScreen.SetActive(false);
    }

    #region Game Over Functions
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        AudioManager.instance.PlaySound(gameOverSound);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);

    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    #endregion

    #region PAUSE Function
    public void pauseGame(bool status)
    {
        pauseScreen.SetActive(status);
        if (status == true)
        {
            Time.timeScale =0f;
        }
        if (status == false)
        {
            Time.timeScale = 1f;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            AudioManager.instance.PlaySound(interactSound);
            if (pauseScreen.activeInHierarchy)
            {
                pauseGame(false);
            }
            else
            {
                pauseGame(true);
            }
        }
    }

    public void ChangeVol()
    {
        AudioManager.instance.ChangeVolume(0.2f);
    }
    public void MusicVol()
    {
        AudioManager.instance.MusicVolume(0.2f);
    }

    #endregion

    #region MAIN MENU
    public void Play()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void Controls()
    {
        mainScreen.SetActive(false);
        rightsScreen.SetActive(false);
        controlsScreen.SetActive(true);
        AudioManager.instance.PlaySound(interactSound);
    }
    public void ToMain()
    {
        controlsScreen.SetActive(false);
        rightsScreen.SetActive(false);
        mainScreen.SetActive(true);
        AudioManager.instance.PlaySound(interactSound);
    }

    public void Creators()
    {
        controlsScreen.SetActive(false);
        mainScreen.SetActive(false);
        rightsScreen.SetActive(true);
        AudioManager.instance.PlaySound(interactSound);
    }
    #endregion
}