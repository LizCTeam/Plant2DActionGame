using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using IceMilkTea.StateMachine;
using UnityEngine.Animations;
//using UnityEditor.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CageBehaviour : BasicBehaviour
{
    [FormerlySerializedAs("Player")] [SerializeField]
    public PlayerController player;
    [SerializeField]
    private Animator VegeAnimatior;
    
    [SerializeField]
    private GameObject CarrotAbilityObject;
    [SerializeField]
    private GameObject CornAbilityObject;
    
    public Text TimerText;
    public Text SeedText;
    
    public float timer = 0f;
    public bool isGrowing = false;
    
    public VegetableType currentVegetableType = VegetableType.Carrot;
    
    private ImtStateMachine<CageBehaviour> stateMachine;
    private Dictionary<VegetableType, IHasAbility> abilities = new Dictionary<VegetableType, IHasAbility>();
    
    public event Action<InputAction.CallbackContext> UniqueAction;
    public event Action<InputAction.CallbackContext> SwitchAction;
    public event Action<InputAction.CallbackContext> Fire;
    
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
            Context.UniqueAction += UniqueAction;
            Context.SwitchAction += SwitchAction;
            Context.isGrowing = false;
            Context.timer = 0f;
            Context.VegeAnimatior.Play("Nothing");
        }

        private void UniqueAction(InputAction.CallbackContext context)
        {
            stateMachine.SendEvent((int)StateEvent.Seeding);
        }

        private void SwitchAction(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                Context.SwitchSeed();
                Context.SeedText.text = "seed : " + Context.currentVegetableType;
            }
        }

        // 状態から脱出する時の処理はこのExitで行う
        protected internal override void Exit()
        {
            Context.UniqueAction -= UniqueAction;
            Context.SwitchAction -= SwitchAction;
        }
    }
    
    private class SeedState : ImtStateMachine<CageBehaviour>.State
    {
        protected internal override void Enter()
        {
            Context.UniqueAction += UniqueAction;
            Context.isGrowing = true;
            Context.VegeAnimatior.Play("Seeding");
        }
        
        protected internal override void Update()
        {
            if (Context.player._playerAct.UniqueAction.IsPressed())
            {
                Context.timer += Time.deltaTime;
                if (Context.timer >= 2f)
                {
                    stateMachine.SendEvent((int)StateEvent.Watering);
                }
            }
        }
        
        private void UniqueAction(InputAction.CallbackContext context)
        {
            
        }
        
        protected internal override void Exit()
        {
            Context.UniqueAction -= UniqueAction;
        }
    }
    
    private class SproutState : ImtStateMachine<CageBehaviour>.State
    {
        protected internal override void Enter()
        {
            Context.isGrowing = true;
            Context.VegeAnimatior.Play("Sprout");
        }
        protected internal override void Update()
        {
            if (Context.player._playerAct.UniqueAction.IsPressed())
            {
                Context.timer += Time.deltaTime;
                if (Context.timer >= 4f)
                {
                    stateMachine.SendEvent((int)StateEvent.Watering);
                }
            }
        }
        
        private void UniqueAction(InputAction.CallbackContext context)
        {
            
        }
        
        protected internal override void Exit()
        {
            Context.UniqueAction -= UniqueAction;
        }
    }
    
    private class FloraState : ImtStateMachine<CageBehaviour>.State
    {
        protected internal override void Enter()
        {
            Context.isGrowing = true;
            Context.VegeAnimatior.Play("Flora");
        }
        protected internal override void Update()
        {
            if (Context.player._playerAct.UniqueAction.IsPressed())
            {
                Context.timer += Time.deltaTime;
                if (Context.timer >= 7f)
                {
                    stateMachine.SendEvent((int)StateEvent.Watering);
                }
            }
        }
        
        private void UniqueAction(InputAction.CallbackContext context)
        {
            
        }
        
        protected internal override void Exit()
        {
            Context.UniqueAction -= UniqueAction;
        }
    }
    
    private class MatureState : ImtStateMachine<CageBehaviour>.State
    {
        protected internal override void Enter()
        {
            Context.Fire += Fire;
            Context.UniqueAction += UniqueAction;
            Context.isGrowing = false;
            Context.timer = 0f;
            Context.VegeAnimatior.Play("Mature");
        }
        protected internal override void Update()
        {
            
        }
        
        private void UniqueAction(InputAction.CallbackContext context)
        {
            //Context.AbilityUse();
            //stateMachine.SendEvent((int)StateEvent.MatureFinish);
        }

        private void Fire(InputAction.CallbackContext context)
        {
            Context.AbilityUse();
            stateMachine.SendEvent((int)StateEvent.MatureFinish);
        }

        protected internal override void Exit()
        {
            Context.UniqueAction -= UniqueAction;
            Context.Fire -= Fire;
        }
    } 
    
    //人の睡眠時間削っておいて許されると思うなよ (11/17 月)
    
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
        
        abilities.Add(VegetableType.Carrot, CarrotAbilityObject.GetComponent<CarrotAbility>());
        abilities.Add(VegetableType.Corn, CornAbilityObject.GetComponent<CornAbility>());
    }

    protected override void OnStart()
    {
        base.OnStart();
        stateMachine.Update();
        if (SeedText != null)
        {
            SeedText.text = "seed : " + currentVegetableType;
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        stateMachine.Update();
        if (TimerText != null)
        {
            TimerText.text = "Timer : " + timer;
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public void AbilityUse()
    {
        abilities[currentVegetableType]?.UseAbility();
    }

    public void SwitchSeed()
    {
        var maxTypeCount = Enum.GetNames(typeof(VegetableType)).Length;
        VegetableType type = currentVegetableType + 1;
        while (!abilities.ContainsKey(type) || abilities[type] == null)
        {
            type = (VegetableType)(((int)type + 1) % maxTypeCount);
        }
        currentVegetableType = type;
    }

    public virtual void OnSwitchAction(InputAction.CallbackContext obj)
    {
        SwitchAction?.Invoke(obj);
    }

    public virtual void OnUniqueAction(InputAction.CallbackContext obj)
    {
        UniqueAction?.Invoke(obj);
    }

    public virtual void OnFire(InputAction.CallbackContext obj)
    {
        Fire?.Invoke(obj);
    }
}