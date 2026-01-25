using UnityEngine;
using UnityEngine.Serialization;

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
            GameResultSingleton.Instance?.IncrementKillCount();
            SoundManagerSingleton.Instance.PlaySound("Hurt");
            Destroy(this.gameObject);
        }
       

    }

   

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public void OnDamaged(int damage)
    {
        SoundManagerSingleton.Instance.PlaySound("Hurt");
        EffectManager.Instance.PlayEffect(transform.position);
        _hp -= damage;
    }
}