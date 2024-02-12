using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.Objects
{
    /// <summary>
    /// Object apeears in the simulated world.
    /// </summary>
    public class BattleObject : MonoBehaviour
    {
        protected List<AttackingObject> attackingObjects = new List<AttackingObject>();

        protected virtual void Awake() { }

        protected virtual void Start() { }

        protected virtual void Update() { }

        /// <summary>
        /// Gives [0, 1] ratio of current_hp/max_hp
        /// </summary>
        public virtual float GetHPRatio()
        {
            throw new NotImplementedException();
        }

        public void RegisterAttackingObject(AttackingObject attackingObject)
        {
            attackingObjects.Add(attackingObject);
        }

        public AttackingObject[] GetAttackingObjects()
        {
            //rid destroyed objects
            attackingObjects.RemoveAll(attackingObject => attackingObject == null);

            return attackingObjects.ToArray();
        }
    }
}
