using IceMilkTea.StateMachine;
using UnityEngine;

public partial class Mole : Enemy, IDamageable
{
    private class MoleHide : ImtStateMachine<Mole>.State
    {
        protected internal override void Enter()
        {
            Context.hurtbox.collider.enabled = false;
            Context._moleAnimator.Play("MoleHideOn");
        }
        
        protected internal override void Update()
        {
            if (!Context.isDetectChaseAway && !Context.isHide)
            {
                stateMachine.SendEvent((int)StateEvent.PlayerExit);
                print("Hide to Idle");
            }
            else if (Context.isDetectChaseAway && !Context.isHide)
            {
                stateMachine.SendEvent((int)StateEvent.PlayerAround);
                print("Hide to ChaseAway");
            }
        }
        
        protected internal override void Exit()
        {
            Context.hurtbox.collider.enabled = true;
            Context._moleAnimator.Play("MoleHideOff");
        }
    }
}