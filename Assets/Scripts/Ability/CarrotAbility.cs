using UnityEngine;

public class CarrotAbility : BasicBehaviour, IHasAbility
{
    public GameObject Position;
    public GameObject Projectile;
    public float CarrotSpeed = 8f;
    
    public void UseAbility(Player player)
    {
        var cage = player.reworkCage;
        
        Debug.Log("Used Ability");
        GameObject Obj = Instantiate(Projectile, Position.transform.position, Quaternion.identity);
        CarrotMissile carrot = Obj.GetComponent<CarrotMissile>();
        carrot.speed = CarrotSpeed;

        switch (cage.GrowthLevel)
        {
            case ReworkCageBehaviour.GrowthStage.Nothing:
                return;
            case ReworkCageBehaviour.GrowthStage.Seeding:
                carrot.Damage = cage.PlantAttributeData[VegetableType.Carrot].SeedingDamage;
                break;
            case ReworkCageBehaviour.GrowthStage.Sprout:
                carrot.Damage = cage.PlantAttributeData[VegetableType.Carrot].SproutDamage;
                break;
            case ReworkCageBehaviour.GrowthStage.Mature:
                carrot.Damage = cage.PlantAttributeData[VegetableType.Carrot].MatureDamage;
                break;
        }
    }
}