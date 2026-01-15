using System;
using IceMilkTea.StateMachine;
using Unity.Mathematics;
using UnityEngine;

public partial class Player
{
    public class PlayerNormal : ImtStateMachine<Player>.State
    {
        protected internal override void Enter()
        {
        }
        
        protected internal override void Update()
        {
            var playerAct = Context.Controller.playerAct;
            
            Context.UpdateSpriteDirection();
            Context.UpdateCoyoteTime();
        
            if (playerAct.Jump.WasPressedThisFrame())
            {
                Context._jumpBufferTimeCounter =  Context._jumpBufferTime;
            }
            else
            {
                Context._jumpBufferTimeCounter -= Time.deltaTime;
            }
        
            if (Context._coyoteTimeCounter > 0f && Context._jumpBufferTimeCounter > 0f)
            {
                Context._body.linearVelocityY = Context.jumpForce;

                Context._jumpBufferTimeCounter = 0f;
            }

            if (playerAct.Jump.WasReleasedThisFrame() && Context._body.linearVelocityY > 0f)
            {
                Context._body.linearVelocityY *= 0.5f;
                Context._coyoteTimeCounter = 0f;
            }

            if (playerAct.Fire.WasPressedThisFrame() && Context.AvailableWeaponHit > 0)
            {
                stateMachine.SendEvent((int)StateEvent.AttackStart);
                Context.AvailableWeaponHit -= 1;
                if (Context.AvailableWeaponHit <= 0)
                {
                    Context.AvailableWeaponHit = 0;
                }
            }

            // if (Context.isGrounded() && Mathf.Abs(Context._playerController.inputDirection.x) < 0.01f)
            // {
            //     float amount = Mathf.Min(Mathf.Abs(Context._body.linearVelocity.x), Mathf.Abs(Context.frictionAmount));
            // }
        
            if (Context.Hp <= 0)
            {
                //Deadアニメーションを再生
                //ゲームオーバー画面を表示
                //操作不可状態にする
            }
            
            Context.Move();
        }
        
        protected internal override void Exit()
        {
        }
    }
}


