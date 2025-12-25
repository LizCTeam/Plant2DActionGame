using UnityEngine;
using IceMilkTea.StateMachine;

public class Crow : Enemy,IDamageable
{
    [SerializeField] private Hurtbox hurtbox;

    public enum StateEvent
    {
        MoveEnter,
        AttackEnter,

        IdleEnter
    }

    private class IdleState : ImtStateMachine<Crow>.State
    {

    }

    private class MoveState : ImtStateMachine<Crow>.State
    {
        private float moveSpeed = 2f;
        private bool movingLeft = true;

        protected internal override void Update()
        {
            float dir = movingLeft ? 1 : -1;

            Context.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-dir * moveSpeed, Context.GetComponent<Rigidbody2D>().linearVelocity.y);

            RaycastHit2D wallHit = Physics2D.Raycast(Context.transform.position, Vector2.right * (movingLeft ? 1 : -1), 0.2f, LayerMask.GetMask("Ground"));
            if (wallHit.collider != null)
            {
                Flip();
            }

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                float dist = Vector2.Distance(player.transform.position, Context.transform.position);

                if (dist < 4.0f)
                {
                    Context.stateMachine.SendEvent((int)StateEvent.AttackEnter);
                }
            }
        }

        private void Flip()
        {
            movingLeft = !movingLeft;
            Vector3 scale = Context.transform.localScale;
            scale.x *= 1;
            Context.transform.localScale = scale;
        }
    }

    private class AttackState : ImtStateMachine<Crow>.State
    {

    }

    private ImtStateMachine<Crow> stateMachine;

    protected override void OnAwake()
    {
        base.OnAwake();
        stateMachine = new ImtStateMachine<Crow>(this);
        stateMachine.AddTransition<IdleState, MoveState>((int)StateEvent.MoveEnter);
        stateMachine.AddTransition<IdleState,AttackState>((int)StateEvent.AttackEnter);

        stateMachine.AddTransition<MoveState, AttackState>((int)StateEvent.AttackEnter);

        stateMachine.AddTransition<AttackState,MoveState>((int)StateEvent.MoveEnter);

        stateMachine.SetStartState<MoveState>();
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

    public void OnDamaged(int damage)
    {
        this._hp -= damage;
    }
}