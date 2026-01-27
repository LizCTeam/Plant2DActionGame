
using IceMilkTea.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class ReworkCageBehaviour
{

    private class PlantState : ImtStateMachine<ReworkCageBehaviour>.State
    {

        
        protected internal override void Enter()
        {
            Context.LeftSwitchAction += LeftSwitchAction;
            Context.RightSwitchAction += RightSwitchAction;
            Context.Fire += Fire;
            Context.IsGrowing = true;
        }
        
        protected internal override void Update()
        {
            var currentGrowthStage = Context.GrowthLevel;
            float maxTime = Context.PlantAttributeData[Context.CurrentVegetableType].MaxGrowthDuration;
            if (Context.Timer <= maxTime)
            {
                Context.Timer += Time.deltaTime;
                Context.Timer = Mathf.Clamp(Context.Timer, 0, maxTime);
                Context._vegeAnimatior.SetInteger(Level, (int)currentGrowthStage);
            }

            if (Context._prevGrowthStage == GrowthStage.Nothing && currentGrowthStage != GrowthStage.Nothing && !Context._hasAttackReadyPlayed)
            {
                Context.Player.AttackReadyAnimator.SetTrigger(DoReady);
                Context._hasAttackReadyPlayed = true;
            }
            
            Context._prevGrowthStage = currentGrowthStage;
        }
        
        private void Fire(InputAction.CallbackContext context)
        {
            if(Context.GrowthLevel == GrowthStage.Nothing) return;
            Context.AbilityUse();
            Context.InitPlantState();
            Context._vegeAnimatior.SetInteger(Level, (int)Context.GrowthLevel);
        }
        
        protected internal override void Exit()
        {
            Context.LeftSwitchAction -= LeftSwitchAction;
            Context.RightSwitchAction -= RightSwitchAction;
            Context.Fire -= Fire;
            Context.IsGrowing = false;
        }
        
        private void LeftSwitchAction(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                Context.LeftSwitchSeed();
            }
        }
        
        private void RightSwitchAction(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                Context.RightSwitchSeed();
            }
        }
    }
}