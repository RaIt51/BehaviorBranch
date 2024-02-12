using Simulation.Objects.Fighters;

namespace AI.BehaviourBranch.Variables
{
    public class Stopwatch : Variable
    {
        public Stopwatch(FighterWatcher fighterWatcher)
            : base(fighterWatcher) { }

        public override float Get()
        {
            BehaviourBranchAI behaviourBranchAI = fighterWatcher.GetComponent<BehaviourBranchAI>();
            return behaviourBranchAI.stopwatch;
        }
    }
}
