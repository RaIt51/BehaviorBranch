using UnityEngine;

namespace Simulation.Objects.Fighters.Actions
{
    /// <summary>
    /// Property for Tackle action
    /// </summary>
    [CreateAssetMenu(
        fileName = "TackleProperty",
        menuName = "ScriptableObjects/Actions/Properties/TackleProperty"
    )]
    public class TackleProperty : ActionProperty
    {
        /// <summary>
        /// Tackle speed
        /// </summary>
        public float speed;

        /// <summary>
        /// Tackle turn speed (degree per second)
        /// </summary>
        public float turnSpeed;

        /// <summary>
        /// Tackle duration
        /// </summary>
        public float duration;

        /// <summary>
        /// Tackle damage
        /// </summary>
        public float damage;

        /// <summary>
        /// Tackle knockback duration
        /// </summary>
        public float knockbackDuration;

        /// <summary>
        /// Tackle knockback speed change on horizontal axis.
        /// Normal direction from the hitted fighter
        /// </summary>
        public float knockbackAccelerationHorizontal;

        /// <summary>
        /// Tackle knockback speed change to upword.
        /// </summary>
        public float knockbackAccelerationUp;
    }
}
