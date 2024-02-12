using UnityEngine;

namespace Simulation.Objects
{
    public interface AttackingObject
    {
        public Vector3 attackingDirection { get; }
        public GameObject gameObject { get; }
    }
}
