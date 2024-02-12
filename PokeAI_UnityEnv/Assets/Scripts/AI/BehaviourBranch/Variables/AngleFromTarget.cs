using Simulation;
using Simulation.Objects.Fighters;

namespace AI.BehaviourBranch.Variables
{
    public class AngleFromTarget : Variable
    {
        public AngleFromTarget(FighterWatcher fighterWatcher)
            : base(fighterWatcher) { }

        public override float Get()
        {
            float angleAbsoluteEnemyHeading = Utils.VectorToHorizontalAngle(
                fighterWatcher.target.transform.forward
            );

            float angleAbsoluteFromEnemy = Utils.VectorToHorizontalAngle(
                fighterWatcher.transform.position - fighterWatcher.target.transform.position
            );

            return angleAbsoluteEnemyHeading - angleAbsoluteFromEnemy;
        }
    }
}
