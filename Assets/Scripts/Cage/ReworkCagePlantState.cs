
using IceMilkTea.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class ReworkCageBehaviour
{
    private class PlantState : ImtStateMachine<ReworkCageBehaviour>.State
    {
        protected internal override void Enter()
        {
            Context.UniqueAction += UniqueAction;
            Context.Fire += Fire;
            Context.IsGrowing = true;
        }
        
        protected internal override void Update()
        {
            if (Context.Player.Controller.playerAct.UniqueAction.IsPressed())
            {
                float maxTime = Context.PlantAttributeData[Context.CurrentVegetableType].MaxGrowthDuration;
                if (Context.Timer < maxTime)
                {
                    Context.Timer += Time.deltaTime;
                    Context.Timer = Mathf.Clamp(Context.Timer, 0, maxTime);
                    Context._vegeAnimatior.SetInteger(Level, (int)Context.GrowthLevel);
                    print("GrowthLevel : " + Context.GrowthLevel);
                }
            }
        }
        
        private void Fire(InputAction.CallbackContext context)
        {
            if(Context.GrowthLevel == GrowthStage.Nothing) return;
            Context.AbilityUse();
            Context.IsGrowing = false;
            Context.Timer = 0f;
            stateMachine.SendEvent((int)StateEvent.Cancel);
            Context._vegeAnimatior.SetInteger(Level, (int)Context.GrowthLevel);
        }
        
        private void UniqueAction(InputAction.CallbackContext context)
        {
            
        }
        
        protected internal override void Exit()
        {
            Context.UniqueAction -= UniqueAction;
            Context.Fire -= Fire;
        }
    }
}