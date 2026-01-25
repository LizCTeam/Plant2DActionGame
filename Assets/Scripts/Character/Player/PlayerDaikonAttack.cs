using System.Collections;
using IceMilkTea.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

public partial class Player
{
    private static readonly int DaikonAttack = Animator.StringToHash("DaikonAttack");
    
    [Header("大根のプロパティ")]
    [SerializeField] private float _dashForce = 50f;

    public class PlayerDaikonAttack : ImtStateMachine<Player>.State
    {
        protected internal override void Enter()
        {
            Context._body.linearVelocityX = 0;
            Context.StartCoroutine(AttackAnimationTask());
        }
        
        protected internal override void Exit()
        {
            Context.Hurtbox.gameObject.SetActive(true);
        }
        
        private IEnumerator AttackAnimationTask()
        {
            Context.IsAttacking = true;
            yield return new WaitUntil(() => Context._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("DaikonAttack"));
            yield return new WaitForAnimation(Context._playerAnimator, 0, "DaikonAttack");
            Context.IsAttacking = false;
            stateMachine.SendEvent((int)StateEvent.AttackFinish);
        }
    }
}

