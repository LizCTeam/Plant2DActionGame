using System.Numerics;
using IceMilkTea.StateMachine;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public partial class BearBoss : Enemy, IDamageable
{
    private static readonly int IsBodyBlow = Animator.StringToHash("isBodyBlow");

    private class BearBossBodyblow : ImtStateMachine<BearBoss>.State
    {
        Vector2 previousPlayerToDirection = Vector2.zero;
        
        // 状態へ突入時の処理はこのEnterで行う
        protected internal override void Enter()
        {
            this.Context._bearAnimator.SetBool(IsBodyBlow, true);
            previousPlayerToDirection = (Context._player.transform.position - Context.transform.position).normalized;
        }

        // 状態の更新はこのUpdateで行う
        protected internal override void Update()
        {
            Context.SpriteFlip();
            var playerToDirection = (Context._player.transform.position - Context.transform.position).normalized;
            Context._targetDirection =  playerToDirection;
            Context.Move();
            if (previousPlayerToDirection.x * playerToDirection.x < 0f)
            {
                var actionType = Context.EvaluateEvent(Context._actionQueue.Dequeue());
                stateMachine.SendEvent((int)actionType);
            }
        }

        // 状態から脱出する時の処理はこのExitで行う
        protected internal override void Exit()
        {
            this.Context._bearAnimator.SetBool(IsBodyBlow, false);
        }
    }
}