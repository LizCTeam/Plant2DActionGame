using System;
using System.Collections;
using IceMilkTea.StateMachine;
using UnityEngine;
using UnityEngine.Assertions;
using Assert = NUnit.Framework.Assert;
using Random = UnityEngine.Random;

public partial class BearBoss : Enemy, IDamageable
{
    private static readonly int IsIdle = Animator.StringToHash("isIdle");

    private class BearBossIdle : ImtStateMachine<BearBoss>.State
    {
        private IEnumerator IdleCoroutine()
        {
            int maxCount = Enum.GetNames(typeof(AttackPattern)).Length;
            var randomValue = (AttackPattern)Random.Range(0, maxCount);
            Context._bearAnimator.SetBool(IsIdle, true);
            yield return new WaitForAnimation(Context._bearAnimator, 0, "BearBossIdle");
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
            
            // switch (randomValue)
            // {
            //     case AttackPattern.BodyBlowToSlam:
            //         stateMachine.SendEvent((int)StateEvent.BodyBlowEnter);
            //         break;
            //     case AttackPattern.SlamToHipDrop:
            //         stateMachine.SendEvent((int)StateEvent.SlamEnter);
            //         break;
            //     case AttackPattern.HipDropToBodyBlow:
            //         stateMachine.SendEvent((int)StateEvent.HipDropEnter);
            //         break;
            //     default:
            //         Assert.IsTrue(false, "Unknown state " + randomValue + "    error!");
            //         break;
            // }
            
            Context._bearAnimator.SetBool(IsIdle, false);
            
        }
        
        // 状態へ突入時の処理はこのEnterで行う
        protected internal override void Enter()
        {
            Context.FaceTarget();
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