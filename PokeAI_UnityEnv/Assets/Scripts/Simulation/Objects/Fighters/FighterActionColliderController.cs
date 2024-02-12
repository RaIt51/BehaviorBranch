using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Simulation.Objects.Fighters.Actions;

namespace Simulation.Objects.Fighters
{
    /// <summary>
    /// Controls ActionCollider of a fighter
    /// </summary>
    public class FighterActionColliderController
    {
        /// <summary>
        /// Set prefabs of colliders for the fighter
        /// </summary>
        [Serializable]
        public class ActionColliderPrefabs
        {
            [Header("Colliders Prefabs")]
            public ActionCollider colliderBody;
            public ActionCollider colliderTail;

            [Header("Parent Transform of Colliders")]
            public Transform parentBody;
            public Transform parentTail;
        }

        /// <summary>
        /// Part name of the collider
        /// </summary>
        [Serializable]
        public enum ColliderPart
        {
            body,
            tail
        }

        /// <summary>
        /// Prefabs list of colliders
        /// </summary>
        private ActionColliderPrefabs prefabs;

        /// <summary>
        /// Mex of colliders ID
        /// </summary>
        private int mexId = 0;

        /// <summary>
        /// ID -> collider that currently generating
        /// </summary>
        private Dictionary<int, ActionCollider> colliders = new Dictionary<int, ActionCollider>();

        /// <summary>
        /// Set instance used in Fighter
        /// </summary>
        public FighterActionColliderController(ActionColliderPrefabs prefabs)
        {
            this.prefabs = prefabs;
        }

        /// <summary>
        /// Generate collider for the action
        ///
        /// Also make an ID for the collider
        /// </summary>
        /// <param name="fighterAction">what FighterAction this invoked by</param>
        /// <param name="part">which collider going to be generated</param>
        /// <returns>ID of the generated collider. Must remember this to delete</returns>
        public int GenerateCollider(
            FighterAction fighterAction,
            ColliderPart part,
            UnityAction<Fighter> onHit
        )
        {
            //id for the collider going to be generated
            int id = GetNewID();

            //generate GameObject
            colliders[id] = UnityEngine.Object
                .Instantiate(GetPrefab(part), GetParent(part))
                .GetComponent<ActionCollider>();

            //set parameters
            colliders[id].Initialize(fighterAction, onHit);

            return id;
        }

        /// <summary>
        /// Delete collider by ID
        /// </summary>
        /// <param name="id">ID of collider</param>
        /// <returns>whether there was corresponding id exists</returns>
        public bool DeleteCollider(int id)
        {
            if (colliders.ContainsKey(id))
            {
                UnityEngine.Object.Destroy(colliders[id].gameObject);
                colliders.Remove(id);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get new ID for collider by using mex
        /// </summary>
        public int GetNewID()
        {
            int returning = mexId;

            //find mex of unused ID
            mexId++;
            while (colliders.ContainsKey(mexId))
            {
                mexId++;
            }

            return returning;
        }

        /// <summary>
        /// Get prefab of the collider from ColliderPart enum
        /// </summary>
        private ActionCollider GetPrefab(ColliderPart part)
        {
            switch (part)
            {
                case ColliderPart.body:
                    return prefabs.colliderBody;
                case ColliderPart.tail:
                    return prefabs.colliderTail;
                default:
                    return null;
            }
        }

        private Transform GetParent(ColliderPart part)
        {
            switch (part)
            {
                case ColliderPart.body:
                    return prefabs.parentBody;
                case ColliderPart.tail:
                    return prefabs.parentTail;
                default:
                    return null;
            }
        }
    }
}
