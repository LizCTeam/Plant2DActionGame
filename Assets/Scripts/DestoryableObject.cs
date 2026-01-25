using UnityEngine;

public class DestoryableObject : BasicBehaviour, IDamageable
{
    public int MaxHp = 1;

    [HideInInspector]
    public int Hp
    {
        set
        {
            _currentHp = Mathf.Clamp(value, 0, MaxHp);
        }
        get
        {
            return _currentHp;
        }
    }
    
    private int _currentHp;
    
    protected override void OnStart()
    {
        base.OnStart();
        Hp = MaxHp;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (this.Hp <= 0)
        {
            SoundManagerSingleton.Instance.PlaySound("Break");
            Destroy(this.gameObject);
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
    
    public void OnDamaged(int damage)
    {
        Hp -= damage;
    }
}