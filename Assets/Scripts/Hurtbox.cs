using System;
using System.Linq;
using UnityEngine;

public class Hurtbox : BasicBehaviour
{
    public GameObject owner;
    // IDamageableはインターフェース、このインターフェースを継承したclassは必ず
    // OnDamaged関数を実装しなくちゃいけない
    // これによってオブジェクト、ボス、敵同じHurtboxを使っても違う表現が出来る
    [HideInInspector]
    public Collider2D collider;
    private IDamageable _damageable;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        
        if (owner)
        {
            // GetComponents系は遅いからAwakeでやる
            _damageable = owner.GetComponents(typeof(IDamageable)).FirstOrDefault() as IDamageable;
        }
    }

    public void Hurt(int damage)
    {
        // ownerがIDamageableであればダメージを与える
        _damageable?.OnDamaged(damage);
    }
}