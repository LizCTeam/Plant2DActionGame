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
    public event Action<InputAction.CallbackContext> SwitchAction;
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
    
    private static readonly int Level = Animator.StringToHash("GrowthLevel");
    private ImtStateMachine<ReworkCageBehaviour> stateMachine;
    
    
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
        var maxTypeCount = PlantAttributeData.Count;
        var plantKeys = PlantAttributeData.Keys;
        var plantTypes = new VegetableType[maxTypeCount];
        plantKeys.CopyTo(plantTypes, 0);
        CurrentVegetableType = plantTypes[(int)(CurrentVegetableType + 1) % maxTypeCount];
    }
    
    public void AbilityUse()
    {
        _abilities[CurrentVegetableType]?.UseAbility(Player);
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