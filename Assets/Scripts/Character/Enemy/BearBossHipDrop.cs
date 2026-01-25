using System.Collections;
using IceMilkTea.StateMachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public partial class BearBoss : Enemy, IDamageable
{
    [Header("熊ちゃんのジャンプ力")]
    [SerializeField] public float JumpHeight = 10f;
    
    [Header("着地時の石の飛び散り具合")]
    [SerializeField] public int minStoneAmount = 3;
    [SerializeField] public int maxStoneAmount = 5;
    
    private class BearBossHipDrop : ImtStateMachine<BearBoss>.State
    {
        private IEnumerator HipDropCoroutine()
        {
            Context._bearAnimator.SetInteger(ParameterState, (int)BearAnimationState.Jump);
            yield return new WaitWhile(() => Context._bearAnimator.GetCurrentAnimatorStateInfo(0).IsName("BearBossJump"));
            yield return new WaitUntil(() => Context.IsJumpReady);

            var playerPos = Context._player.transform.position;
            var targetPos = new Vector3(playerPos.x, Context.transform.position.y + Context.JumpHeight, Context.transform.position.z);

            var isJumpFinish = false;
            Context.transform.DOMove(targetPos, 1f)
                .SetEase(Ease.OutQuart)
                .OnComplete(() => isJumpFinish = true);
            yield return new WaitUntil(() => isJumpFinish);

            Context._bearAnimator.SetInteger(ParameterState, (int)BearAnimationState.HipDrop);
            yield return new WaitWhile(() => Context._bearAnimator.GetCurrentAnimatorStateInfo(0).IsName("BearBossHipDrop"));
            yield return new WaitForAnimation(Context._bearAnimator, 0, "BearBossHipDrop");

            yield return new WaitUntil(() => Context.isGrounded());

            Context.HipDropStone();

            var actionType = Context.EvaluateEvent(Context._actionQueue.Dequeue());
            stateMachine.SendEvent((int)actionType);
        }
        
        // 状態へ突入時の処理はこのEnterで行う
        protected internal override void Enter()
        {
            Context.StartCoroutine(HipDropCoroutine());
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