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

    [Header("必須設定項目")] [SerializeField] protected GameObject _fallingRocks;
    [SerializeField] protected GameObject _stone;
    [SerializeField] protected GameObject VisualRoot;
    [SerializeField] protected GameObject _hipDropPos;
    
    [SerializeField, Header("デバッグ用")] private AttackPattern _attackPatternOverride;
    [SerializeField] private bool _useAttackPatternOverride = false;

    protected float _wallCheckDistance = 20f;
    private Vector2 _raycastOffset = new Vector2(0.0f, 0.0f);
    private Animator _bearAnimator;
    private Hurtbox _hurtbox;
    private Player _player;
    private Vector2 _targetDirection;

    private ImtStateMachine<BearBoss> stateMachine;

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

        _hurtbox.gameObject.SetActive(false);
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
        print(stateMachine.CurrentStateName);
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public void OnDamaged(int damage)
    {
        this._hp -= damage;
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
        var randomAmount = Random.Range(minStoneAmount, maxStoneAmount);
        for (int i = 0; i < randomAmount; i++)
        {
            var stone = Instantiate(_stone, _hipDropPos.transform.position, Quaternion.identity);
            var stonebody = stone.GetComponent<Rigidbody2D>();
            var randonAngle = Common.Rotate(Vector2.up, Mathf.Deg2Rad * Random.Range(-45, 45));
            
            stonebody.AddForce(randonAngle * 10f, ForceMode2D.Impulse);
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
        var playerDirction = (_player.transform.position - transform.position).normalized;
        VisualRoot.transform.localScale = new Vector3(-Math.Sign(playerDirction.x), 1f, 1f);
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

private bool IsWallAhead()
    {
        Vector2 origin = (Vector2)transform.position + _raycastOffset;
        Vector2 dir = new Vector2(_body.linearVelocityX, 0f);
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, _wallCheckDistance, GroundLayer);

        return hit.collider != null;
    }
}