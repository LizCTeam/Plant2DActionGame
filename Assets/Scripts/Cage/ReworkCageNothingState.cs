using IceMilkTea.StateMachine;
using UnityEngine.InputSystem;

public partial class ReworkCageBehaviour
{
    private class NothingState : ImtStateMachine<ReworkCageBehaviour>.State
    {
    
        // 状態へ突入時の処理はこのEnterで行う
        protected internal override void Enter()
        {
            Context.UniqueAction += UniqueAction;
            Context.SwitchAction += SwitchAction;
            Context.IsGrowing = false;
            Context.Timer = 0f;
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
                Context.SeedText.text = "seed : " + Context.CurrentVegetableType;
            }
        }

        // 状態から脱出する時の処理はこのExitで行う
        protected internal override void Exit()
        {
            Context.UniqueAction -= UniqueAction;
            Context.SwitchAction -= SwitchAction;
        }
    }
}