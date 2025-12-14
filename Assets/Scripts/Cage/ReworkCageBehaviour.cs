using System;
using System.Collections.Generic;
using UnityEngine;
using IceMilkTea.StateMachine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public partial class ReworkCageBehaviour : BasicBehaviour
{
    [SerializeField]
    public PlayerController Player;
    [SerializeField]
    private Animator _vegeAnimatior;
    [SerializeField]
    private GameObject _carrotAbilityObject;
    [SerializeField]
    private GameObject _cornAbilityObject;
    [SerializeField] 
    private List<PlantAttributeData> _plantAttributes;
    
    public Text TimerText;
    public Text SeedText;
    public float Timer = 0f;
    public bool IsGrowing = false;
    public VegetableType CurrentVegetableType = VegetableType.Carrot;

    public GrowthStage GrowthLevel
    {
        get
        {
            if (!IsGrowing) return GrowthStage.Nothing;
            PlantAttributeData plantData = _plantAttributeData[CurrentVegetableType];
            
            if (Timer < plantData.MaxGrowthDuration * plantData.SeedingPercentage)
            {
                return GrowthStage.Nothing;
            }
            if (Timer < plantData.MaxGrowthDuration * plantData.SproutPercentage)
            {
                return GrowthStage.Seeding;
            }
            if (Timer < plantData.MaxGrowthDuration)
            {
                return GrowthStage.Sprout;
            }
            return GrowthStage.Mature;
        }
    } 
    
    public event Action<InputAction.CallbackContext> UniqueAction;
    public event Action<InputAction.CallbackContext> SwitchAction;
    public event Action<InputAction.CallbackContext> Fire;
    
    private Dictionary<VegetableType, IHasAbility> _abilities = new Dictionary<VegetableType, IHasAbility>();
    private Dictionary<VegetableType, PlantAttributeData> _plantAttributeData = new Dictionary<VegetableType, PlantAttributeData>();
    
    public enum StateEvent
    {
        Seeding,
        Cancel
    }
    
    public enum GrowthStage
    {
        Nothing,
        Seeding,
        Sprout,
        Mature
    }
    
    private static readonly int Level = Animator.StringToHash("GrowthLevel");
    private ImtStateMachine<ReworkCageBehaviour> stateMachine;
    
    
    protected override void OnAwake()
    {
        base.OnAwake();
        stateMachine = new ImtStateMachine<ReworkCageBehaviour>(this);
        stateMachine.AddTransition<NothingState, PlantState>((int)StateEvent.Seeding);
        stateMachine.AddTransition<PlantState, NothingState>((int)StateEvent.Cancel);
        
        stateMachine.SetStartState<NothingState>();
        
        _abilities.Add(VegetableType.Carrot, _carrotAbilityObject.GetComponent<CarrotAbility>());
        _abilities.Add(VegetableType.Corn, _cornAbilityObject.GetComponent<CornAbility>());

        foreach (var data in _plantAttributes)
        {
            _plantAttributeData.Add(data.VegetableType, data);
        }
    }

    protected override void OnStart()
    {
        base.OnStart();
        stateMachine.Update();
        if (SeedText != null)
        {
            SeedText.text = "seed : " + CurrentVegetableType;
        }
    }
    
    protected override void OnUpdate()
    {
        base.OnUpdate();
        stateMachine.Update();
        if (TimerText != null)
        {
            TimerText.text = "Timer : " + Timer;
        }
    }
    
    public void SwitchSeed()
    {
        var maxTypeCount = _plantAttributeData.Count;
        VegetableType type = CurrentVegetableType + 1;
        while (!_abilities.ContainsKey(type) || _abilities[type] == null)
        {
            type = (VegetableType)(((int)type + 1) % maxTypeCount);
        }
        CurrentVegetableType = type;
    }
    
    public void AbilityUse()
    {
        _abilities[CurrentVegetableType]?.UseAbility();
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

    //
    // private int currentGrowthStage = 0;
    //
    // private ImtStateMachine<ReworkCageBehaviour> stateMachine;
    
    // // 野菜ごとの成長時間（秒）
    // 
    //
    
    //
    // private class SproutState : ImtStateMachine<ReworkCageBehaviour>.State
    // {
    //     protected internal override void Enter()
    //     {
    //         //育成中でも武器として使用可能にする。
    //         Context.Fire += Fire;
    //         Context.isGrowing = true;
    //         Context.VegeAnimatior.Play("Sprout");
    //     }
    //     protected internal override void Update()
    //     {
    //         if (Context.player.playerAct.UniqueAction.IsPressed())
    //         {
    //             Context.timer += Time.deltaTime;
    //             float requiredTime = Context.growthDurations[Context.currentVegetableType][1];
    //             if (Context.timer >= requiredTime)
    //             {
    //                 Context.currentGrowthStage = 2;
    //                 stateMachine.SendEvent((int)StateEvent.Watering);
    //             }
    //         }
    //     }
    //     
    //     private void UniqueAction(InputAction.CallbackContext context)
    //     {
    //         //Context.AbilityUse();
    //     }
    //
    //     private void Fire(InputAction.CallbackContext context)
    //     {
    //         Context.AbilityUse();
    //         Context.isGrowing = false;
    //         Context.timer = 0f;
    //         Context.currentGrowthStage = 0;
    //         stateMachine.SendEvent((int)StateEvent.Cancel);
    //     }
    //
    //     protected internal override void Exit()
    //     {
    //         Context.UniqueAction -= UniqueAction;
    //         Context.Fire -= Fire;
    //     }
    //
    //     
    // }
    //
    // private class FloraState : ImtStateMachine<ReworkCageBehaviour>.State
    // {
    //     protected internal override void Enter()
    //     {
    //         Context.Fire += Fire;
    //         Context.isGrowing = true;
    //         Context.VegeAnimatior.Play("Flora");
    //         Context.UniqueAction += UniqueAction;
    //     }
    //     protected internal override void Update()
    //     {
    //         if (Context.player.playerAct.UniqueAction.IsPressed())
    //         {
    //             Context.timer += Time.deltaTime;
    //             float requiredTime = Context.growthDurations[Context.currentVegetableType][2];
    //             if (Context.timer >= requiredTime)
    //             {
    //                 Context.currentGrowthStage = 3;
    //                 stateMachine.SendEvent((int)StateEvent.Watering);
    //             }
    //         }
    //     }
    //     
    //     private void UniqueAction(InputAction.CallbackContext context)
    //     {
    //         //Context.AbilityUse();
    //     }
    //
    //     private void Fire(InputAction.CallbackContext context)
    //     {
    //         Context.AbilityUse();
    //         Context.isGrowing = false;
    //         Context.timer = 0f;
    //         Context.currentGrowthStage = 0;
    //         stateMachine.SendEvent((int)StateEvent.Cancel);
    //     }
    //
    //     protected internal override void Exit()
    //     {
    //         Context.UniqueAction -= UniqueAction;
    //         Context.Fire -= Fire;
    //     }
    //
    //     
    // }
    //
    // private class MatureState : ImtStateMachine<ReworkCageBehaviour>.State
    // {
    //     protected internal override void Enter()
    //     {
    //         Context.Fire += Fire;
    //         Context.UniqueAction += UniqueAction;
    //         Context.isGrowing = false;
    //         Context.timer = 0f;
    //         Context.VegeAnimatior.Play("Mature");
    //         Context.currentGrowthStage = 0;
    //     }
    //     protected internal override void Update()
    //     {
    //         
    //     }
    //     
    //     private void UniqueAction(InputAction.CallbackContext context)
    //     {
    //         //Context.AbilityUse();
    //         //stateMachine.SendEvent((int)StateEvent.MatureFinish);
    //     }
    //
    //     private void Fire(InputAction.CallbackContext context)
    //     {
    //         Context.AbilityUse();
    //         stateMachine.SendEvent((int)StateEvent.MatureFinish);
    //     }
    //
    //     protected internal override void Exit()
    //     {
    //         Context.UniqueAction -= UniqueAction;
    //         Context.Fire -= Fire;
    //     }
    // } 
    //
    // #endregion
    //
    // protected override void OnAwake()
    // {
    //     base.OnAwake();
    //     stateMachine = new ImtStateMachine<ReworkCageBehaviour>(this);
    //     stateMachine.AddTransition<NothingState, SeedState>((int)StateEvent.Seeding);
    //     stateMachine.AddTransition<SeedState, SproutState>((int)StateEvent.Watering);
    //     stateMachine.AddTransition<SproutState, FloraState>((int)StateEvent.Watering);
    //     stateMachine.AddTransition<FloraState, MatureState>((int)StateEvent.Watering);
    //     stateMachine.AddTransition<SeedState, NothingState>((int)StateEvent.Cancel);
    //     stateMachine.AddTransition<SproutState, NothingState>((int)StateEvent.Cancel);
    //     stateMachine.AddTransition<FloraState, NothingState>((int)StateEvent.Cancel); //Floraからのキャンセルいらんかも
    //     stateMachine.AddTransition<MatureState, NothingState>((int)StateEvent.MatureFinish);
    //     
    //     stateMachine.SetStartState<NothingState>();
    //     
    //     abilities.Add(VegetableType.Carrot, CarrotAbilityObject.GetComponent<CarrotAbility>());
    //     abilities.Add(VegetableType.Corn, CornAbilityObject.GetComponent<CornAbility>());
    //
    //     //野菜ごとの各成長段階(Seed → Sprout → Flora → Mature)の時間
    //     growthDurations.Add(VegetableType.Carrot, new float[] {2f, 4f, 7f});
    //     growthDurations.Add(VegetableType.Corn, new float[] { 1f, 2f, 5f });
    //     // 他の野菜も同様に追加
    // }
    //
    //
    // protected override void OnFixedUpdate()
    // {
    //     base.OnFixedUpdate();
    // }
    //
    // public void AbilityUse()
    // {
    //     abilities[currentVegetableType]?.UseAbility();
    // }
    //

    //
    // public virtual void OnSwitchAction(InputAction.CallbackContext obj)
    // {
    //     SwitchAction?.Invoke(obj);
    // }
    //
    // public virtual void OnUniqueAction(InputAction.CallbackContext obj)
    // {
    //     UniqueAction?.Invoke(obj);
    // }
    //
    // public virtual void OnFire(InputAction.CallbackContext obj)
    // {
    //     Fire?.Invoke(obj);
    // }
}