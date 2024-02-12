using UnityEngine;

namespace Simulation.Objects.Fighters.Actions
{
    /// <summary>
    /// Property for IronTail action
    /// </summary>
    [CreateAssetMenu(
        fileName = "IronTailProperty",
        menuName = "ScriptableObjects/Actions/Properties/IronTailProperty"
    )]
    public class IronTailProperty : ActionProperty
    {
        /// <summary>
        /// Damage of the action
        /// </summary>
        public int damage = 10;

        /// <summary>
        /// Trigger name written in Animator
        /// </summary>
        public string animationTrigger = "StartIronTail";

        public string animationEventFinished = "finished";
    }
}
