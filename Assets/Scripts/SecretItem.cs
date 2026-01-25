using System;
using UnityEngine;

public class SecretItem : BasicBehaviour
{
    protected override void OnStart()
    {
        base.OnStart();
        GameResultSingleton.Instance.MaxSecretCount += 1;
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
            SoundManagerSingleton.Instance.PlaySound("SecretFound");
            GameResultSingleton.Instance.SecretCount += 1;
            Destroy(gameObject);
        }
    }
}