using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseMenu : BasicBehaviour
{
    public GameObject pauseContainer;
    public Player player;
    
    private PlayerController _playerController;
    private PlayerInput _playerInput;
    private bool _lastPaused;

    protected override void OnAwake()
    {
        if (player != null)
        {
            _playerController = player.GetComponent<PlayerController>();
            _playerInput = player.GetComponent<PlayerInput>();
        }
    }
    protected override void OnStart()
    {
        base.OnStart();
    }


    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_playerController == null || _playerInput == null) return;

        bool paused = _playerController.isPaused;
        if (paused != _lastPaused)
        {
            if (paused) EnterPause();
            else ExitPause();

            _lastPaused = paused;
        }
    }

    public void OnResumeButtonClick()
    {
        if (_playerController != null && _playerController.isPaused)
        {
            _playerController.isPaused = false;
            ExitPause();
        }
    }
    
    public void OnRetryButtonClick()
    {
        _playerController.isPaused = false;
        var currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void EnterPause()
    {
        pauseContainer.SetActive(true);
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        _playerInput.SwitchCurrentActionMap("UI");
        
        var first = pauseContainer.GetComponentInChildren<Selectable>();
        if (first != null)
        {
            EventSystem.current.SetSelectedGameObject(first.gameObject);
        }
    }

    public void ExitPause()
    {
        pauseContainer.SetActive(false);
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _playerInput.SwitchCurrentActionMap("Player");
        EventSystem.current.SetSelectedGameObject(null);
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}