using UnityEngine;
using UnityEngine.Serialization;

public class EffectManager : BasicBehaviour
{
    [SerializeField]
    private GameObject _hitEffectPrefab;
    
    public static EffectManager Instance;

    protected override void OnAwake()
    {
        Instance = this;
    }

    public void PlayEffect(Vector3 position)
    {
        Instantiate(_hitEffectPrefab, position, Quaternion.identity);
    }
}