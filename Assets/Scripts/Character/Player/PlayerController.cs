using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : BasicBehaviour
{
    public PlayerAction _act;
    public PlayerAction.PlayerActions playerAct;
    [FormerlySerializedAs("_inputDirection")] public Vector2 inputDirection = Vector2.zero;

    public bool isButtonPress;
    [FormerlySerializedAs("Cage")] public ReworkCageBehaviour cage;

    [SerializeField] private Player _player;

    [Header("サウンド")]
    [SerializeField] private AudioSource soundJump;
    [SerializeField] private AudioSource soundSeedChange;
    
    public bool isPaused = false;
    
    protected override void OnAwake()
    {
        base.OnAwake();

        _act = new PlayerAction();
        playerAct = _act.Player;
    }

    protected override void OnStart()
    {
        base.OnStart();

        
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_player.IsClear) return;
        if (_player.IsDead) return;
        
        inputDirection = playerAct.Move.ReadValue<Vector2>();
        
        if (Math.Abs(inputDirection.x) >= 0.5f)
        {
            inputDirection.x = Math.Sign(inputDirection.x);
        }
        else
        {
            inputDirection.x *= 0.5f;
        }
        
        inputDirection.x = Mathf.Clamp(inputDirection.x, -1f, 1f);
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    private void OnEnable()
    {
        _act?.Enable();

        playerAct.Move.started += OnMove;
        playerAct.Move.performed += OnMove;
        playerAct.Move.canceled += OnMove;

        playerAct.Jump.started += OnJump;
        playerAct.Jump.performed += OnJump;
        playerAct.Jump.canceled += OnJump;

        playerAct.Fire.started += OnFire;
        playerAct.Fire.performed += OnFire;
        playerAct.Fire.canceled += OnFire;

        playerAct.UniqueAction.started += OnUniqueAction;
        playerAct.UniqueAction.performed += OnUniqueAction;
        playerAct.UniqueAction.canceled += OnUniqueAction;
        
        playerAct.LeftSwitching.started += OnLeftSwitching;
        playerAct.LeftSwitching.performed += OnLeftSwitching;
        playerAct.LeftSwitching.canceled += OnLeftSwitching;
        
        playerAct.RightSwitching.started += OnRightSwitching;
        playerAct.RightSwitching.performed += OnRightSwitching;
        playerAct.RightSwitching.canceled += OnRightSwitching;
        

        playerAct.ExitAction.started += OnExitAction;
        playerAct.ExitAction.performed += OnExitAction;
        playerAct.ExitAction.canceled += OnExitAction;
    }

    private void OnDisable()
    {
        playerAct.Move.started -= OnMove;
        playerAct.Move.performed -= OnMove;
        playerAct.Move.canceled -= OnMove;

        playerAct.Jump.started -= OnJump;
        playerAct.Jump.performed -= OnJump;
        playerAct.Jump.canceled -= OnJump;

        playerAct.Fire.started -= OnFire;
        playerAct.Fire.performed -= OnFire;
        playerAct.Fire.canceled -= OnFire;

        playerAct.UniqueAction.started -= OnUniqueAction;
        playerAct.UniqueAction.performed -= OnUniqueAction;
        playerAct.UniqueAction.canceled -= OnUniqueAction;

        playerAct.LeftSwitching.started -= OnLeftSwitching;
        playerAct.LeftSwitching.performed -= OnLeftSwitching;
        playerAct.LeftSwitching.canceled -= OnLeftSwitching;
        
        playerAct.RightSwitching.started -= OnRightSwitching;
        playerAct.RightSwitching.performed -= OnRightSwitching;
        playerAct.RightSwitching.canceled -= OnRightSwitching;

        playerAct.ExitAction.started -= OnExitAction;
        playerAct.ExitAction.performed -= OnExitAction;
        playerAct.ExitAction.canceled -= OnExitAction;

        _act?.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        
        /*
         * if inputDirection.x >= 0.5:
                inputDirection.x=1
           else
                inputDirection.x/=0.5 
         */
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_player.IsClear) return;
        if (!isPaused && !_player.IsDead && context.started)
        {
            soundJump.Play();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (_player.IsClear) return;
        if (!isPaused && !_player.IsDead && context.started)
        {
            cage.OnFire(context);
        }
    }

    public void OnUniqueAction(InputAction.CallbackContext context)
    {
        if (_player.IsClear) return;
        cage.OnUniqueAction(context);
    }

    public void OnLeftSwitching(InputAction.CallbackContext context)
    {
        if (_player.IsClear) return;
        if (!isPaused && !_player.IsDead && context.started)
        {
            cage.OnLeftSwitchAction(context);
            soundSeedChange.Play();
        }
    }
    
    public void OnRightSwitching(InputAction.CallbackContext context)
    {
        if (_player.IsClear) return;
        if (!isPaused && !_player.IsDead && context.started)
        {
            cage.OnRightSwitchAction(context);
            soundSeedChange.Play();
        }
    }

    public void OnExitAction(InputAction.CallbackContext context)
    {
        if (_player.IsClear) return;
        if (!_player.IsDead && context.started)
        {
            isPaused = !isPaused;
        }
    }
}