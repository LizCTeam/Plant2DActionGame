using NUnit.Framework.Internal.Filters;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : Character
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _jumpSpeed = 10f;

    private PlayerAction _act;
    public PlayerAction.PlayerActions _playerAct;
    private Vector2 moveInput = Vector2.zero;

    private float _dir = 0f;

    public bool isButtonPress;
    public GameObject VisualRoot;
    [FormerlySerializedAs("Cage")] public CageBehaviour cage;


    protected override void OnAwake()
    {
        base.OnAwake();

        _act = new PlayerAction();
        _playerAct = _act.Player;
    }

    protected override void OnStart()
    {
        base.OnStart();

        _body = GetComponent<Rigidbody2D>();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        var visualScale = VisualRoot.transform.localScale;

        if (_body.linearVelocity.x < 0f)
        {
            visualScale.x = -Mathf.Abs(visualScale.x);
            VisualRoot.transform.localScale = visualScale;
        }
        else if (_body.linearVelocity.x > 0f)
        {
            visualScale.x = Mathf.Abs(visualScale.x);
            VisualRoot.transform.localScale = visualScale;
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    private void OnEnable()
    {
        _act?.Enable();

        _playerAct.Move.started += OnMove;
        _playerAct.Move.performed += OnMove;
        _playerAct.Move.canceled += OnMove;

        _playerAct.Jump.started += OnJump;
        _playerAct.Jump.performed += OnJump;
        _playerAct.Jump.canceled += OnJump;

        _playerAct.Fire.started += OnFire;
        _playerAct.Fire.performed += OnFire;
        _playerAct.Fire.canceled += OnFire;

        _playerAct.UniqueAction.started += OnUniqueAction;
        _playerAct.UniqueAction.performed += OnUniqueAction;
        _playerAct.UniqueAction.canceled += OnUniqueAction;

        _playerAct.Switching.started += OnSwitching;
        _playerAct.Switching.performed += OnSwitching;
        _playerAct.Switching.canceled += OnSwitching;

        _playerAct.ExitAction.started += OnExitAction;
        _playerAct.ExitAction.performed += OnExitAction;
        _playerAct.ExitAction.canceled += OnExitAction;
    }

    private void OnDisable()
    {
        _playerAct.Move.started -= OnMove;
        _playerAct.Move.performed -= OnMove;
        _playerAct.Move.canceled -= OnMove;

        _playerAct.Jump.started -= OnJump;
        _playerAct.Jump.performed -= OnJump;
        _playerAct.Jump.canceled -= OnJump;

        _playerAct.Fire.started -= OnFire;
        _playerAct.Fire.performed -= OnFire;
        _playerAct.Fire.canceled -= OnFire;

        _playerAct.UniqueAction.started -= OnUniqueAction;
        _playerAct.UniqueAction.performed -= OnUniqueAction;
        _playerAct.UniqueAction.canceled -= OnUniqueAction;

        _playerAct.Switching.started -= OnSwitching;
        _playerAct.Switching.performed -= OnSwitching;
        _playerAct.Switching.canceled -= OnSwitching;

        _playerAct.ExitAction.started -= OnExitAction;
        _playerAct.ExitAction.performed -= OnExitAction;
        _playerAct.ExitAction.canceled -= OnExitAction;

        _act?.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _dir = context.ReadValue<Vector2>().x;
        Move();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded())
        {
            _body.linearVelocityY += _jumpSpeed;
        }
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

    public void Move()
    {
        _body.linearVelocity = new Vector2(_dir * _moveSpeed, _body.linearVelocity.y);
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