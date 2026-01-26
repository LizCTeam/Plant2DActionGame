using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : BasicBehaviour
{
    [SerializeField]
    protected float _speed = 4f;
    
    public LayerMask GroundLayer;
    public Rigidbody2D _body; //改名したいけど使っている場所が多いから変更しないよ
    private Collider2D _collider;

    protected override void OnAwake()
    {
        base.OnAwake();
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }
    
    protected override void OnUpdate()
    {
        base.OnUpdate();

    }
    
    public bool isGrounded()
    {
        //BoxColliderにあるCastを使って地面の判定をboolで返します。
        var hit = Physics2D.BoxCast(
            _collider.bounds.center, 
            _collider.bounds.size, 
            0f, 
            Vector2.down, 
            .1f, 
            GroundLayer
        );
        return hit && Vector2.Dot(hit.normal, Vector2.up) > 0.5;
    }
}
