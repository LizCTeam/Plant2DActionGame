using IceMilkTea.StateMachine;
using UnityEngine;

public partial class Mole : Enemy, IDamageable
{
    private class MoleDead : ImtStateMachine<Mole>.State
    {
        protected internal override void Enter()
        {
        }
        
        protected internal override void Update()
        {
        }
        
        protected internal override void Exit()
        {
        }
    }
}