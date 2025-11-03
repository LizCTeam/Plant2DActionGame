using UnityEngine;

public class BasicBehaviour : MonoBehaviour
{
    void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        // 親クラスの初期化処理
    }

    void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
        // 親クラスのStart処理
    }

    void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
        // 親クラスのUpdate処理
    }

    void FixedUpdate()
    {
        OnFixedUpdate();
    }

    protected virtual void OnFixedUpdate()
    {
        
    }
}