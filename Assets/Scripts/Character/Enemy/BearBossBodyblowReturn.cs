

using IceMilkTea.StateMachine;

public partial class BearBoss : Enemy, IDamageable
{
    private class BearBossBodyblowReturn : ImtStateMachine<BearBoss>.State
    {
        // 状態へ突入時の処理はこのEnterで行う
        protected internal override void Enter()
        {
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