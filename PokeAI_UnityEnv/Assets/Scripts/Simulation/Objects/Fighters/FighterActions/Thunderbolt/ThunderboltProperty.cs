using UnityEngine;

namespace Simulation.Objects.Fighters.Actions
{
    [CreateAssetMenu(
        fileName = "ThunderboltProperty",
        menuName = "ScriptableObjects/Actions/Properties/ThunderboltProperty"
    )]
    /// <summary>
    /// Property of Thunderbolt action
    /// </summary>
    public class ThunderboltProperty : ActionProperty
    {
        public int damage = 30;
        public float speed = 10f;
        public ThunderboltBullet bulletPrefab;
        public float maxDistance = 100;
    }
}
