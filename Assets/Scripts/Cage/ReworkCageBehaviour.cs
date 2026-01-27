using System;
using System.Collections.Generic;
using Ability;
using UnityEngine;
using IceMilkTea.StateMachine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public partial class ReworkCageBehaviour : BasicBehaviour
{
    [SerializeField]
    public Player Player;
    [SerializeField]
    private Animator _vegeAnimatior;
    [SerializeField]
    private GameObject _carrotAbilityObject;
    [SerializeField]
    private GameObject _cornAbilityObject;
    [SerializeField]
    private GameObject _daikonAbilityObject;
    [SerializeField] 
    public List<PlantAttributeData> _plantAttributes;
    
    public float Timer = 0f;
    public bool IsGrowing = false;
    public VegetableType CurrentVegetableType = VegetableType.Carrot;

    public GrowthStage GrowthLevel
    {
        get
        {
            if (!IsGrowing) return GrowthStage.Nothing;
            PlantAttributeData plantData = PlantAttributeData[CurrentVegetableType];
            
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
    public event Action<InputAction.CallbackContext> LeftSwitchAction;
    public event Action<InputAction.CallbackContext> RightSwitchAction;
    public event Action<InputAction.CallbackContext> Fire;
    
    private Dictionary<VegetableType, IHasAbility> _abilities = new Dictionary<VegetableType, IHasAbility>();
    public Dictionary<VegetableType, PlantAttributeData> PlantAttributeData = new Dictionary<VegetableType, PlantAttributeData>();
    
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
    
    private static readonly int DoReady = Animator.StringToHash("DoReady");
    private static readonly int Level = Animator.StringToHash("GrowthLevel");
    private ImtStateMachine<ReworkCageBehaviour> stateMachine;
    private GrowthStage _prevGrowthStage;
    private bool _hasAttackReadyPlayed = false;
    
    
    protected override void OnAwake()
    {
        base.OnAwake();
        stateMachine = new ImtStateMachine<ReworkCageBehaviour>(this);
        
        stateMachine.SetStartState<PlantState>();
        
        _abilities.Add(VegetableType.Carrot, _carrotAbilityObject.GetComponent<CarrotAbility>());
        _abilities.Add(VegetableType.Corn, _cornAbilityObject.GetComponent<CornAbility>());
        _abilities.Add(VegetableType.Daikon, _daikonAbilityObject.GetComponent<DaikonAbility>());

        foreach (var data in _plantAttributes)
        {
            PlantAttributeData.Add(data.VegetableType, data);
        }
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
    
    public void LeftSwitchSeed()
    {
        InitPlantState();
        var maxTypeCount = PlantAttributeData.Count;
        var plantKeys = PlantAttributeData.Keys;
        var plantTypes = new VegetableType[maxTypeCount];
        var index = ((int)CurrentVegetableType - 1) % maxTypeCount;
        index = index < 0 ? index + maxTypeCount : index;
        plantKeys.CopyTo(plantTypes, 0);
        CurrentVegetableType = plantTypes[index];
    }
    
    public void RightSwitchSeed()
    {
        InitPlantState();
        var maxTypeCount = PlantAttributeData.Count;
        var plantKeys = PlantAttributeData.Keys;
        var plantTypes = new VegetableType[maxTypeCount];
        plantKeys.CopyTo(plantTypes, 0);
        CurrentVegetableType = plantTypes[(int)(CurrentVegetableType + 1) % maxTypeCount];
    }
    
    private void InitPlantState()
    {
        _hasAttackReadyPlayed = false;
        _prevGrowthStage = GrowthStage.Nothing;
        Timer = 0f;
    }
    
    public void AbilityUse()
    {
        _abilities[CurrentVegetableType]?.UseAbility(Player);
    }
    
    public virtual void OnLeftSwitchAction(InputAction.CallbackContext obj)
    {
        if(!Player.Controller.isPaused) LeftSwitchAction?.Invoke(obj);
    }
    
    public virtual void OnRightSwitchAction(InputAction.CallbackContext obj)
    {
        if(!Player.Controller.isPaused) RightSwitchAction?.Invoke(obj);
    }
    
    public virtual void OnUniqueAction(InputAction.CallbackContext obj)
    {
        if(!Player.Controller.isPaused) UniqueAction?.Invoke(obj);
    }
    
    public virtual void OnFire(InputAction.CallbackContext obj)
    {
        if(!Player.Controller.isPaused) Fire?.Invoke(obj);
    }
}