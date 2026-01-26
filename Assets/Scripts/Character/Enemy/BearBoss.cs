using System;
using System.Collections;
using System.Collections.Generic;
using IceMilkTea.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public partial class BearBoss : Enemy, IDamageable
{
    //Bossに関してはHurtステートがダメージを受けることができるステートという扱い
    //通常の敵はHurtステートがダメージを受けた

    [Header("速度")] [SerializeField] private float _velPower = 0.9f;
    [SerializeField] protected float _accelPower = 3f;
    [SerializeField] protected float _deaccelPower = 0.1f;
    [SerializeField] protected float _maxSpeed = 10f;

    [Header("必須設定項目")] [SerializeField] public GameObject _fallingRocks;
    [SerializeField] protected GameObject _stone;
    [SerializeField] protected GameObject VisualRoot;
    [SerializeField] protected GameObject _headPos;
    
    [SerializeField, Header("デバッグ用")] private AttackPattern _attackPatternOverride;
    [SerializeField] private bool _useAttackPatternOverride = false;

    [HideInInspector] public bool IsJumpReady;

    private const float WallCheckDistance = 4f;
    private Vector2 _raycastOffset = new Vector2(0.0f, 0.0f);
    private Animator _bearAnimator;
    private Hurtbox _hurtbox;
    private Player _player;
    private Vector2 _targetDirection;

    private ImtStateMachine<BearBoss> stateMachine;

    public bool IsDead = false;
    
    public enum StateEvent
    {
        BodyBlowEnter,
        HipDropEnter,
        SlamEnter,
        WeaknessEnter,
        IdleEnter,
        Dead
    }

    private enum AttackPattern
    {
        BodyBlowToSlam,
        SlamToHipDrop,
        HipDropToBodyBlow
    }

    private enum ActionType
    {
        BodyBlow,
        Slam,
        HipDrop,
        Weakness,
    }

    private enum BearAnimationState
    {
        Idle = 0,
        Jump = 1,
        Slam = 2,
        BodyBlow = 3,
        Weakness = 4,
        HipDrop = 5
    }
    
    private Queue<ActionType> _actionQueue = new Queue<ActionType>();

    protected override void OnAwake()
    {
        base.OnAwake();
        _bearAnimator = GetComponent<Animator>();
        _hurtbox = GetComponentInChildren<Hurtbox>();

        stateMachine = new ImtStateMachine<BearBoss>(this);
        stateMachine.AddTransition<BearBossIdle, BearBossBodyblow>((int)StateEvent.BodyBlowEnter);
        stateMachine.AddTransition<BearBossIdle, BearBossSlam>((int)StateEvent.SlamEnter);
        stateMachine.AddTransition<BearBossIdle, BearBossHipDrop>((int)StateEvent.HipDropEnter);
        stateMachine.AddTransition<BearBossBodyblow, BearBossSlam>((int)StateEvent.SlamEnter);
        stateMachine.AddTransition<BearBossBodyblow, BearBossWeakness>((int)StateEvent.WeaknessEnter);
        stateMachine.AddTransition<BearBossHipDrop, BearBossWeakness>((int)StateEvent.WeaknessEnter);
        stateMachine.AddTransition<BearBossSlam, BearBossWeakness>((int)StateEvent.WeaknessEnter);
        stateMachine.AddTransition<BearBossHipDrop, BearBossBodyblow>((int)StateEvent.BodyBlowEnter);
        stateMachine.AddTransition<BearBossSlam, BearBossHipDrop>((int)StateEvent.HipDropEnter);
        stateMachine.AddTransition<BearBossSlam, BearBossIdle>((int)StateEvent.IdleEnter);
        stateMachine.AddTransition<BearBossWeakness, BearBossIdle>((int)StateEvent.IdleEnter);
        stateMachine.AddTransition<BearBossWeakness, BearBossDead>((int)StateEvent.Dead);

        stateMachine.SetStartState<BearBossIdle>();
    }

    protected override void OnStart()
    {
        base.OnStart();
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        stateMachine.Update();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        stateMachine.Update();

        if (_hp <= 0)
        {
            BGMManager.Instance.PlayStageBGM();
            GameManager.Instance.IsBossDead = true;
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Move();
    }

    public void OnDamaged(int damage)
    {
        SoundManagerSingleton.Instance.PlaySound("Hurt");
        EffectManager.Instance.PlayEffect(transform.position);
        _hp -= damage;
    }

    public void SlamStone()
    {
        for (int i = 0; i < _fallingRocks.transform.childCount; i++)
        {
            var child = _fallingRocks.transform.GetChild(i);
            Instantiate(_stone, child.transform.position, Quaternion.identity);
        }
    }

    public void HipDropStone()
    {
        var randomAmount = Random.Range(minStoneAmount, maxStoneAmount + 1);
        for (int i = 0; i < randomAmount; i++)
        {
            var initPosition = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);
            var stone = Instantiate(_stone, initPosition, Quaternion.identity);
            var stoneBody = stone.GetComponent<Rigidbody2D>();
            var randomAngle = Common.Rotate(Vector2.up, Mathf.Deg2Rad * Random.Range(-45f, 45f));
            
            stoneBody?.AddForce(randomAngle * 20f, ForceMode2D.Impulse);
        }
    }

    private void Dash(float speed, Vector2 direction)
    {
        _body.linearVelocityX = speed * direction.normalized.x;
    }

    private void Move()
    {
        var targetSpeed = _targetDirection.x * _maxSpeed;
        var speedDif = targetSpeed - _body.linearVelocity.x;
        var accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _accelPower : _deaccelPower;
        var movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _velPower) * Mathf.Sign(speedDif);
        _body.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void FaceTarget()
    {
        var playerDirection = (_player.transform.position - transform.position).normalized;
        VisualRoot.transform.localScale = new Vector3(-Math.Sign(playerDirection.x), 1f, 1f);
    }
    
    private void SpriteFlip()
    {
        if (_body.linearVelocityX < -0.1f)
        {
            VisualRoot.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (_body.linearVelocityX > 0.1f)
        {
            VisualRoot.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private StateEvent EvaluateEvent(ActionType to)
    {
        switch (to)
        {
            case ActionType.BodyBlow:
                return StateEvent.BodyBlowEnter;
            case ActionType.Slam:
                return StateEvent.SlamEnter;
            case ActionType.HipDrop:
                return StateEvent.HipDropEnter;
            case ActionType.Weakness:
                return StateEvent.WeaknessEnter;
            default:
                throw new ArgumentOutOfRangeException(nameof(to), to, null);
        }
    }

    private bool IsWallAhead(Vector2 dir, float length)
    {
        var origin = (Vector2)transform.position;
        var hit = Physics2D.Raycast(origin, dir.normalized, length, GroundLayer);

        return hit.collider;
    }
}