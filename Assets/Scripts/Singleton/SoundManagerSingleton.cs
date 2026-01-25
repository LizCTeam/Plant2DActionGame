using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManagerSingleton : BasicBehaviour
{
    public static SoundManagerSingleton Instance { get; private set; }
    
    [SerializeField]
    private SerializableKeyPair<string, AudioSource>[] _audioSource;
    private Dictionary<string, AudioSource> _audioDic;
    public Dictionary<string, AudioSource> AudioSources => _audioDic ??= _audioSource.ToDictionary(p => p.Key, p => p.Value);
    
    protected override void OnAwake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

    public void PlaySound(string soundName)
    {
        if (AudioSources.ContainsKey(soundName))
        {
            AudioSources[soundName].Play();
        }
    }
}