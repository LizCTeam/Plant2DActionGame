using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BasicBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _jumpSpeed = 10f;

    private PlayerAction _act;
    private PlayerAction.PlayerActions _playerAct;

    private float _dir = 0f;

    private Rigidbody2D _rb = null;


    protected override void OnAwake()
    {
        base.OnAwake();

        _act = new PlayerAction();
        _playerAct = _act.Player;
    }

    protected override void OnStart()
    {
        base.OnStart();

        _rb = GetComponent<Rigidbody2D>();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        Move();
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

        _act?.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _dir = context.ReadValue<Vector2>().x;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _rb.linearVelocityY += _jumpSpeed;
        }
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        // 攻撃ボタン押したときの処理を記述
        Debug.Log("OnFire");
    }

    private void OnUniqueAction(InputAction.CallbackContext context)
    {
        // 水やり、種まきボタンを押したときの処理を記述
        Debug.Log("OnUniqueAction");
    }

    private void OnSwitching(InputAction.CallbackContext context)
    {
        // 種の切り替えボタンを押したときの処理を記述
        Debug.Log("OnSwitching");
    }

    private void Move()
    {
        _rb.linearVelocity = new Vector2(_dir * _moveSpeed,_rb.linearVelocity.y);
    }
}