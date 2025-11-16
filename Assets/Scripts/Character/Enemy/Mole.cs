using System;
using IceMilkTea.StateMachine;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class Mole : Enemy, IDamageable
{
    [SerializeField] private Hurtbox hurtbox;
    [SerializeField] private Collider2D DetectorHide;
    [SerializeField] private Collider2D DetectorChaseAway;
    [SerializeField] private GameObject ThrowingObject;
    [SerializeField] private float ThrowingAngle;
    [SerializeField] private float AngleOffset;
    
    private ImtStateMachine<Mole> stateMachine;
    
    private Animator _moleAnimator;
    private GameObject player;
    
    private bool isDetectChaseAway;
    private bool isHide;

    public enum StateEvent
    {
        PlayerEnterHide,
        PlayerEnterChaseAway,
        PlayerExit,
        PlayerNear,
        PlayerAround,
        Dead
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        stateMachine = new ImtStateMachine<Mole>(this);
        //Idle
        stateMachine.AddTransition<MoleIdle, MoleChaseAway>((int)StateEvent.PlayerEnterChaseAway);
        stateMachine.AddTransition<MoleIdle, MoleHide>((int)StateEvent.PlayerEnterHide);
        stateMachine.AddTransition<MoleIdle, MoleDead>((int)StateEvent.Dead);
        
        //ChaseWay
        stateMachine.AddTransition<MoleChaseAway, MoleIdle>((int)StateEvent.PlayerExit);
        stateMachine.AddTransition<MoleChaseAway, MoleHide>((int)StateEvent.PlayerNear);
        stateMachine.AddTransition<MoleChaseAway, MoleDead>((int)StateEvent.Dead);
        
        //Hide
        stateMachine.AddTransition<MoleHide, MoleChaseAway>((int)StateEvent.PlayerAround);
        stateMachine.AddTransition<MoleHide, MoleIdle>((int)StateEvent.PlayerExit);
        
        stateMachine.SetStartState<MoleIdle>();
    }
    
    protected override void OnStart()
    {
        base.OnStart();
        stateMachine.Update();
        _moleAnimator = GetComponent<Animator>();
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
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.IsTouching(DetectorChaseAway))
        {
            if (other.CompareTag("Player"))
            {
                isDetectChaseAway =  true;
                player = other.gameObject;
            }
        }

        if (other.IsTouching(DetectorHide))
        {
            if (other.CompareTag("Player"))
            {
                isHide = true;
                player = other.gameObject;
            }
        }
        print("nope");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.IsTouching(DetectorChaseAway))
        {
            if (other.CompareTag("Player"))
            {
                player = null;
                isDetectChaseAway = false;
            }
        }

        if (!other.IsTouching(DetectorHide))
        {
            if (other.CompareTag("Player"))
            {
                isHide = false;
            }
        }
    }
    
    private void ThrowingBall(GameObject TargetObject)
    {
        
        if (ThrowingObject != null && TargetObject != null)
        {
            GameObject ball = Instantiate(ThrowingObject, this.transform.position, Quaternion.identity);
            
            Vector3 targetPosition = TargetObject.transform.position;
            
            float angle = ThrowingAngle + AngleOffset * Random.Range(-1f, 1f);
            
            Vector3 velocity = CalculateVelocity(this.transform.position, targetPosition, angle);
            
            Rigidbody2D rid = ball.GetComponent<Rigidbody2D>();
            rid.AddForce(velocity * rid.mass, ForceMode2D.Impulse);
            
            print(velocity);
        }
        else
        {
            throw new System.Exception("射出するオブジェクトまたは標的のオブジェクトが未設定です。");
        }
        
    }
    
    private Vector2 CalculateVelocity(Vector2 pointA, Vector2 pointB, float angle)
    {
        float rad = angle * Mathf.PI / 180;
        
        float x = pointA.x - pointB.x;
        
        float y = pointA.y - pointB.y;
        
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            return Vector2.zero;
        }
        else
        {
            return (new Vector2(pointB.x - pointA.x, x * Mathf.Tan(rad)).normalized * speed);
        }
    }
}