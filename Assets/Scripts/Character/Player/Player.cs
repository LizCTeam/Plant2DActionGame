using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : Character, IDamageable
{
    [SerializeField] protected int _maxHp = 3;

    protected int _hp
    {
        set
        {
            _currentHp = Mathf.Clamp(value, 0, _maxHp);
        }
        get
        {
            return _currentHp;
        }
    }
    
    protected int _currentHp;
    
    [SerializeField] private float acceleration = 7f;
    [SerializeField] private float deacceleration = 6f;
    [SerializeField] private float velPower = 0.9f;
    [SerializeField] public float jumpForce = 10f;
    [SerializeField] public float maxSpeed = 10f;
    [SerializeField] public float frictionAmount = 0.8f;

    [SerializeField, Header("落下地点")] private float _deadPointY = -10f;
    
    private float _coyoteTime = 0.2f; //崖から離れた時の猶予時間
    private float _coyoteTimeCounter;
    
    private float _jumpBufferTime = 0.2f;
    private float _jumpBufferTimeCounter;

    private bool _isDead = false;

    private PlayerController _playerController;

    private Vector3 _startPosition;
    
    public GameObject VisualRoot;

    protected override void OnAwake()
    {
        base.OnAwake();
        _playerController = GetComponent<PlayerController>();
    }
    
    protected override void OnStart()
    {
        base.OnStart();
        _hp = _maxHp;
    }
    
    protected override void OnUpdate()
    {
        var playerActJump = _playerController.playerAct.Jump;
        
        base.OnUpdate();
        UpdateSpriteDirection();
        UpdateCoyoteTime();

        Vector3 _pos = transform.position;
        transform.position = _pos;

        if (_isDead == true)
        {
            transform.position = _startPosition;
            _isDead = false;
            ResetStage();
        }
        else
        {
            if (playerActJump.WasPressedThisFrame())
            {
                _jumpBufferTimeCounter = _jumpBufferTime;
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

            if (this._hp <= 0 || transform.position.y < _deadPointY) 
            {
                //Deadアニメーションを再生
                //ゲームオーバー画面を表示
                //操作不可状態にする

                Debug.Log("Player Dead");
                Debug.Log(_hp);
                _isDead = true;
            }
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
        _body.AddForce(movement * Vector2.right, ForceMode2D.Force);
        
        //これは形骸化した挙動
        //_body.linearVelocity = new Vector2(targetSpeed, _body.linearVelocityY);
    }

    public int GetHp()
    {
        return this._hp;
    }

    public void OnDamaged(int damage)
    {
        _hp -= damage;
    }

    void ResetStage()
    {
        ContinuationChange.CurrentSceneName();

        SceneManager.LoadScene("GameOver");
    }
}
