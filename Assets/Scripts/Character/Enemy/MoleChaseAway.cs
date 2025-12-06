using System;
using IceMilkTea.StateMachine;
using UnityEngine;

public partial class Mole : Enemy, IDamageable
{
    private float _throwCooldown = 0f;
    private float _maxCooldown = 0.8f;
    
    private class MoleChaseAway : ImtStateMachine<Mole>.State
    {
        protected internal override void Enter()
        {
            
        }
        
        protected internal override void Update()
        {
            Context._throwCooldown -= Time.deltaTime;
            
            if (Context._throwCooldown <= 0.0f)
            {
                Context.ThrowingBall(Context.player);
                Context._throwCooldown = Context._maxCooldown;
            }
            
            if (!Context.isDetectChaseAway && !Context.isHide)
            {
                stateMachine.SendEvent((int)StateEvent.PlayerExit);
            }
            
            if (Context.isDetectChaseAway && Context.isHide)
            {
                stateMachine.SendEvent((int)StateEvent.PlayerNear);
                return;
            }
        }
        
        protected internal override void Exit()
        {
        }
    }
    
}