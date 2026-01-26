using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Teleport : BasicBehaviour
{
    [Header("カメラの設定")]
    public Camera MainCamera;
    public Camera BossCamera;
    
    [Header("位置")]
    [SerializeField]
    private Transform _playerPosition;
    [SerializeField]
    private Transform _bossPosition;
    [SerializeField]
    private GameObject _fallingRocks;

    [Header("キャンバスの設定")]
    [SerializeField] 
    private CanvasResult _canvasResult;

    [SerializeField] 
    private BearBoss _bearBoss;

    [SerializeField] 
    private GameObject _goal;
    
    private Player _player;
    
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.GetComponent<Player>();
            StartCoroutine(TeleportCoroutine());
        }
    }

    private IEnumerator TeleportCoroutine()
    {
        _canvasResult.FadeIn();
        yield return new WaitUntil((() => _canvasResult.animator.GetCurrentAnimatorStateInfo(0).IsName("FadeIn")));
        yield return new WaitForAnimation(_canvasResult.animator, 0, "FadeIn");
        MainCamera.gameObject.SetActive(false);
        BossCamera.gameObject.SetActive(true);
        _canvasResult.FadeOut();
        _player.gameObject.transform.position = _playerPosition.position;
        yield return new WaitUntil((() => _canvasResult.animator.GetCurrentAnimatorStateInfo(0).IsName("FadeOut")));
        yield return new WaitForAnimation(_canvasResult.animator, 0, "FadeOut");
        BGMManager.Instance.PlayBossBGM();
        yield return new WaitForSeconds(1f);
        _bearBoss._fallingRocks = _fallingRocks;
        Instantiate(_bearBoss, _bossPosition);
    }
}