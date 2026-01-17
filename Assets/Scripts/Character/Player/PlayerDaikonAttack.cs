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
            Context.IsAttacking = true;
            Context._playerAnimator.SetTrigger(DaikonAttack);
            //Context.StartCoroutine(AttackAnimationTask(Context._playerAnimator));
            if (Context.CurrentStage == ReworkCageBehaviour.GrowthStage.Mature)
            {
                Dash();
            }
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
            Context.Hurtbox.gameObject.SetActive(true);
            Context.IsAttacking = false;
        }

        private void Dash()
        {
            Context.Hurtbox.gameObject.SetActive(false);
            var visualScale = Context.VisualRoot.transform.localScale;
            var dash = new Vector2(visualScale.x * Context._dashForce, 0);
            Context._body.AddForce(dash, ForceMode2D.Impulse);
        }
        // private IEnumerator AttackAnimationTask(Animator animator)
        // {
        //     yield return new WaitForAnimation(animator, 0, "DaikonAttack");
        // }
    }
}

