using UnityEngine;
using DG.Tweening;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("BGM AudioSources")]
    [SerializeField] AudioSource stageBGMSource;
    [SerializeField] AudioSource bossBGMSource;

    [Header("Fade Settings")]
    [SerializeField] float fadeOutTime = 1.0f;
    [SerializeField] float fadeInTime = 1.0f;

    AudioSource currentSource;
    Tween volumeTween;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 初期状態はすべて止める（volumeは触らない）
        stageBGMSource.Stop();
        bossBGMSource.Stop();
    }

    public void PlayStageBGM()
    {
        ChangeBGM(stageBGMSource);
    }

    public void PlayBossBGM()
    {
        ChangeBGM(bossBGMSource);
    }

    void ChangeBGM(AudioSource nextSource)
    {
        if (currentSource == nextSource)
            return;

        volumeTween?.Kill();

        if (currentSource != null)
        {
            float fromVolume = currentSource.volume;

            volumeTween = currentSource
                .DOFade(0f, fadeOutTime)
                .OnComplete(() =>
                {
                    currentSource.Stop();
                    currentSource.volume = fromVolume; // 元に戻す
                    StartNextBGM(nextSource);
                });
        }
        else
        {
            StartNextBGM(nextSource);
        }
    }

    void StartNextBGM(AudioSource nextSource)
    {
        currentSource = nextSource;

        float targetVolume = currentSource.volume;

        currentSource.volume = 0f;
        currentSource.Play();

        volumeTween = currentSource.DOFade(targetVolume, fadeInTime);
    }
}