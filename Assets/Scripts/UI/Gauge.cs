using System;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : BasicBehaviour
{
    [SerializeField]
    private ReworkCageBehaviour cage;
    [SerializeField]
    private Image gaugeImage;

    private float _CageDuration
    {
        get
        {
            if (!cage.PlantAttributeData.ContainsKey(cage.CurrentVegetableType)) return 0.0f;
            return cage.PlantAttributeData[cage.CurrentVegetableType].MaxGrowthDuration;
        }
    }

    private float _timeToDuration = 0f;
    
    protected override void OnStart()
    {
        base.OnStart();
        gaugeImage.fillAmount = 0f;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        _timeToDuration = Mathf.InverseLerp(0f, _CageDuration, cage.Timer);
        gaugeImage.fillAmount = _timeToDuration;
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}