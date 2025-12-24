using UnityEngine;

public class CornAbility : BasicBehaviour, IHasAbility
{
    public GameObject Parent;
    public GameObject Position;
    public GameObject CornObject;
    
    public void UseAbility(Player player)
    {
        var cage = player.reworkCage;
        
        Debug.Log("Used Ability 2");
        var Obj = Instantiate(CornObject, Position.transform.position, Quaternion.identity);
        var scale = Obj.transform.localScale;
        scale.x = Mathf.Sign(Parent.transform.localScale.x) * Mathf.Abs(Parent.transform.localScale.x);
        Obj.transform.localScale = scale;
        CornDrill corn = Obj.GetComponent<CornDrill>();
        
        //corn.transform.parent = Parent.transform;
        
        switch (cage.GrowthLevel)
        {
            case ReworkCageBehaviour.GrowthStage.Nothing:
                return;
            case ReworkCageBehaviour.GrowthStage.Seeding:
                corn.Damage = cage.PlantAttributeData[VegetableType.Corn].SeedingDamage;
                corn._scaleMultiplier = cage.PlantAttributeData[VegetableType.Corn].SeedingScale;
                break;
            case ReworkCageBehaviour.GrowthStage.Sprout:
                corn.Damage = cage.PlantAttributeData[VegetableType.Corn].SproutDamage;
                corn._scaleMultiplier = cage.PlantAttributeData[VegetableType.Corn].SproutScale;
                break;
            case ReworkCageBehaviour.GrowthStage.Mature:
                corn.Damage = cage.PlantAttributeData[VegetableType.Corn].MatureDamage;
                corn._scaleMultiplier = cage.PlantAttributeData[VegetableType.Corn].MatureScale;
                break;
        }
    }
}