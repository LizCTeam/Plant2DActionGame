using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : Character
{
    [SerializeField] private float acceleration = 7f;
    [SerializeField] private float deacceleration = 6f;
    [SerializeField] private float velPower = 0.9f;
    [SerializeField] public float jumpForce = 10f;
    [SerializeField] public float maxSpeed = 10f;
    [SerializeField] public float frictionAmount = 0.8f;
    
    private float _coyoteTime = 0.2f; //崖から離れた時の猶予時間
    private float _coyoteTimeCounter;
    
    private float _jumpBufferTime = 0.2f;
    private float _jumpBufferTimeCounter;
    
    private PlayerController _playerController;
    
    public GameObject VisualRoot;

    protected override void OnAwake()
    {
        base.OnAwake();
        _playerController = GetComponent<PlayerController>();
    }
    
    protected override void OnStart()
    {
        base.OnStart();
    }
    
    protected override void OnUpdate()
    {
        var playerActJump = _playerController.playerAct.Jump;
        
        base.OnUpdate();
        UpdateSpriteDirection();
        UpdateCoyoteTime();
        
        print(_body.linearVelocity);
        
        if (playerActJump.WasPressedThisFrame())
        {
            _jumpBufferTimeCounter =  _jumpBufferTime;
        }
        else
        {
            _jumpBufferTimeCounter -= Time.deltaTime;
        }
        
        if (_coyoteTimeCounter > 0f && _jumpBufferTimeCounter > 0f)
        {
            _body.linearVelocityY = jumpForce;

            _jumpBufferTimeCounter = 0f;
        }

        if (playerActJump.WasReleasedThisFrame() && _body.linearVelocityY > 0f)
        {
            _body.linearVelocityY = _body.linearVelocityY * 0.5f;
            
            _coyoteTimeCounter = 0f;
        }

        if (isGrounded() && Mathf.Abs(_playerController.inputDirection.x) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(_body.linearVelocity.x), Mathf.Abs(frictionAmount));
        }
    }

    private void UpdateSpriteDirection()
    {
        var visualScale = VisualRoot.transform.localScale;
        
        if (_body.linearVelocity.x < -0.5f)
        {
            visualScale.x = -Mathf.Abs(visualScale.x);
            VisualRoot.transform.localScale = visualScale;
        }
        else if (_body.linearVelocity.x > 0.5f)
        {
            visualScale.x = Mathf.Abs(visualScale.x);
            VisualRoot.transform.localScale = visualScale;
        }
    }

    private void UpdateCoyoteTime()
    {
        if (isGrounded())
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Move();
    }
    
    public void Move()
    {
        //新挙動
        //accelRateはtargetSpeedがある一定値(0.01)より大きいならばaccelに切り替わる
        //それをmovementで入力した値によって変えている
        var targetSpeed = _playerController.inputDirection.x * maxSpeed;
        var speedDif = targetSpeed - _body.linearVelocity.x;
        var accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deacceleration;
        var movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        print("inputDirection : " + _playerController.inputDirection);
        _body.AddForce(movement * Vector2.right, ForceMode2D.Force);
        
        //これは形骸化した挙動
        //_body.linearVelocity = new Vector2(targetSpeed, _body.linearVelocityY);
    }

    public int GetHp()
    {
        return this._hp;
    }
}
