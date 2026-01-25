using System;
using System.Collections;
using IceMilkTea.StateMachine;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public partial class BearBoss
{
    private static readonly int ParameterState = Animator.StringToHash("state");

    private class BearBossIdle : ImtStateMachine<BearBoss>.State
    {
        private IEnumerator IdleCoroutine()
        {
            Context.FaceTarget();
            
            int maxCount = Enum.GetNames(typeof(AttackPattern)).Length;
            var randomValue = (AttackPattern)Random.Range(0, maxCount);
            Context._bearAnimator.SetInteger(ParameterState, (int)BearAnimationState.Idle);
            if (Context._useAttackPatternOverride)
            {
                randomValue = Context._attackPatternOverride;
            }
            switch (randomValue)
            {
                case AttackPattern.BodyBlowToSlam:
                    Context._actionQueue.Enqueue(ActionType.BodyBlow);
                    Context._actionQueue.Enqueue(ActionType.Slam);
                    Context._actionQueue.Enqueue(ActionType.Weakness);
                    break;
                case AttackPattern.SlamToHipDrop:
                    Context._actionQueue.Enqueue(ActionType.Slam);
                    Context._actionQueue.Enqueue(ActionType.HipDrop);
                    Context._actionQueue.Enqueue(ActionType.Weakness);
                    break;
                case AttackPattern.HipDropToBodyBlow:
                    Context._actionQueue.Enqueue(ActionType.HipDrop);
                    Context._actionQueue.Enqueue(ActionType.BodyBlow);
                    Context._actionQueue.Enqueue(ActionType.Weakness);
                    break;
                default:
                    Assert.IsTrue(false, "Unknown state " + randomValue + "    error!");
                    break;
            }
            
            var actionType = Context.EvaluateEvent(Context._actionQueue.Dequeue());
            stateMachine.SendEvent((int)actionType);
            
            yield return new WaitForSeconds(1.5f);
        }
        
        // 状態へ突入時の処理はこのEnterで行う
        protected internal override void Enter()
        {
            Context.StartCoroutine(IdleCoroutine());
        }

        // 状態の更新はこのUpdateで行う
        protected internal override void Update()
        {
        }

        // 状態から脱出する時の処理はこのExitで行う
        protected internal override void Exit()
        {
        }
    }
    
}