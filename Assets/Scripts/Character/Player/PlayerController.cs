using System;
using NUnit.Framework.Internal.Filters;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : BasicBehaviour
{
    private PlayerAction _act;
    public PlayerAction.PlayerActions playerAct;
    [FormerlySerializedAs("_inputDirection")] public Vector2 inputDirection = Vector2.zero;

    public bool isButtonPress;
    [FormerlySerializedAs("Cage")] public ReworkCageBehaviour cage;

    [SerializeField] private Player _player;

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

        playerAct.Switching.started += OnSwitching;
        playerAct.Switching.performed += OnSwitching;
        playerAct.Switching.canceled += OnSwitching;

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

        playerAct.Switching.started -= OnSwitching;
        playerAct.Switching.performed -= OnSwitching;
        playerAct.Switching.canceled -= OnSwitching;

        playerAct.ExitAction.started -= OnExitAction;
        playerAct.ExitAction.performed -= OnExitAction;
        playerAct.ExitAction.canceled -= OnExitAction;

        _act?.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
        
        if (Math.Abs(inputDirection.x) >= 0.5f)
        {
            inputDirection.x = Math.Sign(inputDirection.x);
        }
        else
        {
            inputDirection.x *= 0.5f;
        }
        
        inputDirection.x = Mathf.Clamp(inputDirection.x, -1f, 1f);
        /*
         * if inputDirection.x >= 0.5:
                inputDirection.x=1
           else
                inputDirection.x/=0.5 
         */
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            cage.OnFire(context);
        }
    }

    public void OnUniqueAction(InputAction.CallbackContext context)
    {
        cage.OnUniqueAction(context);
    }

    public void OnSwitching(InputAction.CallbackContext context)
    {
        cage.OnSwitchAction(context);
    }

    public void OnExitAction(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

        }
    }
}