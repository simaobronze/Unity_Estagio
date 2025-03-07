using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public bool MenuOpenCloseInput { get; private set; }

    private PlayerInput _playerInput;

    private InputAction _pauseAction;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        _pauseAction = _playerInput.actions["Pause"];
    }

    private void Update()
    {
        MenuOpenCloseInput = _pauseAction.WasPressedThisFrame();
    }


}