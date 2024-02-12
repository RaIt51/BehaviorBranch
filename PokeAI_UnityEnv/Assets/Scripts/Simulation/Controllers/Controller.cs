using UnityEngine;
using Simulation.Objects;

namespace Simulation
{
    /// <summary>
    /// Controlling controllable objects. This could be manual or AI.
    /// </summary>
    public abstract class Controller : MonoBehaviour
    {
        /// <summary>
        /// Give this controller a target to control
        /// </summary>
        /// <param name="target"></param>
        public abstract void AllocateTarget(Controllable target);
    }
}
