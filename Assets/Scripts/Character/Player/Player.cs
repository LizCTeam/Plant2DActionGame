using System;
using System.Collections;
using IceMilkTea.StateMachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    [SerializeField] private AudioSource daikonSound;
    
    private float _coyoteTime = 0.2f; //崖から離れた時の猶予時間
    private float _coyoteTimeCounter;
    public bool IsAttacking { private set; get; }

    public int AvailableWeaponHit = 0;
    
    private ImtStateMachine<Player> _stateMachine;
    
    private float _jumpBufferTime = 0.2f;
    private float _jumpBufferTimeCounter;
    
    [SerializeField, Header("落下地点")] private float _deadPointY = -10f;
    
    private Vector3 _startPosition;
    
    public GameObject VisualRoot;
    public Hurtbox Hurtbox;
    
    public ReworkCageBehaviour.GrowthStage CurrentStage = ReworkCageBehaviour.GrowthStage.Nothing;
    
    public UnityEvent OnGameFinishEvent;
    
    [HideInInspector]
    public PlayerInput Input;

    [HideInInspector]
    public bool IsClear
    {
        get
        {
            return GameResultSingleton.Instance?.IsGameClear ?? false;
        }
    }
    
    [HideInInspector]
    public int Hp
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

    private enum PlayerAnimationState
    {
        Idle = 0,
        Hurt = 1,
        DaikonAttack = 2,
        Walk = 3,
        Jump = 4,
        Dead = 5
    }

    public bool IsDead => Hp <= 0;
    private bool _isAnimationHurt = false;
    
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
    
    protected override void OnAwake()
    {
        base.OnAwake();
        Input = GetComponent<PlayerInput>();
        _stateMachine = new ImtStateMachine<Player>(this);
        Controller = GetComponent<PlayerController>();
        
        _stateMachine.AddTransition<PlayerNormal, PlayerDaikonAttack>((int)StateEvent.AttackStart);
        _stateMachine.AddTransition<PlayerDaikonAttack, PlayerNormal>((int)StateEvent.AttackFinish);
        
        _stateMachine.SetStartState<PlayerNormal>();
    }
    
    protected override void OnStart()
    {
        base.OnStart();
        Hp = _maxHp;
        _stateMachine.Update();
        
        BGMManager.Instance.PlayStageBGM();
        GameResultSingleton.Instance?.InitResult();
        GameResultSingleton.Instance?.StartTimer();
    }
    
    protected override void OnUpdate()
    {
        base.OnUpdate();
        _stateMachine.Update();
        
        if(IsClear) return;
        if(IsDead) return;
        if (Controller.isPaused) return;
        
        Vector3 _pos = transform.position;
        transform.position = _pos;
    }

    private void UpdateSpriteDirection()
    {
        if(IsClear) return;
        if(IsDead) return;
        if (Controller.isPaused) return;
        
        var visualScale = VisualRoot.transform.localScale;
        
        if (Controller.inputDirection.x < -0.8f)
        {
            visualScale.x = -Mathf.Abs(visualScale.x);
            VisualRoot.transform.localScale = visualScale;
        }
        else if (Controller.inputDirection.x > 0.8f)
        {
            visualScale.x = Mathf.Abs(visualScale.x);
            VisualRoot.transform.localScale = visualScale;
        }

    }

    private void UpdateCoyoteTime()
    {
        if(IsClear) return;
        if(IsDead) return;
        
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
        UpdateAnimation();
        if (transform.position.y < _deadPointY && !IsDead)
        {
            Hp = 0;
            OnGameFinishEvent.Invoke();
        }
    }
    
    public void Move()
    {
        if(IsClear) return;
        if(IsDead) return;
        if (Controller.isPaused) return;
        
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
        return this.Hp;
    }

    public void OnDamaged(int damage)
    {
        EffectManager.Instance.PlayEffect(transform.position);
        Hp -= damage;
        SoundManagerSingleton.Instance.PlaySound("PlayerHurt");
        if (Hp <= 0)
        {
            OnGameFinishEvent.Invoke();
        }
        else if(!_isAnimationHurt)
        {
            StartCoroutine(PlayHurtAnimation());
        }
        // if (this.Hp <= 0 || transform.position.y < _deadPointY) 
        // {
        //     //Deadアニメーションを再生
        //     //ゲームオーバー画面を表示
        //     //操作不可状態にする
        //
        //     IsDead = true;
        // }
    }

    private IEnumerator PlayHurtAnimation()
    {
        _isAnimationHurt = true;
        yield return new WaitUntil(() => _playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"));
        yield return new WaitForAnimation(_playerAnimator, 0, "Hurt");
        _isAnimationHurt = false;
    }

    public void OnFinish()
    {
        Input.SwitchCurrentActionMap("UI");
        GameResultSingleton.Instance?.StopTimer();
        Time.timeScale = 1;
        _body.linearVelocity = Vector2.zero;
        if (!IsDead)
        {
            var instance = GameResultSingleton.Instance;
            if (instance != null) instance.IsGameClear = true;
        }
    }
    
    private void PlayDaikonSound()
    {
        daikonSound.Play();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Goal"))
        {
            OnGameFinishEvent.Invoke();
        }
    }
    
    public void Dash()
    {
        if (CurrentStage != ReworkCageBehaviour.GrowthStage.Mature) return;
        var visualScale = VisualRoot.transform.localScale;
        var dash = new Vector2(visualScale.x * _dashForce, 0);
        _body.AddForce(dash, ForceMode2D.Impulse);
    }

    private void UpdateAnimation()
    {
        if (IsDead)
        {
            _playerAnimator.SetInteger(State, (int)PlayerAnimationState.Dead);
            return;
        }
        
        if (IsAttacking)
        {
            _playerAnimator.SetInteger(State, (int)PlayerAnimationState.DaikonAttack);
            return;
        }
        
        if (_isAnimationHurt)
        {
            _playerAnimator.SetInteger(State, (int)PlayerAnimationState.Hurt);
            return;
        }
        
        if (!isGrounded())
        {
            _playerAnimator.SetInteger(State, (int)PlayerAnimationState.Jump);
        }
        else if (Controller.inputDirection.x != 0)
        {
            _playerAnimator.SetInteger(State, (int)PlayerAnimationState.Walk);
        }
        else
        {
            _playerAnimator.SetInteger(State, (int)PlayerAnimationState.Idle);
        }
    }
}
