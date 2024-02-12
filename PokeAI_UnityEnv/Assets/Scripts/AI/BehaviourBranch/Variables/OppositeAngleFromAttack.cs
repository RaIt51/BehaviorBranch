using System.Linq;
using UnityEngine;
using Simulation.Objects;
using Simulation.Objects.Fighters;
using Simulation;

namespace AI.BehaviourBranch.Variables
{
    public class OppositeAngleFromAttack : Variable
    {
        public OppositeAngleFromAttack(FighterWatcher fighterWatcher)
            : base(fighterWatcher) { }

        public override float Get()
        {
            AttackingObject[] attackingObjectsEnemy = fighterWatcher.target.GetAttackingObjects();

            //if no attacking object...
            if (attackingObjectsEnemy.Length == 0)
            {
                //...return 90 = right hand side
                return 0f;
            }

            //find nearest attacking object
            AttackingObject nearestAttackingObject = attackingObjectsEnemy
                .OrderBy(
                    attackingObject =>
                        Vector3.Distance(
                            attackingObject.gameObject.transform.position,
                            fighterWatcher.transform.position
                        )
                )
                .First();

            //get evading direction
            Vector3 evadingDirection = GetEvadingDirection(nearestAttackingObject);

            //get angle
            return GetAngle(
                evadingDirection,
                nearestAttackingObject.gameObject.transform.position - fighterWatcher.position
            );
        }

        private Vector3 GetEvadingDirection(AttackingObject attackingObject)
        {
            Vector3 positionRelative =
                fighterWatcher.position - attackingObject.gameObject.transform.position;
            Vector3 supportVector = Vector3.Cross(
                positionRelative,
                attackingObject.attackingDirection
            );
            Vector3 evadingDirection = Vector3.Cross(
                supportVector,
                attackingObject.attackingDirection
            );

            return evadingDirection;
        }

        private float GetAngle(Vector3 directionEvading, Vector3 positionRelative)
        {
            float angleEvading = Utils.VectorToHorizontalAngle(directionEvading);
            float angleRelative = Utils.VectorToHorizontalAngle(positionRelative);

            return angleEvading - angleRelative;
        }
    }
}
