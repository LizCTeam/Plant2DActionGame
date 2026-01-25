using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : BasicBehaviour
{
    public int damage;

    public GameObject owner;

    public bool isContinuousDamage;
    
    private readonly Dictionary<Collider2D, Coroutine> _hitEvents = new();

    private IDamageDealt _damageDealt;

    private Collider2D _collider;

    private void Awake()
    {
        if (owner)
        {
            _damageDealt = owner.GetComponent(nameof(IDamageDealt)) as IDamageDealt;
        }
        _collider = GetComponent<Collider2D>();
    }
    
    // 注意OnTriggerEnterではなくOnTriggerEnter"2D" (1敗)
    // Unity上でIs TriggerをONにすることでGodotのArea2Dみたいな使い方が出来る
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isContinuousDamage)
        {
            // 持続ダメージの例、ここ一旦スキップしてもいい
            DoMultipleHit(other);
        }
        else
        {
            // 通常の使い方、持続ダメージがない
            DoBasicHit(other);   
        }
    }

    private void DoBasicHit(Collider2D other)
    {
        var hurtbox = other.GetComponent<Hurtbox>();
        hurtbox?.Hurt(damage);
        _damageDealt?.OnDealtDamage(damage, other.gameObject);
    }

    private void DoMultipleHit(Collider2D other)
    {
        var hurtbox = other.GetComponent<Hurtbox>();
        if (hurtbox ==null) return;
        
        // Coroutineは途中で止めてまた続けられる処理
        Coroutine coroutine = StartCoroutine(DamageCoroutine(hurtbox));
        // 複数のHurtboxが入る可能性があるからDictionaryを使ってCoroutineを記録する
        _hitEvents.Add(other, coroutine);
    }
    
    // IEnumeratorを返す関数はCoroutineの宣言
    private IEnumerator DamageCoroutine(Hurtbox hurtbox)
    {
        while (true)
        {
            // yield return使うことによってこの関数を一時停止出来る
            hurtbox?.Hurt(damage);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_hitEvents.ContainsKey(other))
        {
            // 該当Hurtboxが離れたらDictionaryに記録したCoroutineを停止させる
            StopCoroutine(_hitEvents[other]);
            _hitEvents.Remove(other);
        }
    }

    // private void OnEnable()
    // {
    //     var filter = new ContactFilter2D();
    //     var results = new List<Collider2D>();
    //     _collider.Overlap(filter, results);
    //     foreach (var other in results)
    //     {
    //         OnTriggerEnter2D(other);
    //     }
    // }
    //
    // private void OnDisable()
    // {
    //     foreach (var hitEvent in _hitEvents)
    //     {
    //         StopCoroutine(hitEvent.Value);
    //     }
    //     _hitEvents.Clear(); 
    // }
}
