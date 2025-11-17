using IceMilkTea.StateMachine;
using UnityEngine;

public partial class BearBoss : Enemy, IDamageable
{
    //Bossに関してはHurtステートがダメージを受けることができるステートという扱い
    //通常の敵はHurtステートがダメージを受けた

    [SerializeField] protected GameObject _fallingRocks;
    [SerializeField] protected GameObject _stone;
    
    private Animator _bearAnimator;
    private Hurtbox _hurtbox;
    
    private ImtStateMachine<BearBoss> stateMachine;

    public enum StateEvent
    {
        SlamEnter,
        WeaknessEnter,
        IdleEnter,
        Dead
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        _bearAnimator = GetComponent<Animator>();
        
        stateMachine = new ImtStateMachine<BearBoss>(this);
        stateMachine.AddTransition<BearBossIdle, BearBossSlam>((int)StateEvent.SlamEnter);
        
        stateMachine.AddTransition<BearBossSlam, BearBossWeakness>((int)StateEvent.WeaknessEnter);
        
        stateMachine.AddTransition<BearBossWeakness, BearBossIdle>((int)StateEvent.IdleEnter);
        
        stateMachine.AddTransition<BearBossWeakness, BearBossDead>((int)StateEvent.Dead);
        
        stateMachine.SetStartState<BearBossIdle>();
    }
    
    protected override void OnStart()
    {
        base.OnStart();
        stateMachine.Update();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        stateMachine.Update();
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
            Instantiate(_stone, child.transform.position,Quaternion.identity);
        }
    }
}