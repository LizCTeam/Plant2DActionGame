using System.Collections;
using UnityEngine;

public class ProjectileBehaviour : BasicBehaviour, IDamageDealt
{
    [SerializeField] 
    private GameObject _collisionObject;
    private Collider2D _collision;
    
    public float speed = 4.5f;
    private Rigidbody2D _body;
    
    #region 発射物の関数

    protected override void OnAwake()
    {
        base.OnAwake();
        _body = GetComponent<Rigidbody2D>();
        _collision = _collisionObject.GetComponentInChildren<Collider2D>();
    }

    protected override void OnStart()
    {
        base.OnStart();
        _collision.enabled = false;
        StartCoroutine(DelayEnableCollision(0.1f));
    }

    private IEnumerator DelayEnableCollision(float delay)
    {
        yield return new WaitForSeconds(delay); 
        _collision.enabled = true;
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