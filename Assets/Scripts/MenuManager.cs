using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject _pauseMenu;

    private bool _isPaused;

    private void Start()
    {
        _pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (PauseMenu.instance.MenuOpenCloseInput)
        {
            if (!_isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }

        }
    }

    #region Pause/Resume Functions
    public void Resume()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        _isPaused = false;
    }

    public void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0f;

        OpenMainMenu();
    }

    #endregion

    #region Canvas Activation Functions

    public void OpenMainMenu()
    {
        _pauseMenu.SetActive(true);
    }

    public void CloseMainMenu()
    {
        _pauseMenu.SetActive(false);
    }
    #endregion


    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _pauseMenu.SetActive(false);
    }
}
