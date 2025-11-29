using UnityEngine;
using UnityEngine.InputSystem;

public class Character : BasicBehaviour
{
    [SerializeField] protected int _hp = 5;
    [SerializeField] protected int _maxHp = 5;
    
    [SerializeField]
    protected float _speed = 4f;
    
    public LayerMask GroundLayer;
    protected Rigidbody2D _body;

    protected override void OnAwake()
    {
        base.OnAwake();
        _body = GetComponent<Rigidbody2D>();
    }
    
    protected override void OnUpdate()
    {
        base.OnUpdate();
    }
    
    protected bool isGrounded()
    {
        //BoxColliderにあるCastを使って地面の判定をboolで返します。
        Collider2D c = GetComponent<Collider2D>();
        return Physics2D.BoxCast(c.bounds.center, c.bounds.size, 0f, Vector2.down, .1f, GroundLayer);
    }
}
