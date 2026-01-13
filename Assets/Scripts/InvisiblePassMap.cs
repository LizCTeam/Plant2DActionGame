using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InvisiblePassMap : BasicBehaviour
{
    private Tilemap _tilemap;
    private Renderer _tilemapRenderer;

    protected override void OnAwake()
    {
        _tilemap = GetComponent<Tilemap>();
        _tilemapRenderer = GetComponent<TilemapRenderer>();
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
        if (collision.CompareTag("Player"))
        {
            this._tilemapRenderer.material.DOFade(0.1f, 0.5f).SetEase(Ease.InQuad);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        this._tilemapRenderer.material.DOFade(1f, 0.5f).SetEase(Ease.InQuad);
    }
}