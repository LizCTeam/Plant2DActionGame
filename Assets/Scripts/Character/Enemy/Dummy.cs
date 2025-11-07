using UnityEngine;
using DG.Tweening;

public class Dummy : Enemy, IDamageable
{
    public TextMesh DamageText;

    public void OnDamaged(int damage)
    {
        DamageText.transform.localScale *= 1.2f;
        DamageText.transform.DOScale(1.0f, 0.5f).SetEase(Ease.OutQuart);
        DamageText.text = "Damage : " + damage;
    }
}