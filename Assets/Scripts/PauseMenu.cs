using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField]
    GameObject _pauseMenu;

    private void Start()
    {
        _pauseMenu.SetActive(false);
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
    }

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