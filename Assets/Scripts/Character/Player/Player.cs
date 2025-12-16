using IceMilkTea.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public partial class Player : Character, IDamageable
{
    [SerializeField] protected int _maxHp = 3;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private float acceleration = 7f;
    [SerializeField] private float deacceleration = 6f;
    [SerializeField] private float velPower = 0.9f;
    [SerializeField] public float jumpForce = 10f;
    [SerializeField] public float maxSpeed = 10f;
    [SerializeField] public float frictionAmount = 0.8f;
    [SerializeField] public ReworkCageBehaviour reworkCage;
    
    private float _coyoteTime = 0.2f; //崖から離れた時の猶予時間
    private float _coyoteTimeCounter;
    public bool IsAttacking { private set; get; }

    public int AvailableWeaponHit = 0;
    
    private ImtStateMachine<Player> _stateMachine;

    private float _jumpBufferTime = 0.2f;
    private float _jumpBufferTimeCounter;

    private bool _isNowJump = false;
    private bool _isInJumpArea = false;

    public bool IsNowJump => _isNowJump;
    public bool IsInJumpArea => _isInJumpArea;

    

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

    [HideInInspector]
    public PlayerController Controller{ private set; get; }
    
    private enum StateEvent
    {
        AttackStart,
        AttackFinish
    }

    private enum StatePlayer
    {
        Normal,
        DaikonAttack
    }
    
    public GameObject VisualRoot;
    
    protected override void OnAwake()
    {
        base.OnAwake();
        _stateMachine = new ImtStateMachine<Player>(this);
        Controller = GetComponent<PlayerController>();
        
        _stateMachine.AddTransition<PlayerNormal, PlayerDaikonAttack>((int)StateEvent.AttackStart);
        _stateMachine.AddTransition<PlayerDaikonAttack, PlayerNormal>((int)StateEvent.AttackFinish);
        
        _stateMachine.SetStartState<PlayerNormal>();
    }
    
    protected override void OnStart()
    {
        base.OnStart();
        _hp = _maxHp;
        _stateMachine.Update();
    }
    
    protected override void OnUpdate()
    {
        base.OnUpdate();
        _stateMachine.Update();
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
    }
    
    public void Move()
    {
        //新挙動
        //accelRateはtargetSpeedがある一定値(0.01)より大きいならばaccelに切り替わる
        //それをmovementで入力した値によって変えている
        var targetSpeed = Controller.inputDirection.x * maxSpeed;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("JumpArea"))
        {
            _isInJumpArea = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("JumpArea"))
        {
            _isInJumpArea = false;
        }
    }
}
