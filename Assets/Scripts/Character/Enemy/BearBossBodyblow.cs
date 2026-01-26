using System;
using System.Numerics;
using IceMilkTea.StateMachine;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public partial class BearBoss
{
    private class BearBossBodyblow : ImtStateMachine<BearBoss>.State
    {
        private Vector2 _previousPlayerToDirection = Vector2.zero;
        private bool _hasPassPlayer = false;
        private float _passPlayerTimer = 0.0f;
        
        // 状態へ突入時の処理はこのEnterで行う
        protected internal override void Enter()
        {
            Context._bearAnimator.SetInteger(ParameterState, (int)BearAnimationState.BodyBlow);
            _previousPlayerToDirection = (Context._player.transform.position - Context._headPos.transform.position).normalized;
            _hasPassPlayer = false;
            _passPlayerTimer = 0.0f;
            if (Math.Abs(Context._player.transform.position.x - Context._headPos.transform.position.x) < 2.0f)
            {
                var actionType = Context.EvaluateEvent(Context._actionQueue.Dequeue());
                stateMachine.SendEvent((int)actionType);
            }
        }

        // 状態の更新はこのUpdateで行う
        protected internal override void Update()
        {
            if (_hasPassPlayer)
            {
                _passPlayerTimer += Time.deltaTime;
            }

            var playerToDirection = (Context._player.transform.position - Context._headPos.transform.position).normalized;
            Context._targetDirection =  playerToDirection;

            Context.SpriteFlip();

            if (Context.IsWallAhead(new Vector2(playerToDirection.x, 0).normalized, WallCheckDistance))
            {
                var actionType = Context.EvaluateEvent(Context._actionQueue.Dequeue());
                stateMachine.SendEvent((int)actionType);
                return;
            } 

            switch (_hasPassPlayer)
            {
                case false when _previousPlayerToDirection.x * playerToDirection.x < 0f:
                    Context._targetDirection = Vector2.zero;
                    _hasPassPlayer = true;
                    break;
                case true when _passPlayerTimer > 1.5f || Math.Abs(Context._body.linearVelocityX) < 1.0f:
                {
                    var actionType = Context.EvaluateEvent(Context._actionQueue.Dequeue());
                    stateMachine.SendEvent((int)actionType);
                    break;
                }
            }
        }

        // 状態から脱出する時の処理はこのExitで行う
        protected internal override void Exit()
        {
            Context._targetDirection = Vector2.zero;
        }
    }
}