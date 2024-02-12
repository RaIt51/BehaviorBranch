using UnityEngine;
using Simulation.Objects.Fighters;

namespace Simulation.Objects
{
    /// <summary>
    /// Bullet that shot from Thunderbolt action
    /// </summary>
    public class ThunderboltBullet : Bullet
    {
        /// <summary>
        /// Call this when instantiate
        /// </summary>
        public void Initialize(
            BattleObject lancher,
            int damage,
            float speed,
            Vector3 direction,
            float maxDistance
        )
        {
            this.lancher = lancher;
            this.damage = damage;
            this.speed = speed;
            this.direction = direction;
            this.maxDistance = maxDistance;
            
            staggerLevel = Fighter.StaggerLevel.heavy;
        }

        protected override void Fly()
        {
            FlyUniformLinearMotion();
        }
    }
}
