using System.Collections;
using IceMilkTea.StateMachine;
using UnityEngine;

public partial class Player
{
    private static readonly int DaikonAttack = Animator.StringToHash("DaikonAttack");

    public class PlayerDaikonAttack : ImtStateMachine<Player>.State
    {
        protected internal override void Enter()
        {
            Context._body.linearVelocityX = 0;
            Context.IsAttacking = true;
            Context._playerAnimator.SetTrigger(DaikonAttack);
            //Context.StartCoroutine(AttackAnimationTask(Context._playerAnimator));
        }
        
        protected internal override void Update()
        {
            if (!Context._playerAnimator.IsInTransition(0) && Context._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                stateMachine.SendEvent((int)StateEvent.AttackFinish);
            }
        }
        
        protected internal override void Exit()
        {
            Context.IsAttacking = false;
        }

        // private IEnumerator AttackAnimationTask(Animator animator)
        // {
        //     yield return new WaitForAnimation(animator, 0, "DaikonAttack");
        // }
    }
}

