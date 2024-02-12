using UnityEngine;
using UnityEngine.Events;
using Simulation.Objects.Fighters;
using Simulation.Objects.Fighters.Actions;

namespace Simulation.Objects
{
    /// <summary>
    /// Collider attatched to fighter during action.
    /// </summary>
    public class ActionCollider : MonoBehaviour
    {
        /// <summary>
        /// FighterAction that using this collider
        /// </summary>
        private FighterAction action;

        /// <summary>
        /// Will be called when hit another fighter
        /// </summary>
        private UnityAction<Fighter> onHit;

        /// <summary>
        /// Fighter that owns the action
        /// </summary>
        private Fighter fighter
        {
            get { return action.fighter; }
        }

        public void OnTriggerEnter(Collider other)
        {
            //if hit another fighter...
            if ((other.gameObject.GetComponent<Fighter>() != null)
                &&(other.gameObject.GetComponent<Fighter>() != fighter))
            {
                //call registered function
                onHit(other.gameObject.GetComponent<Fighter>());
            }
        }

        /// <summary>
        /// Must call this when generating
        /// </summary>
        public void Initialize(FighterAction action, UnityAction<Fighter> onHit)
        {
            this.action = action;
            this.onHit = onHit;
        }
    }
}
