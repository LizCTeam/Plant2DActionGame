using UnityEngine;

namespace Ability
{
    public class DaikonAbility : BasicBehaviour, IHasAbility
    {
        [SerializeField] private int _weaponUseCounter = 0;
        [SerializeField] private Hitbox _hitbox;
        
        public void UseAbility(Player player)
        {
            var growthState = player.reworkCage.GrowthLevel;
            if (growthState == ReworkCageBehaviour.GrowthStage.Nothing) return;
            player.AvailableWeaponHit += _weaponUseCounter;
            var data = player.reworkCage.PlantAttributeData[VegetableType.Daikon];
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