using IceMilkTea.StateMachine;
using Unity.VisualScripting;
using UnityEditor.Experimental.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public partial class Raccoondog : Enemy, IDamageable
{
    [SerializeField] private Collider2D Carrotreflect;

    public bool isreflect;
    private GameObject Vegetable;


    public enum StateEvent
    {
       ParryEnter,
       AttackEnter,
       MoveEnter,

       IdleEnter
    }

    private Hurtbox _hurtbox;

    private ImtStateMachine<Raccoondog> stateMachine;

    int attackerPosition = 0;
   
    private class IdleState : ImtStateMachine<Raccoondog>.State
    {
        protected internal override void Update()
        {
            
            GameObject player = GameObject.FindWithTag("Player");
            GameObject vegetables = GameObject.FindWithTag("Vegetables");
           
            if(player != null)
            {
                float dist = Vector2.Distance(player.transform.position, Context.transform.position);

                if(dist < 7.0f)
                {
                    Context.stateMachine.SendEvent((int)StateEvent.AttackEnter);
                }
            }

            if (vegetables != null)
            {
                float Vegetabledist = Vector2.Distance(vegetables.transform.position, Context.transform.position);

                if(Vegetabledist < 4.0f)
                {
                    Context.stateMachine.SendEvent((int)StateEvent.ParryEnter);
                }
                
            }

        }

    }

    private class moveState : ImtStateMachine<Raccoondog>.State
    {
        protected internal override void Update()
        {

            GameObject player = GameObject.FindWithTag("Player");
            GameObject vegetables = GameObject.FindWithTag("Vegetables");


            if (player != null)
            {
                float dist = Vector2.Distance(player.transform.position, Context.transform.position);

                if (dist < 5.0f)
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
    }

    private class AttackState : ImtStateMachine<Raccoondog>.State
    {
        private float attackDuration = 0.5f;
        private float elapsedTime;
   
        protected internal override void Enter()
        {
           base.Enter();
           elapsedTime = 0f;

        }

        protected internal override void Update()
        {
            elapsedTime += Time.deltaTime;

            Collider2D[] hits = Physics2D.OverlapCircleAll(Context.transform.position, 3.0f);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    var damageable = hit.GetComponent<IDamageable>();
                    if(damageable != null)
                    {
                        damageable.OnDamaged(10);
                    }

                    Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                    if(rb != null)
                    {
                        Vector2 knockbackDir = (hit.transform.position - Context.transform.position).normalized;
                        float knockbackForce = 5f;

                        rb.AddForce(knockbackDir * knockbackForce,ForceMode2D.Impulse);
                    }

                }
               
            }

            if (elapsedTime >= attackDuration)
            {
                Context.stateMachine.SendEvent((int)StateEvent.IdleEnter);
            }

        }

    }

    private class parryState : ImtStateMachine<Raccoondog>.State
    {
       


        protected internal override void Update()
        {

            base.Update();
             
        }
    }


    protected override void OnAwake()
    {
        base.OnAwake();
        stateMachine = new ImtStateMachine<Raccoondog>(this);
        stateMachine.AddTransition<IdleState, AttackState>((int)StateEvent.AttackEnter);
        stateMachine.AddTransition<IdleState,parryState>((int)StateEvent.ParryEnter);
        stateMachine.AddTransition<IdleState,moveState>((int)StateEvent.MoveEnter);

        stateMachine.AddTransition<moveState, AttackState>((int)StateEvent.AttackEnter);
        stateMachine.AddTransition<moveState, parryState>((int)StateEvent.ParryEnter);

        stateMachine.AddTransition<AttackState, IdleState>((int)StateEvent.IdleEnter);
        stateMachine.AddTransition<AttackState, parryState>((int)StateEvent.ParryEnter);

        stateMachine.AddTransition<parryState, IdleState>((int)StateEvent.IdleEnter);
        stateMachine.AddTransition<parryState, moveState>((int)StateEvent.MoveEnter);

        stateMachine.SetStartState<IdleState>();

    }


    protected override void OnStart()
    {
        base.OnStart();
        stateMachine.Update();
    }

   protected override void OnUpdate()
    {
        base .OnUpdate();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject vegetables = GameObject.FindWithTag("Vegetables");
        if (vegetables != null)
        {
            
            Rigidbody2D rb = other.attachedRigidbody;
                if (rb != null)
                {
               
                    Vector2 incoming = (other.transform.position - transform.position).normalized * 10f;


                    Vector2 normal = Carrotreflect.transform.up;

                    Vector2 reflected = Vector2.Reflect(incoming, normal);
                    other.transform.position += (Vector3)(reflected.normalized * 0.5f);
                    rb.linearVelocity = reflected.normalized * 10f;
                    Debug.Log($"Projectile reflected! incoming={incoming}, reflected={reflected}");


                }
        }
        
    }








}
