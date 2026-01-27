using System;
using System.Collections.Generic;
using IceMilkTea.StateMachine;
using UnityEngine;

public partial class Mole : Enemy, IDamageable
{
    private float _throwCooldown = 0.5f;
    private float _maxCooldown = 2.0f;
    
    private class MoleChaseAway : ImtStateMachine<Mole>.State
    {
        protected internal override void Enter()
        {
            
        }
        
        protected internal override void Update()
        {
            Context._throwCooldown -= Time.deltaTime;
            
            if (Context._throwCooldown <= 0.0f && !DetectGround())
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

        private bool DetectGround()
        {
            Vector2 origin = Context.transform.position;
            Vector2 target = Context.player.transform.position;
            Vector2 dir = target - origin;
            float distance = dir.magnitude;

            origin.x += 1 * Mathf.Sign(dir.x);
            
            RaycastHit2D hit = Physics2D.Raycast(
                origin,
                dir.normalized,
                distance,
                Context.GroundLayer
            );
            
            return hit.collider;
        }
    }
    
}