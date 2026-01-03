using IceMilkTea.StateMachine;
using UnityEngine.InputSystem;

public partial class ReworkCageBehaviour
{
    private class NothingState : ImtStateMachine<ReworkCageBehaviour>.State
    {
    
        // 状態へ突入時の処理はこのEnterで行う
        protected internal override void Enter()
        {
            
        }

        // 状態から脱出する時の処理はこのExitで行う
        protected internal override void Exit()
        {
            
        }
    }
}