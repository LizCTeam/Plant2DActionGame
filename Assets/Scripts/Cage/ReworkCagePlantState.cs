
using IceMilkTea.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class ReworkCageBehaviour
{
    private class PlantState : ImtStateMachine<ReworkCageBehaviour>.State
    {
        protected internal override void Enter()
        {
            Context.SwitchAction += SwitchAction;
            Context.Fire += Fire;
            Context.IsGrowing = true;
        }
        
        protected internal override void Update()
        {
            float maxTime = Context.PlantAttributeData[Context.CurrentVegetableType].MaxGrowthDuration;
            if (Context.Timer < maxTime)
            {
                Context.Timer += Time.deltaTime;
                Context.Timer = Mathf.Clamp(Context.Timer, 0, maxTime);
                Context._vegeAnimatior.SetInteger(Level, (int)Context.GrowthLevel);
            }
        }
        
        private void Fire(InputAction.CallbackContext context)
        {
            if(Context.GrowthLevel == GrowthStage.Nothing) return;
            Context.AbilityUse();
            InitPlantState();
            Context._vegeAnimatior.SetInteger(Level, (int)Context.GrowthLevel);
        }
        
        protected internal override void Exit()
        {
            Context.SwitchAction -= SwitchAction;
            Context.Fire -= Fire;
            Context.IsGrowing = false;
        }
        
        private void SwitchAction(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                Context.SwitchSeed();
            }
        }

        private void InitPlantState()
        {
            Context.Timer = 0f;
        }
    }
}