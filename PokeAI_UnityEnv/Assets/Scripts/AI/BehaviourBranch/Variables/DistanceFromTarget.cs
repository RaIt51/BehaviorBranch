using Simulation.Objects.Fighters;

namespace AI.BehaviourBranch.Variables
{
    public class DistanceFromTarget : Variable
    {
        public DistanceFromTarget(FighterWatcher fighterWatcher)
            : base(fighterWatcher) { }

        public override float Get()
        {
            return fighterWatcher.distanceFromTarget;
        }
    }
}
