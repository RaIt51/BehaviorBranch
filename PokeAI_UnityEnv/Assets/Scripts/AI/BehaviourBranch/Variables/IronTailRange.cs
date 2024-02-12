using Simulation.Objects.Fighters;

namespace AI.BehaviourBranch.Variables
{
    public class IronTailRange : Variable
    {
        public IronTailRange(FighterWatcher fighterWatcher)
            : base(fighterWatcher) { }

        public override float Get()
        {
            return fighterWatcher.irontailRange;
        }
    }
}
