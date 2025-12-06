using System.Collections.Generic;
using IceMilkTea.StateMachine;
using DG.Tweening;
using UnityEngine;

public class CarrotMissile : ProjectileBehaviour
{
    public SpriteRenderer sprite;
    
    private ImtStateMachine<CarrotMissile> stateMachine;
    
    #region 状態遷移(ステート)
    
    public enum StateEvent
    {
        LaunchFinish,
    }
    
    private class LaunchState : ImtStateMachine<CarrotMissile>.State
    {
        private float goalPosition;
        
        // 状態へ突入時の処理はこのEnterで行う
        protected internal override void Enter()
        {
            //goal_position = parent.global_position.y - goal_position
            goalPosition = 5;
            goalPosition = this.Context.transform.position.y + goalPosition;
            this.Context.transform.DOMoveY(goalPosition, 0.8f)
                .SetEase(Ease.OutQuart)
                .SetDelay(0.2f)
                .OnComplete(() =>
                {
                    stateMachine.SendEvent((int)StateEvent.LaunchFinish);
                });
        }

        // 状態の更新はこのUpdateで行う
        protected internal override void Update()
        {
            //positionは構造体なので変数に入れてから値を変えなければいけない(バージョンがC#9より上だとより良い書き方がある)
            // var parentPosition = this.Context.transform.position;
            // parentPosition.y += Context.speed * Time.deltaTime;
            // this.Context.transform.position = parentPosition;
            // if (this.Context.transform.position.y >= goalPosition)
            // {
            //     stateMachine.SendEvent((int)StateEvent.LaunchFinish);
            // }
        }

        // 状態から脱出する時の処理はこのExitで行う
        protected internal override void Exit()
        {
        }
    }

    private class LockOnState : ImtStateMachine<CarrotMissile>.State
    {
        GameObject target;
        
        // 状態へ突入時の処理はこのEnterで行う
        protected internal override void Enter()
        {
            float minDistance = float.MaxValue;
            GameObject nearestEnemy = null;
            List<GameObject> enemies = new List<GameObject>();
            enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
            foreach (var enemyObj in enemies)
            {
                Component[] components = (Component[])enemyObj.GetComponents(typeof(Enemy));
                if (components.Length == 0)
                {
                    continue;
                }
                float distance = (enemyObj.transform.position - this.Context.transform.position).magnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = enemyObj;
                }
            }

            target = nearestEnemy;
        }

        // 状態の更新はこのUpdateで行う
        protected internal override void Update()
        {
            if (target)
            {
                var direction = (target.transform.position - this.Context.transform.position).normalized;
                Context.Move(direction, Context.speed);
            }
        }

        // 状態から脱出する時の処理はこのExitで行う
        protected internal override void Exit()
        {
        }
    }
    
    #endregion
    
    protected override void OnAwake()
    {
        base.OnAwake();
        stateMachine = new ImtStateMachine<CarrotMissile>(this);
        stateMachine.AddTransition<LaunchState, LockOnState>((int)StateEvent.LaunchFinish);
        
        stateMachine.SetStartState<LaunchState>();
    }
    
    protected override void OnStart()
    {
        base.OnStart();
        stateMachine.Update();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        stateMachine.Update();
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
    
    public void Move(Vector3 direction, float speed)
    {
        const float SpriteOffset = 135;
        //現在の位置を保存
        var currentPosition = transform.position;
        //移動
        transform.position += direction * (speed * Time.deltaTime);
        //回転させる
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sprite.transform.rotation = Quaternion.AngleAxis(angle + SpriteOffset, Vector3.forward);
    }
}