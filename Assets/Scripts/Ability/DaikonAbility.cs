using UnityEngine;

namespace Ability
{
    public class DaikonAbility : BasicBehaviour, IHasAbility
    {
        [SerializeField] private Hitbox _hitbox;
        
        public void UseAbility(Player player)
        {
            var growthState = player.reworkCage.GrowthLevel;
            if (growthState == ReworkCageBehaviour.GrowthStage.Nothing) return;
            var data = player.reworkCage.PlantAttributeData[VegetableType.Daikon];
            player.AvailableWeaponHit = data.SeedingSwingCount;
            _hitbox.damage = growthState switch
            {
                ReworkCageBehaviour.GrowthStage.Seeding => data.SeedingDamage,
                ReworkCageBehaviour.GrowthStage.Sprout => data.SproutDamage,
                ReworkCageBehaviour.GrowthStage.Mature => data.MatureDamage,
                _ => _hitbox.damage
            };
            
        }
    }
}