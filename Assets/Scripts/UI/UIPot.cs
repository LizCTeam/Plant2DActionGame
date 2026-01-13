using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIPot : BasicBehaviour
{
    [SerializeField]
    private Image[] plantImages;
    [SerializeField]
    private ReworkCageBehaviour cage;
    
    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (cage.CurrentVegetableType == VegetableType.Corn)
        {
            plantImages[0].enabled = true;
            plantImages[1].enabled = false;
            plantImages[2].enabled = false;
        }
        else if (cage.CurrentVegetableType == VegetableType.Carrot)
        {
            plantImages[0].enabled = false;
            plantImages[1].enabled = true;
            plantImages[2].enabled = false;
        }
        else if (cage.CurrentVegetableType == VegetableType.Daikon)
        {
            plantImages[0].enabled = false;
            plantImages[1].enabled = false;
            plantImages[2].enabled = true;
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}