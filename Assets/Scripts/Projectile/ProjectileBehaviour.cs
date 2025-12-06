using UnityEngine;

public class ProjectileBehaviour : BasicBehaviour, IDamageDealt
{
    [SerializeField] 
    private GameObject _collisionObject;
    private Collider2D _collision;
    
    public float speed = 4.5f;
    Rigidbody2D _body;
    
    #region 発射物の関数

    protected override void OnStart()
    {
        base.OnStart();
        _body = GetComponent<Rigidbody2D>();
        _collision = _collisionObject.GetComponent<Collider2D>();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
    
    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    #endregion
    
    public void OnDealtDamage(int damage, GameObject gameObject)
    {
        Destroy(this.gameObject);
    }
}