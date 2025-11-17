using System.Collections;
using IceMilkTea.StateMachine;

public partial class BearBoss : Enemy, IDamageable
{
    private class BearBossWeakness : ImtStateMachine<BearBoss>.State
    {
        private IEnumerator WeaknessCoroutine()
        {
            Context._bearAnimator.Play("BearBossWeakness");
            yield return new WaitForAnimation(Context._bearAnimator, 0, "BearBossWeakness");
            stateMachine.SendEvent((int)StateEvent.IdleEnter);
        }
        
        // 状態へ突入時の処理はこのEnterで行う
        protected internal override void Enter()
        {
            Context.StartCoroutine(WeaknessCoroutine());
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