using System.Collections;
using IceMilkTea.StateMachine;
using UnityEngine;

public partial class BearBoss : Enemy, IDamageable
{
    private static readonly int DoWeakness = Animator.StringToHash("doWeakness");

    private class BearBossWeakness : ImtStateMachine<BearBoss>.State
    {
        private IEnumerator WeaknessCoroutine()
        {
            Context.FaceTarget();
            Context._bearAnimator.SetInteger(ParameterState, (int)BearAnimationState.Weakness);
            yield return new WaitWhile(() => Context._bearAnimator.GetCurrentAnimatorStateInfo(0).IsName("BearBossWeakness"));
            yield return new WaitForAnimation(Context._bearAnimator, 0, "BearBossWeakness");
            stateMachine.SendEvent((int)StateEvent.IdleEnter);
        }
        
        // 状態へ突入時の処理はこのEnterで行う
        protected internal override void Enter()
        {
            Context._hurtbox.gameObject.SetActive(true);
            Context.StartCoroutine(WeaknessCoroutine());
        }

        // 状態の更新はこのUpdateで行う
        protected internal override void Update()
        {
        }

        // 状態から脱出する時の処理はこのExitで行う
        protected internal override void Exit()
        {
            Context._hurtbox.gameObject.SetActive(false);
        }
    }
}