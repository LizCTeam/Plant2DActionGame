using UnityEngine;

public class Enemy : Character, IDamageable
{

    [SerializeField] protected int _maxHp = 5;

    protected int _hp
    {
        set
        {
            _currentHp = Mathf.Clamp(value, 0, _maxHp);
        }
        get
        {
            return _currentHp;
        }
    }

    protected int _currentHp;
    
    protected override void OnStart()
    {
        _hp = _maxHp;
        base.OnStart();
        
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (this._hp <= 0)
        {
            Destroy(this.gameObject);
        }
       

    }

   

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public void OnDamaged(int damage)
    {
        _hp -= damage;
    }
}