using IceMilkTea.StateMachine;
using UnityEngine;

public partial class Mole : Enemy, IDamageable
{
    private class MoleIdle : ImtStateMachine<Mole>.State
    {
        protected internal override void Enter()
        {
            //アニメーション再生など
        }
        
        protected internal override void Update()
        {
            if (Context.isDetectChaseAway)
            {
                stateMachine.SendEvent((int)StateEvent.PlayerEnterChaseAway);
                return;
            }

            if (Context.isHide)
            {
                stateMachine.SendEvent((int)StateEvent.PlayerEnterHide);
                return;
            }
        }
        
        protected internal override void Exit()
        {
        }
    }
}