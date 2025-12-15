using IceMilkTea.StateMachine;
using UnityEngine;

public partial class Raccoondog : Enemy, IDamageable
{
    [SerializeField] private Collider2D Carrotreflect;

    public bool IsReflect;
    private GameObject _vegetable;


    public enum StateEvent
    {
        ParryEnter,
        AttackEnter,
        MoveEnter,

        IdleEnter
    }


    private ImtStateMachine<Raccoondog> stateMachine;

    private class IdleState : ImtStateMachine<Raccoondog>.State
    {
        protected internal override void Update()
        {

            GameObject player = GameObject.FindWithTag("Player");
            GameObject vegetables = GameObject.FindWithTag("Vegetables");

            if (player != null)
            {
                float dist = Vector2.Distance(player.transform.position, Context.transform.position);

                if (dist < 7.0f)
                {
                    Context.stateMachine.SendEvent((int)StateEvent.AttackEnter);
                }
            }

            if (vegetables != null)
            {
                float _vegetableDist = Vector2.Distance(vegetables.transform.position, Context.transform.position);

                if (_vegetableDist < 4.0f)
                {
                    Context.stateMachine.SendEvent((int)StateEvent.ParryEnter);
                }

            }

        }

    }

    private class moveState : ImtStateMachine<Raccoondog>.State
    {

        private float moveSpeed = 2f;
        private bool movingLeft = true;


        protected internal override void Update()
        {
            float dir = movingLeft ? 1 : -1;
            
            Context.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-dir * moveSpeed, Context.GetComponent<Rigidbody2D>().linearVelocity.y);
            Transform groundCheck = Context.transform.Find("GroundCheck"); 
            if (groundCheck != null)
            {
                if (!Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground")))
                {
                    Flip();
                }
            }



            RaycastHit2D wallHit = Physics2D.Raycast(Context.transform.position, Vector2.right * (movingLeft ? 1 : -1), 0.2f, LayerMask.GetMask("Ground"));
            if (wallHit.collider != null)
            {
                Flip();
            }

            GameObject player = GameObject.FindWithTag("Player");
            GameObject vegetables = GameObject.FindWithTag("Vegetables");


            if (player != null)
            {
                float dist = Vector2.Distance(player.transform.position, Context.transform.position);

                if (dist < 4.0f)
                {
                    Context.stateMachine.SendEvent((int)StateEvent.AttackEnter);
                }
            }

            if (vegetables != null)
            {
                float Vegetabledist = Vector2.Distance(vegetables.transform.position, Context.transform.position);

                if (Vegetabledist < 4.0f)
                {
                    Context.stateMachine.SendEvent((int)StateEvent.ParryEnter);
                }


            }

        }
        private void Flip()
        {
            movingLeft = false;
            Vector3 scale = Context.transform.localScale;
            scale.x *= 1;
            Context.transform.localScale = scale;
        }



    }

    private class AttackState : ImtStateMachine<Raccoondog>.State
    {
        private float _attackDuration = 0.5f;
        private float _elapsedTime;

        protected internal override void Enter()
        {
            base.Enter();
            _elapsedTime = 0f;

        }

        protected internal override void Update()
        {
            _elapsedTime += Time.deltaTime;

            Collider2D[] hits = Physics2D.OverlapCircleAll(Context.transform.position, 3.0f);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {

                    Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 knockbackDir = (hit.transform.position - Context.transform.position).normalized;
                        float knockbackForce = 2f;

                        rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
                    }

                }

            }

            if (_elapsedTime >= _attackDuration)
            {
               
            }

            if (_elapsedTime >= _attackDuration)
            {
                Context.stateMachine.SendEvent((int)StateEvent.MoveEnter);
            }

        }

    }

    private class parryState : ImtStateMachine<Raccoondog>.State
    {
        protected internal override void Update()
        {
            base.Update();
            Collider2D[] hits = Physics2D.OverlapCircleAll(Context.transform.position, 2.0f);
            foreach (var hit in hits)
            {
                GameObject vegetables = GameObject.FindWithTag("Vegetables");
                if (vegetables != null)
                {
                    Object.Destroy(vegetables);
                    Debug.Log("hannn");
                }
            }
           
            Context.stateMachine.SendEvent((int)StateEvent.MoveEnter);

        }
    }


    protected override void OnAwake()
    {
        base.OnAwake();
        stateMachine = new ImtStateMachine<Raccoondog>(this);
        stateMachine.AddTransition<IdleState, AttackState>((int)StateEvent.AttackEnter);
        stateMachine.AddTransition<IdleState, parryState>((int)StateEvent.ParryEnter);
        stateMachine.AddTransition<IdleState, moveState>((int)StateEvent.MoveEnter);

        stateMachine.AddTransition<moveState, AttackState>((int)StateEvent.AttackEnter);
        stateMachine.AddTransition<moveState, parryState>((int)StateEvent.ParryEnter);

        stateMachine.AddTransition<AttackState, moveState>((int)StateEvent.MoveEnter);
        stateMachine.AddTransition<AttackState, parryState>((int)StateEvent.ParryEnter);

        stateMachine.AddTransition<parryState, IdleState>((int)StateEvent.IdleEnter);
        stateMachine.AddTransition<parryState, moveState>((int)StateEvent.MoveEnter);

        stateMachine.SetStartState<moveState>();

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
