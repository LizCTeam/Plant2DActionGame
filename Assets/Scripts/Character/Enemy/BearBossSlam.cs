using System.Collections;
using IceMilkTea.StateMachine;
using UnityEngine;

public partial class BearBoss : Enemy, IDamageable
{
    private class BearBossSlam : ImtStateMachine<BearBoss>.State
    {
        private IEnumerator SlamCoroutine()
        {
            Context.FaceTarget();
            Context._bearAnimator.SetInteger(ParameterState, (int)BearAnimationState.Slam);
            yield return new WaitWhile(() => Context._bearAnimator.GetCurrentAnimatorStateInfo(0).IsName("BearBossSlam"));
            yield return new WaitForAnimation(Context._bearAnimator, 0, "BearBossSlam");
            var actionType = Context.EvaluateEvent(Context._actionQueue.Dequeue());
            stateMachine.SendEvent((int)actionType);
        }
        
        // 状態へ突入時の処理はこのEnterで行う
        protected internal override void Enter()
        {
            Context.StartCoroutine(SlamCoroutine());
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