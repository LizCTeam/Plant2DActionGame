using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemGyunyu : BasicBehaviour
{
    [SerializeField]
    private int healAmount = 1;
    [SerializeField]
    private AudioSource _healSound;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private Collider2D _collider2D;

    protected override void OnAwake()
    {
        base.OnAwake();
        _collider2D = GetComponent<Collider2D>();
    }
    
    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();
            player.Hp += healAmount;
            StartCoroutine(DestroyAfterPlaySE());
        }
        
    }

    private IEnumerator DestroyAfterPlaySE()
    {
        var color = _spriteRenderer.color.a;
        _healSound.PlayOneShot(_healSound.clip);
        yield return new WaitWhile(() => _healSound.isPlaying);
        gameObject.SetActive(false);
        color = 0;
    }
}