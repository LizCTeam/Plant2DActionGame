using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using IceMilkTea.StateMachine;
using UnityEditor.Animations;

public class CageBehaviour : BasicBehaviour
{
    [SerializeField]
    public Player Player;
    [SerializeField]
    private Animator VegeAnimatior;
    public GameObject Position;
    public GameObject Projectile;
    
    
    private ImtStateMachine<CageBehaviour> stateMachine;
    
    #region 状態遷移(ステート)
    public enum StateEvent
    {
        Seeding,
        Watering,
        Cancel,
        MatureFinish
    }

    private class NothingState : ImtStateMachine<CageBehaviour>.State
    {
        // 状態へ突入時の処理はこのEnterで行う
        protected internal override void Enter()
        {
            Context.VegeAnimatior.Play("Nothing");
        }

        // 状態の更新はこのUpdateで行う
        protected internal override void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                stateMachine.SendEvent((int)StateEvent.Seeding);
            }
        }

        // 状態から脱出する時の処理はこのExitで行う
        protected internal override void Exit()
        {
        }
    }
    
    private class SeedState : ImtStateMachine<CageBehaviour>.State
    {
        protected internal override void Enter()
        {
            Context.VegeAnimatior.Play("Seeding");
        }
        
        protected internal override void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                stateMachine.SendEvent((int)StateEvent.Watering);
            }
        }
    }
    
    private class SproutState : ImtStateMachine<CageBehaviour>.State
    {
        protected internal override void Enter()
        {
            Context.VegeAnimatior.Play("Sprout");
        }
        protected internal override void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                stateMachine.SendEvent((int)StateEvent.Watering);
            }
        }
    }
    
    private class FloraState : ImtStateMachine<CageBehaviour>.State
    {
        protected internal override void Enter()
        {
            Context.VegeAnimatior.Play("Flora");
        }
        protected internal override void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                stateMachine.SendEvent((int)StateEvent.Watering);
            }
        }
    }
    
    private class MatureState : ImtStateMachine<CageBehaviour>.State
    {
        protected internal override void Enter()
        {
            Context.VegeAnimatior.Play("Mature");
        }
        protected internal override void Update()
        {
            if (Context.Player.isButtonPress)
            {
                Context.AbilityUse();
                stateMachine.SendEvent((int)StateEvent.MatureFinish);
            }
        }
    }
    
    #endregion

    protected override void OnAwake()
    {
        base.OnAwake();
        stateMachine = new ImtStateMachine<CageBehaviour>(this);
        stateMachine.AddTransition<NothingState, SeedState>((int)StateEvent.Seeding);
        stateMachine.AddTransition<SeedState, SproutState>((int)StateEvent.Watering);
        stateMachine.AddTransition<SproutState, FloraState>((int)StateEvent.Watering);
        stateMachine.AddTransition<FloraState, MatureState>((int)StateEvent.Watering);
        stateMachine.AddTransition<SeedState, NothingState>((int)StateEvent.Cancel);
        stateMachine.AddTransition<SproutState, NothingState>((int)StateEvent.Cancel);
        stateMachine.AddTransition<FloraState, NothingState>((int)StateEvent.Cancel); //Floraからのキャンセルいらんかも
        stateMachine.AddTransition<MatureState, NothingState>((int)StateEvent.MatureFinish);
        
        stateMachine.SetStartState<NothingState>();
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

    public void AbilityUse()
    {
        Debug.Log("Ability Use");
        Instantiate(Projectile, Position.transform.position, Quaternion.identity);
    }
    
}